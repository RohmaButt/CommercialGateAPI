using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.SharedModels
{
    /// <summary>
    /// Wrapped Parms are added to fetch flat model
    /// </summary>
    public class AiroRequestModel
    {
        public WrappedParms data { get; set; }
    }

    public class WrappedParms
    {
        /// <summary>
        /// length of data
        /// </summary>
        public int length { get; set; }
        /// <summary>
        /// current page for data
        /// </summary>
        public int currentPage { get; set; }
        /// <summary>
        /// Airo search parms to search data
        /// </summary>
        public SearchParms SearchParms { get; set; }
    }

    public class SearchParms
    {
        /// <summary>
        /// ACD to search in Airo flat model
        /// </summary>
        public List<SearchParams> ACD { get; set; }
        /// <summary>
        /// Account to search in Airo flat model
        /// </summary>
        public List<SearchParams> Account { get; set; }
        /// <summary>
        /// Account Lead to search in Airo flat model
        /// </summary>
        public List<SearchParams> AccountLead { get; set; }
        /// <summary>
        /// Account Phase to search in Airo flat model
        /// </summary>
        public List<SearchParams> AccountPhase { get; set; }
        /// <summary>
        /// Account Status to search in Airo flat model
        /// </summary>
        public List<SearchParams> AccountStatus { get; set; }
        /// <summary>
        /// Airport to search in Airo flat model
        /// </summary>
        public List<SearchParams> Airport { get; set; }
        /// <summary>
        /// Avaya AE to search in Airo flat model
        /// </summary>
        public List<SearchParams> AvayaAE { get; set; }
        /// <summary>
        /// Avaya Area Leaders to search in Airo flat model
        /// </summary>
        public List<SearchParams> AvayaAreaLeaders { get; set; }
        /// <summary>
        /// Avaya CSD to search in Airo flat model
        /// </summary>
        public List<SearchParams> AvayaCSD { get; set; }
        /// <summary>
        /// Avaya Notes to search in Airo flat model
        /// </summary>
        public List<SearchParams> AvayaNotes { get; set; }
        /// <summary>
        /// Avaya Partner to search in Airo flat model
        /// </summary>
        public List<SearchParams> AvayaPartner { get; set; }
        /// <summary>
        /// Avaya Regional Sales Leaders to search in Airo flat model
        /// </summary>
        public List<SearchParams> AvayaRegionalSalesLeaders { get; set; }
        /// <summary>
        /// Avaya Release to search in Airo flat model
        /// </summary>
        public List<SearchParams> AvayaRelease { get; set; }
        /// <summary>
        /// Avaya SE to search in Airo flat model
        /// </summary>
        public List<SearchParams> AvayaSE { get; set; }
        /// <summary>
        /// CEO Meeting to search in Airo flat model
        /// </summary>
        public List<SearchParams> CEOMeeting { get; set; }
        /// <summary>
        /// CTO Meeting to search in Airo flat model
        /// </summary>
        public List<SearchParams> CTOMeeting { get; set; }
        /// <summary>
        /// COO Meeting to search in Airo flat model
        /// </summary>
        public List<SearchParams> COOMeeting { get; set; }
        /// <summary>
        /// First Queue to search in Airo flat model
        /// </summary>
        public List<SearchParams> FirstQueue { get; set; }
        /// <summary>
        /// First Queue Status to search in Airo flat model
        /// </summary>
        public List<SearchParams> FirstQueueStatus { get; set; }
        /// <summary>
        /// JAPS to search in Airo flat model
        /// </summary>
        public List<SearchParams> JAPS { get; set; }
        /// <summary>
        /// Lead With to search in Airo flat model
        /// </summary>
        public List<SearchParams> LeadWith { get; set; }
        /// <summary>
        /// Priority to search in Airo flat model
        /// </summary>
        public List<SearchParams> Priority { get; set; }
        /// <summary>
        /// Region to search in Airo flat model
        /// </summary>
        public List<SearchParams> Region { get; set; }
        /// <summary>
        /// Sales Notes to search in Airo flat model
        /// </summary>
        public List<SearchParams> SalesNotes { get; set; }
        /// <summary>
        /// Seats to search in Airo flat model
        /// </summary>
        public List<SearchParams> Seats { get; set; }
        /// <summary>
        /// Segment to search in Airo flat model
        /// </summary>
        public List<SearchParams> Segment { get; set; }
        /// <summary>
        /// Vertical to search in Airo flat model
        /// </summary>
        public List<SearchParams> Vertical { get; set; }
        //    public List<SearchParams> Country { get; set; }
    }
    /// <summary>
    /// Request model to add contact
    /// </summary>
    public class AiroAddRequestModel
    {
        /// <summary>
        /// Title of contact
        /// </summary>
        //[Required]
        public Guid Title { get; set; }

        /// <summary>
        /// First Name of contact
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Middle Name of contact
        /// </summary>
        /// //[Required]
        public string MiddleName { get; set; }

        /// <summary>
        /// Last Name of contact
        /// </summary>
        /// [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Email of contact
        /// </summary>
        /// // [Required]
        public string Email { get; set; }

        /// <summary>
        /// Contact Number of contact
        /// </summary>
        /// // [Required]
        public string ContactNumber { get; set; }
    }

    /// <summary>
    /// Request model to update an account
    /// </summary>
    public class AiroUpdateRequestModel
    {
        /// <summary>
        /// Unique Key to update an account
        /// </summary>
        [Required]
        public Guid pUniqueKey { get; set; }

        /// <summary>
        /// Property Key to update an account
        /// </summary>
        /// [Required]
        public string pProperty { get; set; }

        /// <summary>
        /// Property Value to update an account
        /// </summary>
        /// [Required]
        public string pPropertyValue { get; set; }
    }

    /// <summary>
    /// AvayaAttribute is strongly typed object for properties in AiRo Flat model 
    /// </summary>
    public class AvayaAttribute
    {
        public string AttributeName { get; set; }
        public string AttributeDataType { get; set; }
        public string LookUpTable { get; set; }
        public bool IsEditable { get; set; }
        public string DisplayString { get; set; }
        public string ValueString { get; set; }
        public int MaxLength { get; set; }
    }

    public class AvayaAccountsFlatModel
    {
        public Guid UniqueKey { get; set; }
        public AvayaAttribute Account { get; set; }
        public AvayaAttribute Priority { get; set; }
        public AvayaAttribute LeadWith { get; set; }
        public AvayaAttribute AccountStatus { get; set; }
        public AvayaAttribute AccountPhase { get; set; }
        public AvayaAttribute FirstQueue { get; set; }
        public AvayaAttribute FirstQueueStatus { get; set; }
        public AvayaAttribute CEOMeeting { get; set; }
        public AvayaAttribute CTOMeeting { get; set; }
        public AvayaAttribute COOMeeting { get; set; }
        public AvayaAttribute AccountLead { get; set; }
        public AvayaAttribute Vertical { get; set; }
        public AvayaAttribute Segment { get; set; }
        public AvayaAttribute Seats { get; set; }
        public AvayaAttribute ACD { get; set; }
        public AvayaAttribute AvayaRelease { get; set; }
        public AvayaAttribute AvayaAreaLeaders { get; set; }
        public AvayaAttribute AvayaRegionalSalesLeaders { get; set; }
        public AvayaAttribute AvayaAE { get; set; }
        public AvayaAttribute AvayaSE { get; set; }
        public AvayaAttribute AvayaCSD { get; set; }
        public AvayaAttribute JAPS { get; set; }
        public AvayaAttribute AvayaPartner { get; set; }
        public AvayaAttribute Region { get; set; }
        public AvayaAttribute Airport { get; set; }
        public AvayaAttribute AvayaNotes { get; set; }
        public AvayaAttribute SalesNotes { get; set; }
    }

}
