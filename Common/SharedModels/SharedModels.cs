using System.Collections.Generic;

namespace Common.SharedModels
{
    //public class SharedModels
    //{
    //}

    public class PipelineSummaryRequestModel
    {
        public WrapperParms data { get; set; }
    }

    public class WrapperParms
    {
        public int length { get; set; }
        public int currentPage { get; set; }
        public PipelineSearchParms PipelineSearchParms { get; set; }
    }

    public class PipelineSearchParms
    {
        public List<SearchParams> Account { get; set; }
        public List<SearchParams> Industry { get; set; }
        public List<SearchParams> Country { get; set; }
        public List<SearchParams> People { get; set; }
        public List<SearchParams> Roles { get; set; }
        public List<SearchParams> Phase { get; set; }
        public List<SearchParams> QueueStatus { get; set; }
        public List<SearchParams> QueueType { get; set; }
        public List<SearchParams> CC1Status { get; set; }
        public List<SearchParams> CC2Status { get; set; }
        //public List<SearchParams> ClientBusinessCase { get; set; }
        //public List<SearchParams> AfinitiBusinessCase { get; set; }        
    }

    public struct SearchParams
    {
        public string data { get; set; }
        public string label { get; set; }//value
        public string value { get; set; }//key,         
    }

    public class Search
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }
}
