using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MStarGUI
{
    public interface IMessageLogger
    {
        void logMessage (string message);
    }

    public enum PackingType
    {
        None,
        Sparse,
        Lzo
    }

    public class Partition
    {
        [DllImport( "ntdll.dll" )]
        static extern uint RtlComputeCrc32 (uint seed, byte[] data, uint dataLength);

        public static uint computeCrc32 (string filename)
        {
            uint crc32 = 0;
            if (File.Exists( filename )) 
            {
                using (FileStream fileStream = new FileStream( filename, FileMode.Open, FileAccess.Read )) 
                {
                    crc32 = 0;                    
                    byte[] buffer = new byte[1000];
                    int read = fileStream.Read( buffer, 0, 1000 );
                    while (read != 0) 
                    {
                        crc32 = RtlComputeCrc32( crc32, buffer, (uint)read );
                        read = fileStream.Read( buffer, 0, 1000 );
                    }
                }
            }
            return crc32;
        }
        public static uint computeCrc32 (string filename, long offset, long size)
        {
            uint crc32 = 0;
            if (File.Exists( filename ))
            {
                using (FileStream fileStream = new FileStream( filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ))
                {
                    crc32 = computeCrc32( fileStream, offset, size );
                }
            }
            return crc32;
        }
        public static uint computeCrc32 (FileStream fileStream, long offset, long size)
        {
            uint crc32 = 0;
            fileStream.Seek( offset, SeekOrigin.Begin );
            long readSize = 0;
            crc32 = 0;
            byte[] buffer = new byte[1000];

            int read = fileStream.Read( buffer, 0, 1000 );
            while (read != 0 && read < size - readSize)
            {
                readSize += read;
                crc32 = RtlComputeCrc32( crc32, buffer, (uint)read );
                read = fileStream.Read( buffer, 0, 1000 );
            }
            if (read > 0)
            {
                crc32 = RtlComputeCrc32( crc32, buffer, (uint)(size < readSize + read ? (int)(size - readSize) : read) );
            }
            return crc32;
        }

        public static uint reverseBytes (uint source)
        {
            return BitConverter.ToUInt32( reversedBytes( source ), 0 );
        }
        public static byte[] reversedBytes (uint source)
        {
            byte[] bytes = BitConverter.GetBytes( source );
            Array.Reverse( bytes );
            return bytes;
        }

        public readonly List<WriteFileCommand> Chunks = new List<WriteFileCommand>();

        [ReadOnly( true ), DisplayName( "Имя" )]
        public string Name
        {
            get; private set;
        }

        public PackingType PackingType
        {
            get; set;
        } = PackingType.None;

        [ReadOnly( true ), DisplayName( "Разбиение на фрагменты" )]
        public bool Sparsed
        {
            get;
            private set;
        }

        [ReadOnly( true ), DisplayName( "Размер раздела" )]
        public long Size
        {
            get; set;
        }
        [ReadOnly( true ), DisplayName( "Размер данных" )]
        public long FileChunksTotalSize
        {
            get;
            private set;
        }

        public Partition (PartitionCreateCommand createCommand)
        {
            Name = createCommand.Name;
            Size = Convert.ToInt64( createCommand.Size, 16 );
        }
        public Partition (WriteFileCommand writeChunkCommand)
        {
            Name = writeChunkCommand.PartitionName;
            Size = writeChunkCommand.Size;
        }
        public void addChunk (WriteFileCommand writeChunkCommand)
        {
            if (writeChunkCommand is SparseWriteCommand)
                PackingType = PackingType.Sparse;
            else if (writeChunkCommand is UnlzoCommand)
                PackingType = PackingType.Lzo;

            FileChunksTotalSize += writeChunkCommand.Size;
            Chunks.Add( writeChunkCommand );
        }
        public string getSizeString ()
        {
            return Size.ToString();
        }
        public string getChunksSizeString ()
        {
            if (Chunks.Count == 0)
                return string.Empty;
            return FileChunksTotalSize.ToString();
        }
        public string getTypeString ()
        {
            return Chunks.Count > 0 ? Chunks[0].getTypeName() : "";
        }
        public string getChunksString ()
        {
            return Chunks.Count.ToString();
        }

        public bool unpack (string workDirectory, FileStream firmwareStream, IMessageLogger messageLogger)
        {
            string filename = Path.Combine( workDirectory, Name + ".img" );
            messageLogger.logMessage( "Выгрузка образа " + filename + "." );

            List<string> chunkNames = new List<string>();
            /*
                        readFileFromBinary( firmwareStream, 0, 16 * 1024, filename + ".header" );
                        readFileFromBinary( firmwareStream, 16 * 1024, firmwareStream.Length - 32 - 16 * 1024, filename + ".body" );
                        readFileFromBinary( firmwareStream, 0, firmwareStream.Length - 20, filename + ".all_minus_20" );
                        messageLogger.logMessage( $"header CRC 1: { reverseBytes( computeCrc32( firmwareStream.Name, 0, 16 * 1024 ) ):X8}" );
                        messageLogger.logMessage( $"header CRC 2: { reverseBytes( computeCrc32( filename + ".header" ) ):X8}" );
                        messageLogger.logMessage( $"body CRC 1: { reverseBytes( computeCrc32( firmwareStream.Name, 16 * 1024, firmwareStream.Length - 32 - 16 * 1024 ) ):X8}" );
                        messageLogger.logMessage( $"body CRC 2: { reverseBytes( computeCrc32( filename + ".body" ) ):X8}" );
                        messageLogger.logMessage( $"firmware without last 20 bytes CRC 1: { reverseBytes( computeCrc32( firmwareStream.Name, 0, firmwareStream.Length - 20 ) ):X8}" );
                        messageLogger.logMessage( $"firmware without last 20 bytes CRC 2: { reverseBytes( computeCrc32( filename + ".all_minus_20" ) ):X8}" );
            */

            int chunkIndex = 1;
            foreach (WriteFileCommand command in Chunks)
            {
                long offset = command.Offset;
                long size = command.Size;

                if (command is SparseWriteCommand writeSparseCommand)
                {
                    string sparseFilename = filename + $".{offset:X}";
                    readFileFromBinary( firmwareStream, offset, size, sparseFilename );
                    chunkNames.Add( sparseFilename );
                }
                else if (command is UnlzoCommand unlzoCommand) 
                {
                    string lzoFilename = filename + $".{offset:X}";
                    //string lzoFilename = filename + $".chunk.{chunkIndex}";
                    readFileFromBinary( firmwareStream, offset, size, lzoFilename );
                    chunkNames.Add( lzoFilename );
                }
                else 
                {
                    readFileFromBinary( firmwareStream, offset, size, filename );
                }
                //if (command is WritePiCommand writePiCommand || command is WriteBootCommand writeBootCommand || )

                chunkIndex++;
            }

            if (chunkNames.Count > 0)
            {
                if (PackingType == PackingType.Sparse)
                {
                    messageLogger.logMessage( "Распаковка образа " + filename + "." );
                    Compress.Sparse.Decompress( chunkNames.ToArray(), filename );
                    File.Delete( filename + ".sparse" );
                    foreach (string chunkFileName in chunkNames)
                        File.Delete( chunkFileName );
                }
                else if (PackingType == PackingType.Lzo)
                {
                    messageLogger.logMessage( "Распаковка образа " + filename + "." );
                    decompressLzoChunks( chunkNames, filename );
                }
            }
            return true;
        }
        void decompressLzoChunks (List<string> chunkNames, string filename)
        {
            using (FileStream outputStream = new FileStream( filename, FileMode.Create, FileAccess.Write )) 
            {
                foreach (string chunkName in chunkNames)
                {
                    decompressLzoChunk( chunkName );
                    writeFileToBinary( outputStream, chunkName + ".plain", align: false );
                    File.Delete( chunkName );
                    File.Delete( chunkName + ".plain" );
                }
            }
        }
        void decompressLzoChunk (string chunkName)
        {
            string decompressedFilename = chunkName + ".plain";
            if (File.Exists( decompressedFilename ))
                File.Delete( decompressedFilename );
            Compress.Lzo.Decompress( chunkName, decompressedFilename );
        }

        void readFileFromBinary (FileStream inputStream, long offset, long size, string filename)
        {
            inputStream.Seek( offset, SeekOrigin.Begin );
            using (FileStream outputStream = new FileStream( filename, FileMode.Create, FileAccess.Write ))
            {
                long readSize = 0;
                byte[] buffer = new byte[1000];
                int read = inputStream.Read( buffer, 0, 1000 );
                while (read != 0 && read < size - readSize)
                {
                    outputStream.Write( buffer, 0, read );
                    readSize += read;
                    read = inputStream.Read( buffer, 0, 1000 );
                }
                if (read > 0)
                    outputStream.Write( buffer, 0, size < readSize + read ? (int)(size - readSize) : read );

                outputStream.Flush();
            }
        }

        long writeFileToBinary (FileStream outputStream, string filename, bool align = true)
        {
            long size = 0;
            using (FileStream inputStream = new FileStream( filename, FileMode.Open, FileAccess.Read ))
            {
                byte[] buffer = new byte[1000];
                int read = inputStream.Read( buffer, 0, 1000 );
                while (read != 0)
                {
                    size += read;
                    outputStream.Write( buffer, 0, read );
                    read = inputStream.Read( buffer, 0, 1000 );
                }
            }
            outputStream.Flush();
            if (align)
                alignStream( outputStream );

            return size;
        }
        void alignStream (FileStream outputStream)
        {
            long offset = outputStream.Position & 0x0FFF;
            if (offset != 0)
            {
                long offsetRemainder = 0x1000 - offset;
                for (long i = 0; i < offsetRemainder; i++)
                {
                    outputStream.WriteByte( 0xFF );
                }
                outputStream.Flush();
            }
        }

        public List<WriteFileCommand> pack (string workDirectory, FileStream outputStream, IMessageLogger messageLogger)
        {
            long currentOffset = outputStream.Position;
            List<WriteFileCommand> chunks = new List<WriteFileCommand>();

            if (Chunks.Count > 0)
            {
                messageLogger.logMessage( $"Добавление образа {Name}." );
                string imgFilename = Path.Combine( workDirectory, Name + ".img" );

                WriteFileCommand command = Chunks[0];

                if (command is WritePiCommand writePiCommand)
                {
                    long size = writeFileToBinary( outputStream, imgFilename );
                    chunks.Add( new WritePiCommand( writePiCommand, currentOffset, size ) );
                }
                else if (command is WriteBootCommand writeBootCommand)
                {
                    long size = writeFileToBinary( outputStream, imgFilename );
                    chunks.Add( new WriteBootCommand( writeBootCommand, currentOffset, size ) );
                }
                else if (command is StoreSecureInfoCommand storeSecureInfoCommand)
                {
                    long size = writeFileToBinary( outputStream, imgFilename );
                    chunks.Add( new StoreSecureInfoCommand( storeSecureInfoCommand, currentOffset, size ) );
                }
                else if (command is StoreNuttxConfigCommand storeNuttxConfigCommand)
                {
                    long size = writeFileToBinary( outputStream, imgFilename );
                    chunks.Add( new StoreNuttxConfigCommand( storeNuttxConfigCommand, currentOffset, size ) );
                }
                else if (command is SparseWriteCommand writeSparseCommand)
                {
                    messageLogger.logMessage( "Упаковка sparse." );
//                    string simgFilename = Path.Combine( workDirectory, Name + ".simg" );
                    Compress.Sparse.Compress( imgFilename, 4096, 157286400 );

                    int index = 0;
                    string chunkFileName = imgFilename + ".chunk." + index++;
                    while (File.Exists( chunkFileName ))
                    {
                        messageLogger.logMessage( "Добавление части " + index + "." );
                        currentOffset = outputStream.Position;
                        long size = writeFileToBinary( outputStream, chunkFileName );
                        File.Delete( chunkFileName );
                        chunks.Add( new SparseWriteCommand( writeSparseCommand, currentOffset, size, firstChunk: index == 1 ) );
                        chunkFileName = imgFilename + ".chunk." + index++;
                    }
                }
                else if (command is UnlzoCommand unlzoCommand)
                {
                    const long chunkSize = 100 * 1024 * 1024;

                    messageLogger.logMessage( "Упаковка lzo." );

                    string wholeLzoFilename = imgFilename + ".lzo";
                    if (File.Exists( wholeLzoFilename ))
                        File.Delete( wholeLzoFilename );
                    Compress.Lzo.Compress( imgFilename, wholeLzoFilename );
                    long packedLength = 0;
                    using (FileStream inputStream = new FileStream( wholeLzoFilename, FileMode.Open, FileAccess.Read )) {
                        packedLength = inputStream.Length;
                    }

                    if (packedLength < chunkSize)
                    {
                        currentOffset = outputStream.Position;
                        long size = writeFileToBinary( outputStream, wholeLzoFilename );
                        //long size = (!File.Exists( imgFilename + ".chunk.1" )) ? 
                        //    writeFileToBinary( outputStream, wholeLzoFilename ) :
                        //    writeFileToBinary( outputStream, imgFilename + ".chunk.1" );
                        chunks.Add( new UnlzoCommand( unlzoCommand, currentOffset, size, firstChunk: true ) );
                    }
                    else 
                    {
                        using (FileStream inputStream = new FileStream( imgFilename, FileMode.Open, FileAccess.Read ))
                        {
                            long imgLength = inputStream.Length;

                            //if (!File.Exists( imgFilename + ".chunk.1" ))
                            {
                                messageLogger.logMessage( "Разбиение на чанки." );
                                int chunksCounter = 0;
                                for (long position = 0; position < imgLength; position += chunkSize)
                                {
                                    chunksCounter++;
                                    long remainderLength = imgLength - position;
                                    //readFileFromBinary( inputStream, position, remainderLength >= chunkSize ? chunkSize : remainderLength, imgFilename + $"a{(chunksCounter + 9):X}" );
                                    readFileFromBinary( inputStream, position, remainderLength >= chunkSize ? chunkSize : remainderLength, imgFilename + ".plain_chunk." + chunksCounter );
                                }
                                if (chunksCounter > 1)
                                    messageLogger.logMessage( "Упаковка " + chunksCounter + " частей." );
                                for (int chunkIndex = 1; chunkIndex <= chunksCounter; chunkIndex++)
                                {
                                    messageLogger.logMessage( "Упаковка части " + chunkIndex + "." );

                                    //string plainChunkName = imgFilename + $"a{(chunkIndex + 9):X}";
                                    string plainChunkName = imgFilename + ".plain_chunk." + chunkIndex;
                                    string chunkName = imgFilename + ".chunk." + chunkIndex;
                                    if (File.Exists( chunkName ))
                                        File.Delete( chunkName );
                                    Compress.Lzo.Compress( plainChunkName, chunkName );
                                    File.Delete( plainChunkName );
                                }
                            }
                            //for (int chunkIndex = 1; chunkIndex <= chunksCounter; chunkIndex++)

                            for (int chunkIndex = 1; File.Exists( imgFilename + ".chunk." + chunkIndex ); chunkIndex++)
                            {
                                messageLogger.logMessage( "Добавление части " + chunkIndex + "." );
                                currentOffset = outputStream.Position;
                                long size = writeFileToBinary( outputStream, imgFilename + ".chunk." + chunkIndex );
                                File.Delete( imgFilename + ".chunk." + chunkIndex );
                                chunks.Add( new UnlzoCommand( unlzoCommand, currentOffset, size, firstChunk: chunkIndex == 1 ) );
                            }
                        }
                    }
                    File.Delete( wholeLzoFilename );
                }
            }
            return chunks;
        }
    }
}
