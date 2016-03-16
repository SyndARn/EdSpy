using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace ARnEdSpy.EDDScheme
{
    public class  EDDCommodity
    {
        private string fSchemaRef;
        private CommodityHeader fHeader;
        private CommodityMessage fMessage;

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
        public CommodityHeader Header
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
        public CommodityMessage Message
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

        public static  EDDCommodity FromJson(string data)
        {
            return JsonConvert.DeserializeObject<EDDCommodity>(data);
        }

    }

    public class CommodityHeader 
    {
        private string _uploaderID;
        private string _softwareName;
        private string _softwareVersion;
        private DateTime? _gatewayTimestamp;

        [JsonProperty("uploaderID", Required = Required.Always)]
        public string UploaderID
        {
            get { return _uploaderID; }
            set 
            {
                if (_uploaderID != value)
                {
                    _uploaderID = value; 
                }
            }
        }

        [JsonProperty("softwareName", Required = Required.Always)]
        public string SoftwareName
        {
            get { return _softwareName; }
            set 
            {
                if (_softwareName != value)
                {
                    _softwareName = value; 
                }
            }
        }

        [JsonProperty("softwareVersion", Required = Required.Always)]
        public string SoftwareVersion
        {
            get { return _softwareVersion; }
            set 
            {
                if (_softwareVersion != value)
                {
                    _softwareVersion = value; 
                }
            }
        }

        /// <summary>Timestamp upon receipt at the gateway. If present, this property will be overwritten by the gateway; submitters are not intended to populate this property.</summary>
        [JsonProperty("gatewayTimestamp", Required = Required.Default)]
        public DateTime? GatewayTimestamp
        {
            get { return _gatewayTimestamp; }
            set 
            {
                if (_gatewayTimestamp != value)
                {
                    _gatewayTimestamp = value; 
                }
            }
        }


        public string ToJson() 
        {
            return JsonConvert.SerializeObject(this);
        }

        public static CommodityHeader FromJson(string data)
        {
            return JsonConvert.DeserializeObject<CommodityHeader>(data);
        }

    }

    public class CommodityMessage 
    {
        private string _systemName;
        private string _stationName;
        private DateTime _timestamp;
        private List<CommoSupp> _commodities;

        [JsonProperty("systemName", Required = Required.Always)]
        public string SystemName
        {
            get { return _systemName; }
            set 
            {
                if (_systemName != value)
                {
                    _systemName = value; 
                }
            }
        }

        [JsonProperty("stationName", Required = Required.Always)]
        public string StationName
        {
            get { return _stationName; }
            set 
            {
                if (_stationName != value)
                {
                    _stationName = value; 
                }
            }
        }

        [JsonProperty("timestamp", Required = Required.Always)]
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set 
            {
                if (_timestamp != value)
                {
                    _timestamp = value; 
                }
            }
        }

        [JsonProperty("commodities", Required = Required.Always)]
        public List<CommoSupp> Commodities
        {
            get { return _commodities; }
            set 
            {
                if (_commodities != value)
                {
                    _commodities = value; 
                }
            }
        }


        public string ToJson() 
        {
            return JsonConvert.SerializeObject(this);
        }

        public static CommodityMessage FromJson(string data)
        {
            return JsonConvert.DeserializeObject<CommodityMessage>(data);
        }

    }

    public class CommoSupp 
    {
        private string _name;
        private long _buyPrice;
        private long _supply;
        private string _supplyLevel;
        private long _sellPrice;
        private long _demand;
        private string _demandLevel;

        [JsonProperty("name", Required = Required.Always)]
        public string Name
        {
            get { return _name; }
            set 
            {
                if (_name != value)
                {
                    _name = value; 
                }
            }
        }

        /// <summary>Price to buy from the market</summary>
        [JsonProperty("buyPrice", Required = Required.Always)]
        public long BuyPrice
        {
            get { return _buyPrice; }
            set 
            {
                if (_buyPrice != value)
                {
                    _buyPrice = value; 
                }
            }
        }

        [JsonProperty("supply", Required = Required.Always)]
        public long Supply
        {
            get { return _supply; }
            set 
            {
                if (_supply != value)
                {
                    _supply = value; 
                }
            }
        }

        [JsonProperty("supplyLevel", Required = Required.Default)]
        public string SupplyLevel
        {
            get { return _supplyLevel; }
            set 
            {
                if (_supplyLevel != value)
                {
                    _supplyLevel = value; 
                }
            }
        }

        /// <summary>Price to sell to the market</summary>
        [JsonProperty("sellPrice", Required = Required.Always)]
        public long SellPrice
        {
            get { return _sellPrice; }
            set 
            {
                if (_sellPrice != value)
                {
                    _sellPrice = value; 
                }
            }
        }

        [JsonProperty("demand", Required = Required.Always)]
        public long Demand
        {
            get { return _demand; }
            set 
            {
                if (_demand != value)
                {
                    _demand = value; 
                }
            }
        }

        [JsonProperty("demandLevel", Required = Required.Default)]
        public string DemandLevel
        {
            get { return _demandLevel; }
            set 
            {
                if (_demandLevel != value)
                {
                    _demandLevel = value; 
                }
            }
        }

        public string ToJson() 
        {
            return JsonConvert.SerializeObject(this);
        }

        public static CommoSupp FromJson(string data)
        {
            return JsonConvert.DeserializeObject<CommoSupp>(data);
        }

    }
}