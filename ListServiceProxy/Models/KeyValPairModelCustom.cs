using System;
using System.Collections.Generic;

namespace ListServiceProxy.Models
{
    public class KeyValPairModelCustom
    {
        public string value { get; set; }
        public string label { get; set; }
    }

    public class KeyValPairIntModelCustom
    {
        public int value { get; set; }
        public string label { get; set; }
    }

    public class KeyValPairShortModelCustom
    {
        public short value { get; set; }
        public string label { get; set; }
    }

    public class KeyValPairGuidModelCustom
    {
        public Guid value { get; set; }
        public string label { get; set; }
    }

    public class KeyValPairGuidWithAdditionalModelCustom
    {
        public string filler1 { get; set; }
        public Guid mappingKey { get; set; }
        public Guid value { get; set; }
        public string label { get; set; }
    } 

    public class CommercialGateMetaDataCustom
    {
        public List<KeyValPairModelCustom> ACDList { get; set; }
        public List<KeyValPairModelCustom> AccountTeamLocation { get; set; }
        public List<KeyValPairModelCustom> DeploymentComplexity { get; set; }
        public List<KeyValPairModelCustom> DeploymentLocation { get; set; }
        public List<KeyValPairModelCustom> LevelList { get; set; }
        public List<KeyValPairModelCustom> NewDeployment { get; set; }
        public List<KeyValPairIntModelCustom> OptimizationMetricType { get; set; }
        public List<KeyValPairModelCustom> PilotPricing { get; set; }
        public List<KeyValPairIntModelCustom> PricingLogic { get; set; }
        public List<KeyValPairModelCustom> QueuePhase { get; set; }
        public List<KeyValPairIntModelCustom> UpliftType { get; set; }
        public List<KeyValPairModelCustom> YearsList { get; set; }
    }

    public class SummaryPipelineMetaDataCustom
    {
        public List<KeyValPairShortModelCustom> QueueStatusList { get; set; }
        public List<KeyValPairGuidModelCustom> Accounts { get; set; }
        public List<KeyValPairGuidModelCustom> Industries { get; set; }
        public List<KeyValPairGuidModelCustom> Countries { get; set; }
        public List<KeyValPairGuidModelCustom> People { get; set; }
        public List<KeyValPairGuidModelCustom> Roles { get; set; }
        public List<KeyValPairGuidModelCustom> AccountPhase { get; set; }
        public List<KeyValPairShortModelCustom> QueueType { get; set; }
        public List<KeyValPairModelCustom> CC1Status { get; set; }
        public List<KeyValPairModelCustom> CC2Status { get; set; }
    }

    public class AiroMetaDataCustom
    {
        public List<KeyValPairShortModelCustom> Airport { get; set; }
        public List<KeyValPairGuidWithAdditionalModelCustom> AvailableQueues { get; set; }
        public List<KeyValPairGuidModelCustom> AvayaPeople { get; set; }
        public List<KeyValPairShortModelCustom> CLevelMeetingStatus { get; set; }
        public List<KeyValPairShortModelCustom> LeadWith { get; set; }
        public List<KeyValPairShortModelCustom> Priority { get; set; }
        public List<KeyValPairShortModelCustom> Region { get; set; }
        public List<KeyValPairShortModelCustom> Segment { get; set; }
        public List<KeyValPairGuidModelCustom> Title { get; set; }
    }

}
