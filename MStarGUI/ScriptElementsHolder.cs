using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace MStarGUI
{
    [DataContract]
    class ScriptElementsHolder
    {
        public List<IHeaderScriptElement> Elements {
            get;
            private set;
        } = new List<IHeaderScriptElement>();

        InformationalBlock LastInformationalBlock = null;
        FilePartLoadCommand LastFilePartLoadCommand = null;

        WriteFileCommand LastWriteFileCommand = null;
        List<IHeaderScriptElement> WritePrologCommands = new List<IHeaderScriptElement>();

        int BootCounter = 0;
        public int PostEnvIndex 
        {
            get; private set;
        } = -1;

        public PartionsListSection PartitionsList
        {
            get; private set;
        } = new PartionsListSection();

        bool PartitionsListAdded = false;

        void addPartitionsListIfNotAdded ()
        {
            if (!PartitionsListAdded)
            {
                PartitionsListAdded = true;
                Elements.Add( PartitionsList );
            }
        }

        public enum CrcTypes
        {
            First = 1,
            Second,
            Third
        }
        [DataMember]
        public CrcTypes CrcType
        {
            get;
            private set;
        } = CrcTypes.First;

        [DataMember]
        public string Slc
        {
            get => PartitionsList.Slc;
            set
            {
                PartitionsList.Slc = value;
                addPartitionsListIfNotAdded();
            }
        }
        [DataMember]
        public bool Rgmpt
        {
            get => PartitionsList.Rmgpt;
            set
            {
                PartitionsList.Rmgpt = value;
                addPartitionsListIfNotAdded();
            }
        }

        public uint FirmwareBodyCrc
        {
            get; private set;
        }
        public void setFirmwareBodyCrc(uint crc)
        {
            FirmwareBodyCrc = crc;
            //if (CrcType == CrcTypes.Third)
            {
                InformationalBlock thirdCrcBlock = null;
                for (int i = 0; i < Elements.Count && thirdCrcBlock == null; i++ )
                {
                    if (Elements[i] is WriteFileCommand writeFileCommand && writeFileCommand.Epilog != null && writeFileCommand.Epilog.hasThirdCrc)
                        thirdCrcBlock = writeFileCommand.Epilog;
                    if (Elements[i] is InformationalBlock informationalBlock && informationalBlock.hasThirdCrc)
                        thirdCrcBlock = informationalBlock;
                }
                if (thirdCrcBlock != null) 
                {
                    CrcType = CrcTypes.Third;
                    thirdCrcBlock.setThirdCrc( crc );
                }
            }
        }

        public void setTitle (string title)
        {
            InformationalBlock info;
            if (!(Elements[0] is InformationalBlock))
            {
                info = new InformationalBlock();
                Elements.Insert( 0, info );
            }
            else {
                info = Elements[0] as InformationalBlock;
            }
            info.setTitle( title );
        }

        public bool parseScriptLine (string line)
        {
            if (line.Trim().Length == 0 || line.StartsWith( "#" ) || line.StartsWith( "set " )
                || line.StartsWith( "setenv" ) || line.StartsWith( "saveenv" ) || line.StartsWith( "printenv" ))
            {
                addInformationalString( line );
            }
            else
            {
                if (line.StartsWith( "mmc" ))
                {
                    string[] mmcTokens = line.Split( ' ' );
                    switch (mmcTokens[1])
                    {
                        case "slc":
                            Slc = line;
                            break;

                        case "rmgpt":
                            Rgmpt = true;
                            break;

                        case "create":
                            addPartitionCreateCommand( mmcTokens[2], mmcTokens[3], mmcTokens.Length >= 5 && mmcTokens[4] == "xgi" );
                            break;

                        case "write.p":
                            addWritePiCommand( line );
                            break;

                        case "erase.p":
                        case "erase.part":
                            addEraseCommand( line );
                            break;

                        case "write.boot":
                            addWriteBootCommand( line );
                            break;

                        case "unlzo":
                        case "unlzo.cont":
                            addUnlzoCommand( line );
                            break;
                    }
                }
                else if (line.StartsWith( "filepartload" ))
                {
                    addFilepartLoadCommand( line );
                }
                else if (line.StartsWith( "sparse_write" ))
                {
                    addSparseWriteCommand( line );
                }
                else if (line.StartsWith( "store_secure_info" )) 
                {
                    addStoreSecureInfoCommand( line );
                }
                else if (line.StartsWith( "store_nuttx_config" ))
                {
                    addStoreNuttxConfigCommand( line );
                }
                else if (line.StartsWith( "%" ))
                {
                    addInformationalString( line );
                    return false;
                }
            }
            return true;
        }

        public void addInformationalString (string line)
        {
            if (LastWriteFileCommand != null)
            {
                if (LastInformationalBlock == null)
                    LastInformationalBlock = new InformationalBlock();

                if (line.StartsWith( "setenv" ))
                {
                    LastWriteFileCommand.Epilog = LastInformationalBlock;
                }
                else if (line.StartsWith( "saveenv" ))
                {
                    LastWriteFileCommand.Epilog = LastInformationalBlock;
                    LastInformationalBlock.addString( line );
                    LastWriteFileCommand = null;
                    LastInformationalBlock = new InformationalBlock();
                    Elements.Add( LastInformationalBlock );
                    return;
                }
                else
                {
                    LastWriteFileCommand = null;
                    LastInformationalBlock = new InformationalBlock();
                    Elements.Add( LastInformationalBlock );
                }
            }
            else {
                if (LastInformationalBlock == null)
                {
                    LastInformationalBlock = new InformationalBlock();
                    Elements.Add( LastInformationalBlock );
                }
            }

            LastInformationalBlock.addString( line );
            if (line.StartsWith( "setenv" ))
            {
                string[] setEnvTokens = line.Split( ' ' );
                if (setEnvTokens.Length > 1 && (setEnvTokens[1] == "CEnv_UpgradeCRC_Tmp" || setEnvTokens[1] == "CEnv_UpgradeCRC_Val"))
                {
                    LastInformationalBlock.hasThirdCrc = true;
                    CrcType = CrcTypes.Third;
                }
            }
        }
        public void addEraseCommand (string line)
        {
            EraseCommand command = new EraseCommand( line.Split( ' ' ) );
            WritePrologCommands.Add( command );
        }
        public void addPartitionCreateCommand (string name, string size, bool xgi)
        {
            if (xgi)
                CrcType = CrcTypes.Second;
            PartitionsList.addPartitionCreateCommand( name, size );
            addPartitionsListIfNotAdded();
            LastInformationalBlock = null;
        }
        public void addFilepartLoadCommand (string line)
        {
            LastFilePartLoadCommand = new FilePartLoadCommand( line.Split( ' ' ) );
            WritePrologCommands.Add( LastFilePartLoadCommand );
            LastInformationalBlock = null;
        }
        public void addWritePiCommand (string line)
        {
            addWriteFileCommand( new WritePiCommand( LastFilePartLoadCommand, line.Split( ' ' ) ) );
        }
        public void addWriteBootCommand (string line)
        {
            addWriteFileCommand( new WriteBootCommand( LastFilePartLoadCommand, line.Split( ' ' ), ++BootCounter ) );
        }
        public void addSparseWriteCommand (string line)
        {
            addWriteFileCommand( new SparseWriteCommand( LastFilePartLoadCommand, line.Split( ' ' ) ) );
        }
        public void addUnlzoCommand (string line)
        {
            addWriteFileCommand( new UnlzoCommand( LastFilePartLoadCommand, line.Split( ' ' ) ) );
        }
        public void addStoreSecureInfoCommand (string line)
        {
            addWriteFileCommand( new StoreSecureInfoCommand( LastFilePartLoadCommand, line.Split( ' ' ) ) );
        }
        public void addStoreNuttxConfigCommand (string line)
        {
            addWriteFileCommand( new StoreNuttxConfigCommand( LastFilePartLoadCommand, line.Split( ' ' ) ) );
        }
        void addWriteFileCommand (WriteFileCommand command)
        {
            command.Prolog = WritePrologCommands;
            WritePrologCommands = new List<IHeaderScriptElement>();
            LastWriteFileCommand = command;

            Elements.Add( command );
            LastFilePartLoadCommand = null;
            LastInformationalBlock = null;
            PostEnvIndex = Elements.Count;
        }

        public List< WriteFileCommand > getWriteCommands ()
        {
            List<WriteFileCommand> result = new List<WriteFileCommand>();
            foreach (IHeaderScriptElement element in Elements) 
            {
                if (element is WriteFileCommand command)
                    result.Add( command );
            }
            return result;
        }

        public void saveTo (string filename)
        {
            using (StreamWriter writer = new StreamWriter( filename, false, Encoding.GetEncoding( 1251 ) ))
                writeTo( writer );
        }
        public void writeTo (FileStream outputStream)
        {
            using (StreamWriter writer = new StreamWriter( outputStream ))
                writeTo( writer );
        }
        public void writeTo (StreamWriter writer)
        {
            writer.NewLine = "\x0a";
            foreach (IHeaderScriptElement element in Elements)
                element.writeToHeader( writer );
        }
        public void writeSelectedPartitionsTo (FileStream outputStream)
        {
            int postEnvIndex = -1;
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i] is WriteFileCommand)
                    postEnvIndex = i + 1;
            }

            bool partitionsStart = false;
            using (StreamWriter writer = new StreamWriter( outputStream ))
            {
                writer.NewLine = "\x0a";
                for (int i = 0; i < Elements.Count; i++)
                {
                    if (Elements[i] is PartionsListSection)
                        partitionsStart = true;
                    else
                    {
                        if (!partitionsStart || i >= postEnvIndex || Elements[i] is EraseCommand || Elements[i] is WriteFileCommand)
                            Elements[i].writeToHeader( writer );
                    }
                }
            }
        }

        public bool loadFrom (string filename, IMessageLogger logger)
        {
            using (StreamReader reader = new StreamReader( filename ))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    try
                    {
                        if (!parseScriptLine( line.Trim() ))
                            break;
                    }
                    catch (Exception error)
                    {
                        // Вывод ошибки в протокол
                        logger.logMessage( "> " + line );
                        logger.logMessage( error.Message );
                        return false;
                    }
                }
            }
            return true;
        }

        public Dictionary<string, Partition> getPartitions ()
        {
            Dictionary<string, Partition> partitions = new Dictionary<string, Partition>();
            foreach (WriteFileCommand command in getWriteCommands())
            {
                Partition partition;
                if (!partitions.TryGetValue( command.PartitionName, out partition ))
                {
                    partition = new Partition( command );
                    partitions.Add( command.PartitionName, partition );
                }
                partition.addChunk( command );
            }
            return partitions;
        }
    }
}
