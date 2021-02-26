using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Drawing;

namespace MStarGUI
{
    [DataContract]
    class UserPreferences
    {
        [DataMember]
        public string WorkingDirectory
        {
            get; set;
        }

        [DataMember]
        public Size MainWindowSize
        {
            get; set;
        } = new Size( -1, -1 );

        [DataMember]
        public Point MainWindowPosition
        {
            get; set;
        } = new Point( -10000, -10000 );

        [DataMember]
        public int UnpackSplitterPosition
        {
            get; set;
        } = -1;

        [DataMember]
        public int PackSplitterPosition
        {
            get; set;
        } = -1;

        public bool Loaded
        {
            get; private set;
        }

        readonly string Filename;

        static readonly DataContractJsonSerializer PreferencesSerializer = new DataContractJsonSerializer( typeof( UserPreferences ) );

        public UserPreferences (string filename)
        {
            Filename = filename;
        }

        public bool load ()
        {
            try
            {
                if (File.Exists( Filename ))
                {
                    using (FileStream inputStream = new FileStream( Filename, FileMode.Open, FileAccess.Read ))
                    {
                        UserPreferences loadedConfiguration = (UserPreferences)PreferencesSerializer.ReadObject( inputStream );
                        if (loadedConfiguration == null)
                            return false;

                        WorkingDirectory = loadedConfiguration.WorkingDirectory;
                        MainWindowSize = loadedConfiguration.MainWindowSize;
                        MainWindowPosition = loadedConfiguration.MainWindowPosition;
                        UnpackSplitterPosition = loadedConfiguration.UnpackSplitterPosition;
                        PackSplitterPosition = loadedConfiguration.PackSplitterPosition;
                    }
                }
                Loaded = true;
                return true;
            }
            catch (Exception error)
            {
                return false;
            }
        }
        public bool save ()
        {
            try
            {
                using (FileStream outputStream = new FileStream( Filename, FileMode.Create, FileAccess.Write ))
                {
                    PreferencesSerializer.WriteObject( outputStream, this );
                }
                return true;
            }
            catch (Exception error)
            {
                return false;
            }
        }
    }
}
