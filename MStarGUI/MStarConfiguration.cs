using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MStarGUI
{
    [DataContract]
    public class MStarConfiguration
    {
        [DataMember]
        public string FirmwareTitle
        {
            get; set;
        }

        readonly string Filename;

        static readonly DataContractJsonSerializer ConfigSerializer = new DataContractJsonSerializer( typeof( MStarConfiguration ) );

        public MStarConfiguration (string filename)
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
                        MStarConfiguration loadedConfiguration = (MStarConfiguration) ConfigSerializer.ReadObject( inputStream );
                        if (loadedConfiguration == null)
                            return false;

                        FirmwareTitle = loadedConfiguration.FirmwareTitle;
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
                    ConfigSerializer.WriteObject( outputStream, this );
                }
                return true;
            }
            catch (Exception error) {
                return false;
            }
        }
    }
}
