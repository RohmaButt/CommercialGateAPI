using System;
using System.Collections.Generic;

namespace ListServiceProxy.Models
{

    public class FinancialExportAsExcelRequestModel
    {
        //public FinancialTabularSearchModel FinancialTabularSearchModel { get; set; }
        public List<Guid> TemplateKeys { get; set; }
        public bool IsAggregated { get; set; }
    }
}
