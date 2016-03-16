using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ARnEdSpy.EDDScheme
{

    public class EDDShipYard
    {
        private string fSchemaRef ;
        private ShipYardHeader fHeader;
        private ShipYardMessage fMessage;

        [JsonProperty("$schemaRef", Required = Required.Always)]
        public string SchemaRef
        {
            get { return fSchemaRef; }
            set 
            {
                if (fSchemaRef != value)
                {
                    fSchemaRef = value; 
                }
            }
        }

        [JsonProperty("header", Required = Required.Always)]
        public ShipYardHeader Header
        {
            get { return fHeader; }
            set 
            {
                if (fHeader != value)
                {
                    fHeader = value; 
                }
            }
        }

        [JsonProperty("message", Required = Required.Always)]
        public ShipYardMessage Message
        {
            get { return fMessage; }
            set 
            {
                if (fMessage != value)
                {
                    fMessage = value; 
                }
            }
        }

        public string ToJson() 
        {
            return JsonConvert.SerializeObject(this);
        }

        public static EDDShipYard FromJson(string data)
        {
            return JsonConvert.DeserializeObject<EDDShipYard>(data);
        }

    }

    public class ShipYardHeader
    {
        private string fUploaderID;
        private string fSoftwareName;
        private string fSoftwareVersion;
        private DateTime? fGatewayTimestamp;

        [JsonProperty("uploaderID", Required = Required.Always)]
        public string UploaderID
        {
            get { return fUploaderID; }
            set 
            {
                if (fUploaderID != value)
                {
                    fUploaderID = value; 
                }
            }
        }

        [JsonProperty("softwareName", Required = Required.Always)]
        public string SoftwareName
        {
            get { return fSoftwareName; }
            set 
            {
                if (fSoftwareName != value)
                {
                    fSoftwareName = value; 
                }
            }
        }

        [JsonProperty("softwareVersion", Required = Required.Always)]
        public string SoftwareVersion
        {
            get { return fSoftwareVersion; }
            set 
            {
                if (fSoftwareVersion != value)
                {
                    fSoftwareVersion = value; 
                }
            }
        }

        /// <summary>Timestamp upon receipt at the gateway. If present, this property will be overwritten by the gateway; submitters are not intended to populate this property.</summary>
        [JsonProperty("gatewayTimestamp", Required = Required.Default)]
        public DateTime? GatewayTimestamp
        {
            get { return fGatewayTimestamp; }
            set 
            {
                if (fGatewayTimestamp != value)
                {
                    fGatewayTimestamp = value; 
                }
            }
        }

        public string ToJson() 
        {
            return JsonConvert.SerializeObject(this);
        }

        public static ShipYardHeader FromJson(string data)
        {
            return JsonConvert.DeserializeObject<ShipYardHeader>(data);
        }

    }

    public class ShipYardMessage
    {
        private string fSystemName;
        private string fStationName;
        private DateTime fTimestamp;
        private List<string> fShipList;

        [JsonProperty("systemName", Required = Required.Always)]
        public string SystemName
        {
            get { return fSystemName; }
            set 
            {
                if (fSystemName != value)
                {
                    fSystemName = value; 
                }
            }
        }

        [JsonProperty("stationName", Required = Required.Always)]
        public string StationName
        {
            get { return fStationName; }
            set 
            {
                if (fStationName != value)
                {
                    fStationName = value; 
                }
            }
        }

        [JsonProperty("timestamp", Required = Required.Always)]
        public DateTime Timestamp
        {
            get { return fTimestamp; }
            set 
            {
                if (fTimestamp != value)
                {
                    fTimestamp = value; 
                }
            }
        }

        [JsonProperty("ships", Required = Required.Always)]
        public List<string> ShipList
        {
            get { return fShipList; }
            set 
            {
                if (fShipList != value)
                {
                    fShipList = value; 
                }
            }
        }

        public string ToJson() 
        {
            return JsonConvert.SerializeObject(this);
        }

        public static ShipYardMessage FromJson(string data)
        {
            return JsonConvert.DeserializeObject<ShipYardMessage>(data);
        }

    }
}

