using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MStarGUI
{
    class FirmwareBuilder
    {
        readonly string Title;
        readonly string Directory;

        public FirmwareBuilder (string directory, string title)
        {
            Directory = directory;
            Title = title;
        }

        public void pack (ScriptElementsHolder scriptHolder, Dictionary<string, Partition> partitions, IMessageLogger messageLogger)
        {
            ScriptElementsHolder newScriptHolder = new ScriptElementsHolder();
            bool fullPackage = true;

            List<string> writtenPartitions = new List<string>();

            string newFirmwareFilename = Directory + ".bin";
            newFirmwareFilename = Path.Combine( Path.GetDirectoryName( newFirmwareFilename ), "new_" + Path.GetFileName( newFirmwareFilename ) );

            using (FileStream outputStream = new FileStream( newFirmwareFilename, FileMode.Create, FileAccess.Write ))
            {
                messageLogger.logMessage( "Сборка прошивки." );

                for (int i = 0; i < 16 * 1024; i++)
                    outputStream.WriteByte( 0xFF );

                foreach (IHeaderScriptElement element in scriptHolder.Elements)
                {
                    if (element is InformationalBlock || element is PartionsListSection)
                    {
                        newScriptHolder.Elements.Add( element );
                    }
                    else if (element is WriteFileCommand command)
                    {
                        if (writtenPartitions.Contains( command.PartitionName ))
                            continue;
                        writtenPartitions.Add( command.PartitionName );

                        if (partitions.TryGetValue( command.PartitionName, out Partition partition ) && File.Exists( Path.Combine( Directory, command.PartitionName + ".img" ) ))
                        {
                            var newChunks = partition.pack( Directory, outputStream, messageLogger );
                            newScriptHolder.Elements.AddRange( newChunks );
                        }
                        else 
                        {
                            fullPackage = false;
                        }
                    }
                }

                messageLogger.logMessage( "Расчет контрольной суммы бинарной части прошивки." );
                newScriptHolder.setFirmwareBodyCrc( Partition.computeCrc32( outputStream.Name, 16 * 1024, outputStream.Length - 16 * 1024 ) );

                messageLogger.logMessage( "Добавление штампа в заголовок." );
                if (!string.IsNullOrEmpty( Title ))
                    newScriptHolder.setTitle( Title );

                messageLogger.logMessage( "Запись заголовка." );
                outputStream.Seek( 0, SeekOrigin.Begin );
                if (fullPackage)
                    newScriptHolder.writeTo( outputStream );
                else
                    newScriptHolder.writeSelectedPartitionsTo( outputStream );
            }

            using (FileStream outputStream = new FileStream( newFirmwareFilename, FileMode.Append, FileAccess.Write, FileShare.Read ))
            {
                messageLogger.logMessage( "Расчет и запись контрольных сумм." );
                writeFinalCrc( outputStream, scriptHolder.CrcType, newScriptHolder.FirmwareBodyCrc );

                messageLogger.logMessage( "Добавление финальной подписи." );
                string starter = (newScriptHolder.Elements[0] is InformationalBlock firstComment) ? firstComment.getStarter() : null;
                if (starter == null)
                    starter = "# Alytona FmWare";
                outputStream.Write( Encoding.ASCII.GetBytes( starter ), 0, 16 );
            }
        }

        void writeFinalCrc (FileStream firmwareOutputStream, ScriptElementsHolder.CrcTypes crcType, uint bodyCrc)
        {
            uint headerCrc = Partition.computeCrc32( firmwareOutputStream.Name, 0, 16 * 1024 );

            if (crcType == ScriptElementsHolder.CrcTypes.Third)
                firmwareOutputStream.Write( BitConverter.GetBytes( bodyCrc ), 0, 4 );

            firmwareOutputStream.Write( new byte[] { 49, 50, 51, 52, 53, 54, 55, 56 }, 0, 8 );
            firmwareOutputStream.Write( BitConverter.GetBytes( headerCrc ), 0, 4 );

            if (crcType == ScriptElementsHolder.CrcTypes.First)
                firmwareOutputStream.Write( BitConverter.GetBytes( bodyCrc ), 0, 4 );

            firmwareOutputStream.Flush();

            if (crcType > ScriptElementsHolder.CrcTypes.First)
            {
                uint firmwareCrc = Partition.computeCrc32( firmwareOutputStream.Name, 0, firmwareOutputStream.Length );
                firmwareOutputStream.Write( BitConverter.GetBytes( firmwareCrc ), 0, 4 );
            }
        }
    }
}
