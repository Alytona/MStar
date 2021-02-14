﻿using System;
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

        public bool parseScriptLine (string line)
        {
            if (line.Trim().Length == 0 || line.StartsWith( "#" )
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
                            addEraseCommand( line );
                            break;

                        case "write.boot":
                            addWriteBootCommand( line );
                            break;

                        case "unlzo":
                        case "unlzo.cont":
                            addUnlzoCommand( line );
                            break;

                        case "store_secure_info":
                            addStoreSecureInfoCommand( line );
                            break;

                        case "store_nutxx_config":
                            addStoreNutxxConfigCommand( line );
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
            if (LastInformationalBlock == null)
            {
                LastInformationalBlock = new InformationalBlock();
                Elements.Add( LastInformationalBlock );
            }
            LastInformationalBlock.addString( line );

            if (line.StartsWith( "setenv" ))
            {
                string[] setEnvTokens = line.Split( ' ' );
                if (setEnvTokens.Length > 1 && (setEnvTokens[1] == "CEnv_UpgradeCRC_Tmp" || setEnvTokens[1] == "CEnv_UpgradeCRC_Val"))
                {
                    CrcType = CrcTypes.Third;
                }
            }
        }
        public void addEraseCommand (string line)
        {
            Elements.Add( new EraseCommand( line.Split( ' ' ) ) );
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
        public void addStoreNutxxConfigCommand (string line)
        {
            addWriteFileCommand( new StoreNutxxConfigCommand( LastFilePartLoadCommand, line.Split( ' ' ) ) );
        }
        void addWriteFileCommand (WriteFileCommand command)
        {
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

        static DataContractJsonSerializer Serializer = new DataContractJsonSerializer( typeof( ScriptElementsHolder ) );
        public void saveTo (StreamWriter writer)
        {
            Serializer.WriteObject( writer.BaseStream, this );
            writer.WriteLine();
            writer.Flush();

            //foreach (IHeaderScriptElement element in Elements)
            //    element.saveTo( writer );
        }

        public void saveTo (string filename)
        {
            using (StreamWriter writer = new StreamWriter( filename, false, System.Text.Encoding.GetEncoding( 1251 ) ))
            {
                foreach (IHeaderScriptElement element in Elements)
                    element.writeToHeader( writer );
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
                        if (!parseScriptLine( line ))
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
/*
            foreach (PartitionCreateCommand command in PartitionsList.PartitionCreateCommands.Values)
            {
                Partition partition;
                if (!partitions.TryGetValue( command.Name, out partition ))
                {
                    partition = new Partition( command );
                    partitions.Add( command.Name, partition );
                }
                partition.Size = Convert.ToInt64( command.Size, 16 );
            }
*/
            return partitions;
        }
    }
}