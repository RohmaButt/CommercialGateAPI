using ListServiceProxy.Models;
using ListServiceProxy.ServiceReference;
using System.Collections.Generic;
using System.Linq;

namespace ListServiceProxy.ExtendedModels
{

    public static class AccountModelMapper
    {
        public static AccountModelTrimmed TrimFromAccountModel(this AccountModel source)
        {
            var account = new AccountModelTrimmed
            {
                AccountKey = source.AccountKey,
                AccountName = source.AccountName,
                AccountCode = source.AccountCode,
                ImageFileExtension = source.ImageFileExtension,
                AccountStatusStyle = source.AccountStatusStyle
            };
            account.AccountInfo = new Dictionary<string, string>();
            //Account Details
            if (source.AccountCardList.Any(x => x.CardNameId == "company-deal-type") && !string.IsNullOrEmpty(source.AccountDealType))
            {
                account.AccountInfo.Add("Deal Type", source.AccountDealType.Replace(" ", "-"));
            }

            if (source.AccountCardList.Any(x => x.CardNameId == "company-classification") && !string.IsNullOrEmpty(source.ClassificationValue))
            {
                account.AccountInfo.Add("Classification", source.ClassificationValue);
            }

            if (source.AccountCardList.Any(x => x.CardNameId == "company-currency") && !string.IsNullOrEmpty(source.AccountCurrency))
            {
                account.AccountInfo.Add("Currency", source.AccountCurrency);
            }

            if (source.AccountCardList.Any(x => x.CardNameId == "company-parent") && !string.IsNullOrEmpty(source.ParentClassificationValue))
            {
                account.AccountInfo.Add(source.ParentClassificationValue, source.ParentName);
            }

            if (source.AccountCardList.Any(x => x.CardNameId == "company-country") && !string.IsNullOrEmpty(source.AccountCountryName))
            {
                account.AccountInfo.Add("Country", source.AccountCountryName);
            }

            if (source.AccountCardList.Any(x => x.CardNameId == "company-status") && !string.IsNullOrEmpty(source.AccountStatus))
            {
                account.AccountInfo.Add("Phase", source.AccountStatus);
            }

            if (source.AccountCardList.Any(x => x.CardNameId == "company-account-leads") && source.AccountExecutive != null &&
                source.AccountExecutive.Any())
            {
                var leads = "";
                foreach (var lead in source.AccountExecutive)
                {
                    leads += lead.FullName + ", ";
                }
                if (!string.IsNullOrEmpty(leads))
                {
                    leads = leads.Trim();
                    leads = leads.Substring(0, leads.Length - 1);
                }

                account.AccountInfo.Add("Account Lead(s)", leads);
            }

            return account;
        }

        //public static AccountModelTrimmed TrimFromSearchAccountModel(this SearchAccountModel source)
        //{
        //    var account = new AccountModelTrimmed
        //    {
        //        AccountKey = source.AccountKey,
        //        AccountName = source.AccountName,
        //        AccountCode = source.AccountCode,
        //        ImageFileExtension = source.ImageFileExtension,
        //        AccountStatusStyle = source.AccountStatusStyle
        //    };
        //    return account;
        //}

        //public static AccountModelTrimmed TrimAccountsFromSearchResult(this SearchResults source)
        //{
        //    Guid guidKey;
        //    Guid.TryParse(source.ObjectKey, out guidKey);
        //    var person = new AccountModelTrimmed
        //    {
        //        AccountName = source.ObjectName.TrimText(35),
        //        AccountKey = guidKey,
        //        ImageFileExtension = source.ObjectImageUrl,
        //        AccountCode = source.ObjectDetail1,
        //        //PersonRoleValue = string.IsNullOrEmpty(source.ObjectDetail1) ? string.Empty : (source.ObjectDetail1 + " at " + source.ObjectDetail2)
        //    };
        //    return person;
        //}
    }
}
