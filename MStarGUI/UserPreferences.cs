using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

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
                    }
                }
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
