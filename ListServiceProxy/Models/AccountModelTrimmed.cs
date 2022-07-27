using System;
using System.Collections.Generic;

namespace ListServiceProxy.Models
{
    public class AccountModelTrimmed
    {
        public Dictionary<string, string> dic;

        public string AccountName { get; set; }
        public string AccountStatusStyle { get; set; }
        public string AccountCode { get; set; }
        public Guid AccountKey { get; set; }
        public string ImageFileExtension { get; set; }
        public Dictionary<string, string> AccountInfo { get; set; }
    }


}
