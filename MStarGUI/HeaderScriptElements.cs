using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MStarGUI
{
    public interface IHeaderScriptElement
    {
        void writeToHeader (StreamWriter writer);
    }

    public class InformationalBlock : IHeaderScriptElement
    {
        readonly List<string> Strings = new List<string>();

        public void addString (string scriptLine)
        {
            Strings.Add( scriptLine );
        }

        public string getStarter ()
        {
            if (Strings.Count > 0 && Strings[0].Length >= 16)
                return Strings[0].Substring( 0, 16 );
            return null;
        }

        public void writeToHeader (StreamWriter writer)
        {
            foreach (string line in Strings)
            {
                writer.WriteLine( line );
            }
        }
    }
    class EraseCommand : IHeaderScriptElement
    {
        public readonly string PartitionName;

        public EraseCommand (string[] commandTokens)
        {
            if (commandTokens.Length != 3)
                throw new Exception( "Неверное количество параметров mmc erase.p (" + (commandTokens.Length - 2) + ", должен быть 1)." );

            PartitionName = commandTokens[2];
        }
        public void writeToHeader (StreamWriter writer)
        {
            writer.WriteLine( $"mmc erase.p " + PartitionName );
        }
    }

    class PartionsListSection : IHeaderScriptElement
    {
        public Dictionary<string, PartitionCreateCommand> PartitionCreateCommands
        {
            get; private set;
        } = new Dictionary<string, PartitionCreateCommand>();

        public string Slc
        {
            get; set;
        }
        public bool Rmgpt
        {
            get; set;
        }

        public void addPartitionCreateCommand (string name, string size)
        {
            PartitionCreateCommands.Add( name, new PartitionCreateCommand( name, size ) );
        }
        public bool isPartitionExists (string name)
        {
            return PartitionCreateCommands.ContainsKey( name );
        }
        public void writeToHeader (StreamWriter writer)
        {
            writer.WriteLine( Slc );
            if (Rmgpt)
                writer.WriteLine( "mmc rmgpt" );

            foreach (PartitionCreateCommand createCommand in PartitionCreateCommands.Values)
            {
                createCommand.writeTo( writer );
            }
        }
    }

    public class PartitionCreateCommand
    {
        public readonly string Name;

        public readonly string Size;

        public PartitionCreateCommand (string name, string size)
        {
            Name = name;
            Size = size;
        }
        public void writeTo (StreamWriter writer)
        {
            writer.WriteLine( $"mmc create {Name} {Size:8X}" );
        }
    }

    public class FilePartLoadCommand : IHeaderScriptElement
    {
        public string Address
        {
            get; private set;
        }
        public string ImageName
        {
            get; private set;
        }
        public long Offset
        {
            get; set;
        }
        public long Size
        {
            get; private set;
        }

        public FilePartLoadCommand (string[] commandTokens)
        {
            if (commandTokens.Length != 5)
                throw new Exception( "Неверное количество параметров filepartload (" + (commandTokens.Length - 1) + ", должно быть 4)." );

            Address = commandTokens[1];
            ImageName = commandTokens[2];

            try
            {
                Offset = Convert.ToInt64( commandTokens[3], 16 );
            }
            catch (Exception error)
            {
                throw new Exception( $"Ошибка разбора смещения сегмента в команде filepartload (третий параметр)", error );
            }
            try
            {
                Size = Convert.ToInt64( commandTokens[4], 16 );
            }
            catch (Exception error)
            {
                throw new Exception( $"Ошибка разбора размера сегмента в команде filepartload (четвёртый параметр)", error );
            }
        }
        public FilePartLoadCommand (FilePartLoadCommand sourceCommand, long offset, long size)
        {
            Address = sourceCommand.Address;
            ImageName = sourceCommand.ImageName;
            Offset = offset;
            Size = size;
        }

        public void writeToHeader (StreamWriter writer)
        {
            writer.WriteLine( $"filepartload {Address} {ImageName} 0x{Offset:x} 0x{Size:x}" );
        }
    }
}
