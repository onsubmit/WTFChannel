using System.Collections.Generic;

namespace WTFChannel.DataContracts
{
    public class TimeZone
    {
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string Offset { get; set; }
    }

    public class Service
    {
        public string ServiceClass { get; set; }
        public string ServiceId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public string MSO { get; set; }
        public string MSOID { get; set; }
        public string SystemName { get; set; }
        public List<TimeZone> TimeZones { get; set; }
    }

    public class Services
    {
        public List<Service> Service { get; set; }
    }

    public class ServicesResult
    {
        public string RequestId { get; set; }
        public string TimeStamp { get; set; }
        public string Status { get; set; }
        public List<object> Errors { get; set; }
        public string EndTimestamp { get; set; }
        public string Build { get; set; }
        public Services Services { get; set; }
    }

    public class ServicesResultObject
    {
        public ServicesResult ServicesResult { get; set; }
    }
}