using System.Collections.Generic;

namespace WTFChannel.DataContracts
{
    public class Airing
    {
        public string ProgramId { get; set; }
        public string Title { get; set; }
        public string AiringTime { get; set; }
        public int Duration { get; set; }
        public string Color { get; set; }
        public string AiringType { get; set; }
        public bool CC { get; set; }
        public bool LetterBox { get; set; }
        public bool Stereo { get; set; }
        public bool HD { get; set; }
        public bool SAP { get; set; }
        public string TVRating { get; set; }
        public bool Dolby { get; set; }
        public bool DSS { get; set; }
        public string HDLevel { get; set; }
        public bool DVS { get; set; }
        public string Category { get; set; }
        public bool Sports { get; set; }
        public string SeriesId { get; set; }
        public string EpisodeTitle { get; set; }
        public string Subcategory { get; set; }
        public string MovieRating { get; set; }
    }

    public class GridChannel
    {
        public int ServiceId { get; set; }
        public int SourceId { get; set; }
        public int Order { get; set; }
        public string Channel { get; set; }
        public string CallLetters { get; set; }
        public string DisplayName { get; set; }
        public string SourceLongName { get; set; }
        public string Type { get; set; }
        public string SourceType { get; set; }
        public int ParentNetworkId { get; set; }
        public bool IconAvailable { get; set; }
        public bool IsChannelOverride { get; set; }
        public string SourceAttributes { get; set; }
        public List<object> ChannelSchedules { get; set; }
        public object SourceAttributeTypes { get; set; }
        public List<Airing> Airings { get; set; }
        public List<object> ChannelImages { get; set; }
    }

    public class GridScheduleResult
    {
        public string Locale { get; set; }
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public int Duration { get; set; }
        public string RequestId { get; set; }
        public string TimeStamp { get; set; }
        public string Status { get; set; }
        public List<object> Errors { get; set; }
        public string EndTimestamp { get; set; }
        public string Build { get; set; }
        public List<TimeZone> TimeZones { get; set; }
        public List<GridChannel> GridChannels { get; set; }
    }

    public class GridScheduleObject
    {
        public GridScheduleResult GridScheduleResult { get; set; }
    }
}