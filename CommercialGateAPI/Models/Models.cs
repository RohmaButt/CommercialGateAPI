using ListServiceProxy.ServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommercialGateAPI.Models
{
    public class CommercialUpdateModel
    {
        [Required]
        public Guid pUniqueKey { get; set; }

        [Required]
        public string pProperty { get; set; }

        [Required]
        public string pPropertyValue { get; set; }
    }

    public class CommercialAddModel
    {
        [Required]
        public Guid pAccountkey { get; set; }

        [Required]
        public Guid pQueueKey { get; set; }
    }

    public class CommercialModel
    {
        //[Required]
        public CommercialText pModel { get; set; }

        //[Required]
        public Guid pAccountKey { get; set; }
    }

    public class ApprovalRejectionModel
    {
        [Required]
        public Guid pUniqueKey { get; set; }
    }

    public class UserModel
    {
        public string userKey { get; set; }
        public string userName { get; set; }
        public string imageUrl { get; set; }
    }

    public class PipelineSummaryUpdateModel
    {
        [Required]
        public Guid pUniqueKey { get; set; }

        [Required]
        public string pProperty { get; set; }

        [Required]
        public string pPropertyValue { get; set; }
    }


    //public class ViewerAndPaging<T>
    //{
    //    public string Message { get; set; }
    //    public string Code { get; set; }
    //    public List<T> ReviewResults { get; set; }
    //    public PagingClass PageInformation { get; set; }

    //    public ViewerAndPaging()
    //    {
    //        ReviewResults = new List<T>();
    //        PageInformation = new PagingClass();
    //        Code = "Initiate_9000";
    //        Message = "";// Resources.Resource.ResourceManager.GetString("Initiate_9000", CultureInfo.CurrentCulture);
    //    }
    //}

    //public class PagingClass
    //{
    //    public int TotalItems { get; set; }
    //    public int ItemsPerPage { get; set; }
    //    public int CurrentPage { get; set; }

    //    public PagingClass()
    //    {
    //        CurrentPage = 1;
    //        ItemsPerPage = 5;
    //        TotalItems = 5;
    //    }

    //    public int TotalPages
    //    {
    //        get
    //        {
    //            if (ItemsPerPage == 0)
    //                return 0;

    //            return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    //        }
    //    }
    //}

}