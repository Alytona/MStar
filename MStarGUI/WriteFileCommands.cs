using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MStarGUI
{
    public abstract class WriteFileCommand : IHeaderScriptElement
    {
        readonly protected FilePartLoadCommand LoadCommand;

        public List<IHeaderScriptElement> Prolog
        {
            get; set;
        }
        public InformationalBlock Epilog
        {
            get; set;
        }

        public string Address
        {
            get;
            protected set;
        }
        public long Offset
        {
            get => LoadCommand.Offset;
            set => LoadCommand.Offset = value;
        }
        public long Size
        {
            get;
            protected set;
        }
        public string PartitionName
        {
            get;
            protected set;
        } = "---";

        protected WriteFileCommand (FilePartLoadCommand loadCommand)
        {
            if (loadCommand == null)
                throw new Exception( "Для команды записи файла сегмента должна быть указана команда его чтения." );
            LoadCommand = loadCommand;
        }
        protected WriteFileCommand (WriteFileCommand otherCommand, long offset, long size, bool copyProlog = true)
        {
            Address = otherCommand.Address;
            PartitionName = otherCommand.PartitionName;
            Size = size;
            LoadCommand = new FilePartLoadCommand( otherCommand.LoadCommand, offset, size );
            if (copyProlog) {
                Prolog = otherCommand.Prolog;
                for (int i = 0; i < Prolog.Count; i++)
                {
                    if (Prolog[i] is FilePartLoadCommand)
                    {
                        Prolog[i] = LoadCommand;
                    }
                }
            }
            Epilog = otherCommand.Epilog;
        }

        public abstract string getTypeName ();
        public abstract void writeToHeader (StreamWriter writer);
        
        protected void writeProlog (StreamWriter writer)
        {
            if (Prolog != null)
            {
                foreach (IHeaderScriptElement prologElement in Prolog)
                {
                    prologElement.writeToHeader( writer );
                }
            }
        }
        protected void writeEpilog (StreamWriter writer)
        {
            if (Epilog != null) {
                Epilog.writeToHeader(writer );
            }
        }
    }

    public class WriteBootCommand : WriteFileCommand
    {
        public enum BootType
        {
            RomBoot = 0,
            SecureBoot = 1
        }

        BootType BootTypeValue;

        string Unknown2;

        public WriteBootCommand (FilePartLoadCommand loadCommand, string[] commandTokens, int orderNumber) : base( loadCommand )
        {
            if (commandTokens.Length != 6)
                throw new Exception( "Неверное количество параметров mmc write.boot (" + (commandTokens.Length - 2) + ", должно быть 4)." );

            BootTypeValue = commandTokens[2] == "0" ? BootType.RomBoot : BootType.SecureBoot;
            Address = commandTokens[3];
            Unknown2 = commandTokens[4];
            try
            {
                Size = Convert.ToInt64( commandTokens[5], 16 );
            }
            catch (Exception error)
            {
                throw new Exception( $"Ошибка разбора размера сегмента в команде mmc write.boot (четвёртый параметр)", error );
            }

            if (BootTypeValue == BootType.SecureBoot)
                PartitionName = "sboot";
            else if (BootTypeValue == BootType.RomBoot)
                PartitionName = "rboot";
            else
                PartitionName = "BOOT" + orderNumber;
        }
        public WriteBootCommand (WriteBootCommand otherCommand, long offset, long size) : base( otherCommand, offset, size )
        {
            BootTypeValue = otherCommand.BootTypeValue;
            Unknown2 = otherCommand.Unknown2;
        }

        public override string getTypeName () => "boot";

        public override void writeToHeader (StreamWriter writer)
        {
            writeProlog( writer );
            writer.WriteLine( $"mmc write.boot {(int)BootTypeValue} {Address} {Unknown2} 0x{Size:x}" );
            writeEpilog( writer );
        }
    }

    public class WritePiCommand : WriteFileCommand
    {
        string Unknown;

        public WritePiCommand (FilePartLoadCommand loadCommand, string[] commandTokens) : base( loadCommand )
        {
            if (commandTokens.Length != 5 && commandTokens.Length != 6)
                throw new Exception( "Неверное количество параметров mmc write.p (" + (commandTokens.Length - 2) + ", должно быть 3 или 4)." );

            Address = commandTokens[2];
            PartitionName = commandTokens[3];
            try
            {
                Size = Convert.ToInt64( commandTokens[4], 16 );
            }
            catch (Exception error)
            {
                throw new Exception( $"Ошибка разбора размера сегмента в команде mmc write.p (третий параметр)", error );
            }

            if (commandTokens.Length == 6)
            {
                Unknown = commandTokens[5];
            }
        }
        public WritePiCommand (WritePiCommand otherCommand, long offset, long size) : base( otherCommand, offset, size )
        {
            Unknown = otherCommand.Unknown;
        }

        public override string getTypeName () => "pi";

        public override void writeToHeader (StreamWriter writer)
        {
            writeProlog( writer );
            writer.Write( $"mmc write.p {Address} {PartitionName} 0x{Size:X}" );
            if (!string.IsNullOrEmpty( Unknown ))
                writer.Write( $" {Unknown}" );
            writer.WriteLine();
            writeEpilog( writer );
        }
    }

    public class SparseWriteCommand : WriteFileCommand
    {
        readonly bool SizeFromLoadCommand;

        public SparseWriteCommand (FilePartLoadCommand loadCommand, string[] commandTokens) : base( loadCommand )
        {
            if (commandTokens.Length != 5)
                throw new Exception( "Неверное количество параметров sparse_write mmc (" + (commandTokens.Length - 2) + ", должно быть 3)." );

            Address = commandTokens[2];
            PartitionName = commandTokens[3];

            SizeFromLoadCommand = true;
            if (commandTokens[4] == "$(filesize)")
                Size = loadCommand.Size;
            else
            {
                try
                {
                    Size = Convert.ToInt64( commandTokens[4], 16 );
                }
                catch (Exception error)
                {
                    throw new Exception( $"Ошибка разбора размера сегмента в команде sparse_write mmc (третий параметр)", error );
                }
                SizeFromLoadCommand = false;
            }
        }
        public SparseWriteCommand (SparseWriteCommand otherCommand, long offset, long size, bool firstChunk) : base( otherCommand, offset, size, copyProlog: firstChunk )
        {
            if (!firstChunk)
                Prolog = new List<IHeaderScriptElement>() { LoadCommand };
            SizeFromLoadCommand = otherCommand.SizeFromLoadCommand;
        }

        public override string getTypeName () => "sparse";

        public override void writeToHeader (StreamWriter writer)
        {
            writeProlog( writer );
            writer.Write( $"sparse_write mmc {Address} {PartitionName}" );
            if (SizeFromLoadCommand)
                writer.Write( " $(filesize)" );
            else
                writer.Write( $" 0x{Size:x}" );
            writer.WriteLine();
        }
    }

    public class UnlzoCommand : WriteFileCommand
    {
        readonly bool FirstChunk;
        readonly string Unknown;

        public UnlzoCommand (FilePartLoadCommand loadCommand, string[] commandTokens) : base( loadCommand )
        {
            if (commandTokens.Length != 5 && commandTokens.Length != 6)
                throw new Exception( "Неверное количество параметров mmc unlzo (" + (commandTokens.Length - 2) + ", должно быть 3 или 4)." );

            FirstChunk = commandTokens[1] == "unlzo";

            Address = commandTokens[2];
            try
            {
                Size = Convert.ToInt64( commandTokens[3], 16 );
            }
            catch (Exception error)
            {
                throw new Exception( $"Ошибка разбора размера сегмента в команде mmc unlzo (второй параметр)", error );
            }
            PartitionName = commandTokens[4];
            Unknown = (commandTokens.Length == 6) ? commandTokens[5] : "";
        }
        public UnlzoCommand (UnlzoCommand otherCommand, long offset, long size, bool firstChunk) : base( otherCommand, offset, size, copyProlog: firstChunk )
        {
            FirstChunk = firstChunk;
            if (!firstChunk)
                Prolog = new List<IHeaderScriptElement>() { LoadCommand };
            Unknown = otherCommand.Unknown;
        }

        public override string getTypeName () => "lzo";

        public override void writeToHeader (StreamWriter writer)
        {
            writeProlog( writer );
            if (FirstChunk) 
                writer.Write( $"mmc unlzo " );
            else
                writer.Write( $"mmc unlzo.cont " );
            writer.Write( $"{Address} 0x{Size:X} {PartitionName}" );
            if (!string.IsNullOrEmpty( Unknown ))
                writer.Write( $" {Unknown}" );
            writer.WriteLine();
            writeEpilog( writer );
        }
    }
    public class StoreSecureInfoCommand : WriteFileCommand
    {
        public StoreSecureInfoCommand (FilePartLoadCommand loadCommand, string[] commandTokens) : base( loadCommand )
        {
            if (commandTokens.Length != 3)
                throw new Exception( "Неверное количество параметров store_secure_info (" + (commandTokens.Length - 1) + ", должно быть 2)." );

            PartitionName = commandTokens[1];
            Address = commandTokens[2];

            Offset = loadCommand.Offset;
            Size = loadCommand.Size;
        }
        public StoreSecureInfoCommand (StoreSecureInfoCommand otherCommand, long offset, long size) : base( otherCommand, offset, size )
        {
        }

        public override string getTypeName () => "secure info";

        public override void writeToHeader (StreamWriter writer)
        {
            writeProlog( writer );
            writer.WriteLine( $"store_secure_info {PartitionName} {Address}" );
            writeEpilog( writer );
        }
    }
    public class StoreNuttxConfigCommand : WriteFileCommand
    {
        public StoreNuttxConfigCommand (FilePartLoadCommand loadCommand, string[] commandTokens) : base( loadCommand )
        {
            if (commandTokens.Length != 3)
                throw new Exception( "Неверное количество параметров store_nuttx_config (" + (commandTokens.Length - 1) + ", должно быть 2)." );

            PartitionName = commandTokens[1];
            Address = commandTokens[2];

            Offset = loadCommand.Offset;
            Size = loadCommand.Size;
        }
        public StoreNuttxConfigCommand (StoreNuttxConfigCommand otherCommand, long offset, long size) : base( otherCommand, offset, size )
        {
        }

        public override string getTypeName () => "nuttx config";

        public override void writeToHeader (StreamWriter writer)
        {
            writeProlog( writer );
            writer.WriteLine( $"store_nuttx_config {PartitionName} {Address}" );
            writeEpilog( writer );
        }
    }
}