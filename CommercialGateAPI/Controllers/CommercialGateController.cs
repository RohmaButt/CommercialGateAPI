using Afiniti.Framework.LoggingTracing;
using CommercialGateAPI.ActionFilters;
using CommercialGateAPI.Helpers;
using CommercialGateAPI.Models;
using CommercialGateAPI.Response;
using Common;
using ListServiceProxy;
using ListServiceProxy.Helpers;
using ListServiceProxy.Models;
using ListServiceProxy.ServiceReference;
using SecurityServiceProxy;
using SecurityServiceProxy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace CommercialGateAPI.Controllers
{
    /// <summary>
    /// Commercial Gate Controller
    /// </summary>
    [RoutePrefix("api/CommercialGate")]
    public class CommercialGateController : ApiController
    {
        #region CommercialGate
        /*
         Authentication and  Authorization is not Controller based but it is applied as per each ActionMethod requirement.
        */

        /// <summary>
        /// Ping to Commercial Gate controller
        /// </summary>
        /// <returns></returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<string>))]
        [HttpGet]
        [CustomAuthorization]
        [Route("Ping")]
        public string Ping()
        {
            ApplicationTrace.Log("CommercialGateAPI", "Ping after authentication", Status.Started);
            ApplicationTrace.Log("CommercialGateAPI", "Ping after authentication", Status.Completed);
            return "Yayy i am in. Authentication/Authorization passed for Commercial API:)";
        }

        #region Authentication

        /// <summary>
        /// Get User Admin Menu for Commercial Gate
        /// </summary>
        /// <remarks>
        /// Get Admin Menu details of User for Commercial Gate
        /// </remarks>
        /// <returns>
        /// Menu permissions list is returned
        /// </returns>
        [CustomAuthorization]
        [HttpGet]
        [Route("GetUserAdminMenu")]
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<List<MenuPermission>>))]
        public async Task<IHttpActionResult> GetUserAdminMenu()
        {
            try
            {
                ApplicationTrace.Log("CommercialGateAPI", "GetUserAdminMenu", Status.Started);
                APIHelper helper = new APIHelper();
                var SSOToken = helper.GetSSOTokenFromHeaders(Request);
                SecurityProxy obj = new SecurityProxy();
                var model = await obj.GetGenieAdminMenuForUser(SSOToken, "");
                ApplicationTrace.Log("CommercialGateAPI", "GetUserAdminMenu", Status.Completed);
                return new ResponseModel<List<MenuPermission>>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Get User Menu List for Commercial Gate
        /// </summary>
        /// <remarks>
        /// Get User Menu List of User for Commercial Gate
        /// </remarks>
        /// <returns>
        /// Menu permissions list is returned
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<List<MenuPermission>>))]
        [CustomAuthorization]
        [HttpGet]
        [Route("GetUserLeftMenu")]
        public async Task<IHttpActionResult> GetMenuList()
        {
            try
            {
                ApplicationTrace.Log("CommercialGateAPI", "GetUserLeftMenu", Status.Started);
                APIHelper helper = new APIHelper();
                var SSOToken = helper.GetSSOTokenFromHeaders(Request);
                SecurityProxy obj = new SecurityProxy();
                var model = await obj.GetGenieLeftMenuForUser(SSOToken, "");
                ApplicationTrace.Log("CommercialGateAPI", "GetUserLeftMenu", Status.Completed);
                return new ResponseModel<List<MenuPermission>>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Get logged user details for Commercial Gate
        /// </summary>
        /// <remarks>
        /// Get logged user information for Commercial Gate
        /// </remarks>
        /// <returns>
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<UserModel>))]
        [CustomAuthorization]
        [HttpGet]
        [Route("GetLoggedInUserInfo")]
        public IHttpActionResult GetLoggedInUserInfo()
        {
            try
            {
                ApplicationTrace.Log("CommercialGateAPI", "GetLoggedInUserInfo", Status.Started);
                APIHelper helper = new APIHelper();
                helper.GetUserModelInfo(Request);
                UserModel model = new UserModel();
                model = helper.GetUserModelInfo(Request);

                model.imageUrl = "/ContactProfile/Image?name=" + model.userKey;
                ApplicationTrace.Log("CommercialGateAPI", "GetLoggedInUserInfo", Status.Completed);

                return new ResponseModel<UserModel>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        #endregion Authentication

        /// <summary>
        /// Get Commercial Flat Model by Account
        /// </summary>
        /// <remarks>
        ///Get information of Commercial Flat Model based on Account Guid
        /// </remarks>
        /// <returns>
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<CommercialGateFlatModel>))]
        [CustomAuthorization]
        [Route("GetCommercialFlatModelByAcct")]
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<CommercialGateFlatModel>))]
        public async Task<IHttpActionResult> GetCommercialGateFlatModelByAccountKey(Guid pAccountKey)
        {
            try
            {
                if (pAccountKey == null || pAccountKey == Guid.Empty)
                {
                    return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong with request model.");
                }
                else
                {
                    ApplicationTrace.Log("CommercialGateAPI", "GetCommercialGateFlatModelByAccount", Status.Started);
                    var listProxy = new ListProxy(Request.Headers);
                    var model = await listProxy.GetSubmittedCommercialGateFlatModelByAccountKey(pAccountKey);
                    ApplicationTrace.Log("CommercialGateAPI", "GetCommercialGateFlatModelByAccount", Status.Completed);
                    return new ResponseModel<CommercialGateFlatModel>(model, Request);
                }
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Get Commercial Gate Meta Data
        /// </summary>
        /// <remarks>
        ///  Get MetaData for Commercial Gate 
        /// </remarks>
        /// <returns>
        /// MetaData in key pair values are returned 
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<CommercialGateMetaData>))]
        [Route("GetCommercialGateMetaDataAsync")]
        public async Task<IHttpActionResult> GetCommercialGateMetaDataAsync()
        {
            try
            {
                ApplicationTrace.Log("CommercialGateAPI", "GetCommercialGateMetaDataAsync", Status.Started);
                var listProxy = new ListProxy(Request.Headers);
                var model = await listProxy.GetCommercialGateMetaDataAsync();
                ApplicationTrace.Log("CommercialGateAPI", "GetCommercialGateMetaDataAsync", Status.Completed);
                return new ResponseModel<CommercialGateMetaData>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Get Commercial Gate Meta Data
        /// </summary>
        /// <remarks>
        ///  Get MetaData for Commercial Gate 
        /// </remarks>
        /// <returns>
        /// MetaData in key pair values are returned 
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<CommercialGateMetaDataCustom>))]
        [CustomAuthorization]
        [HttpGet]
        [Route("GetCommercialGateMetaData")]
        public async Task<IHttpActionResult> GetCommercialGateMetaData()
        {
            try
            {
                ApplicationTrace.Log("CommercialGateAPI", "GetCommercialGateMetaData", Status.Started);
                var listProxy = new ListProxy(Request.Headers);
                var model = await listProxy.GetCommercialGateMetaData1();
                ApplicationTrace.Log("CommercialGateAPI", "GetCommercialGateMetaData", Status.Completed);
                return new ResponseModel<CommercialGateMetaDataCustom>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Get Commercial Gate flat model
        /// </summary>
        /// <remarks>
        ///  Flat model with Viewer and Paging will be returned for Commercial Gate 
        /// </remarks>
        /// <returns>
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<ViewerAndPagingOfCommercialGateFlatModelTrimmedmGFQGkGm>))]
        [CustomAuthorization]
        [Route("GetCommercialGateFlatModels")]
        public async Task<IHttpActionResult> GetCommercialGateFlatModelsAsyn(/*SearchCriteria pCriteria, PagingClass pPagingClass*/)
        {
            try
            {
                ApplicationTrace.Log("CommercialGateAPI", "GetCommercialGateFlatModels", Status.Started);
                var listProxy = new ListProxy(Request.Headers);
                var model = await listProxy.GetCommercialGateFlatModelsAsync(/*pCriteria, pPagingClass*/);
                ApplicationTrace.Log("CommercialGateAPI", "GetCommercialGateFlatModels", Status.Completed);
                return new ResponseModel<ViewerAndPagingOfCommercialGateFlatModelTrimmedmGFQGkGm>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// validate if file upload is allowed
        /// </summary>
        /// <remarks>
        /// validate if file upload is allowed to logged in user
        /// </remarks>
        /// <returns>
        /// returns true/false
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<bool>))]
        [CustomAuthorization]
        [HttpGet]
        [Route("IsValidateFileUpload")]
        public Task<bool> IsValidateFileUpload()
        {
            try
            {
                ApplicationTrace.Log("CommercialGateAPI", "IsValidateFileUpload", Status.Started);
                ApplicationTrace.Log("CommercialGateAPI", "IsValidateFileUpload", Status.Completed);
                return Task.FromResult(true);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Search Internal Contacts for Commercial Gate
        /// </summary>
        /// <remarks>
        ///Search Internal Contacts for Commercial Gate and returns Key Value Guid Pair 
        /// </remarks>
        /// <returns>
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<List<KeyValPairGuid>>))]
        [CustomAuthorization]
        [HttpGet]
        [Route("SearchInternalContacts")]
        public async Task<IHttpActionResult> SearchInternalContacts(string searchVal)
        {
            try
            {
                ApplicationTrace.Log("CommercialGateAPI", "SearchInternalContacts", Status.Started);
                var listProxy = new ListProxy(Request.Headers);
                var model = await listProxy.SearchInternalContacts(searchVal);
                ApplicationTrace.Log("CommercialGateAPI", "SearchInternalContacts", Status.Completed);
                return new ResponseModel<List<KeyValPairGuid>>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Search external contacts for Commercial Gate
        /// </summary>
        /// <remarks>
        ///Search external contacts for Commercial Gate and returns Key Value Guid Pair 
        /// </remarks>
        /// <returns>
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<List<KeyValPairGuid>>))]
        [CustomAuthorization]
        [HttpGet]
        [Route("SearchExternalContacts")]
        public async Task<IHttpActionResult> SearchExternalContacts(string searchVal)
        {
            try
            {
                ApplicationTrace.Log("CommercialGateAPI", "SearchExternalContacts", Status.Started);
                var listProxy = new ListProxy(Request.Headers);
                var model = await listProxy.SearchExternalContacts(searchVal);
                ApplicationTrace.Log("CommercialGateAPI", "SearchExternalContacts", Status.Completed);
                return new ResponseModel<List<KeyValPairGuid>>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Search board contacts for Commercial Gate
        /// </summary>
        /// <remarks>
        ///Search board contacts for Commercial Gate and returns Key Value Guid Pair 
        /// </remarks>
        /// <returns>
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<List<KeyValPairGuid>>))]
        [CustomAuthorization]
        [HttpGet]
        [Route("SearchBoardContacts")]
        public async Task<IHttpActionResult> SearchBoardContacts(string searchVal)
        {
            try
            {
                ApplicationTrace.Log("CommercialGateAPI", "SearchBoardContacts", Status.Started);
                var listProxy = new ListProxy(Request.Headers);
                var model = await listProxy.SearchBoardContacts(searchVal);
                ApplicationTrace.Log("CommercialGateAPI", "SearchBoardContacts", Status.Completed);
                return new ResponseModel<List<KeyValPairGuid>>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Update Commercial Gate Flat model
        /// </summary>
        /// <remarks>
        /// Update Commercial Gate flat model
        /// </remarks>
        /// <returns>
        /// returns true/false
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<bool>))]
        [CustomAuthorization]
        [Route("UpdateCommercialGateFlat")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateCommercialGateFlat([FromBody]CommercialUpdateModel Requestmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationTrace.Log("CommercialGateAPI", "UpdateCommercialGateFlat", Status.Started);
                    var listProxy = new ListProxy(Request.Headers);
                    var model = await listProxy.RecommendChangesToCommercialGateFlat(Requestmodel.pUniqueKey, Requestmodel.pProperty, Requestmodel.pPropertyValue);
                    ApplicationTrace.Log("CommercialGateAPI", "UpdateCommercialGateFlat", Status.Completed);
                    return new ResponseModel<bool>(model, Request);
                }
                else
                {
                    return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong with request model.");
                }
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Add Commercial Gate Flat model
        /// </summary>
        /// <remarks>
        /// Add Commercial Gate
        /// </remarks>
        /// <returns>
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<CommercialGateModel>))]
        [CustomAuthorization]
        [Route("AddCommercialGate")]
        [HttpPost]
        public async Task<IHttpActionResult> AddCommercialGate([FromBody]CommercialAddModel Requestmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationTrace.Log("CommercialGateAPI", "AddCommercialGate", Status.Started);
                    var listProxy = new ListProxy(Request.Headers);
                    var model = await listProxy.AddCommercialGate(Requestmodel.pAccountkey, Requestmodel.pQueueKey);
                    ApplicationTrace.Log("CommercialGateAPI", "AddCommercialGate", Status.Completed);
                    return new ResponseModel<CommercialGateModel>(model, Request);
                }
                else
                {
                    return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong with request model.");
                }
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Add Commercial Gate Queue 
        /// </summary>
        /// <remarks>
        /// Add Commercial Gate Queue model
        /// </remarks>
        /// <returns>
        /// returns string
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<CommercialGateQueueModel>))]
        [CustomAuthorization]
        [Route("AddCommercialGateQueueModel")]
        [HttpPost]
        public async Task<IHttpActionResult> AddCommercialGateQueueModel([FromBody]CommercialAddModel Requestmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationTrace.Log("CommercialGateAPI", "AddCommercialGateQueueModel", Status.Started);
                    var listProxy = new ListProxy(Request.Headers);
                    var model = await listProxy.AddCommercialGateQueueModel(Requestmodel.pAccountkey, Requestmodel.pQueueKey);
                    ApplicationTrace.Log("CommercialGateAPI", "AddCommercialGateQueueModel", Status.Completed);
                    return new ResponseModel<CommercialGateQueueModel>(model, Request);
                }
                else
                {
                    return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong with request model.");
                }
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Add Commercial Gate discussion model
        /// </summary>
        /// <remarks>
        /// Add Commercial discussion  returns CommercialText
        /// </remarks>
        /// <returns>
        /// returns CommercialText
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<CommercialText>))]
        [CustomAuthorization]
        [Route("AddCommercialDiscussionToModel")]
        [HttpPost]
        public async Task<IHttpActionResult> AddCommercialDiscussionToModel([FromBody]CommercialModel Requestmodel)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                //StringCollection eMessages = new StringCollection
                //{
                //    DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:ffffff") + " CommercialGateAPI:AddCommercialDiscussionToModel-- Entered"
                //};
                ApplicationTrace.Log("CommercialGateAPI", "AddCommercialDiscussionToModel", Status.Started);
                var listProxy = new ListProxy(Request.Headers);
                var model = await listProxy.AddCommercialDiscussionToModel(Requestmodel.pAccountKey, Requestmodel.pModel);
                //eMessages.Add(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:ffffff") + " CommercialGateAPI:AddCommercialDiscussionToModel-- Exit");
                //LoggingUtility.LogTrace("CommercialGateAPI", eMessages);
                ApplicationTrace.Log("CommercialGateAPI", "AddCommercialDiscussionToModel", Status.Completed);
                return new ResponseModel<CommercialText>(model, Request);
                //}
                //else
                //{
                //    return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong with request model.");
                //}
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Get Commercial Gate discussion model by criteria
        /// </summary>
        /// <remarks>
        /// Get Commercial Gate discussion model by criteria
        /// </remarks>
        /// <returns>
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<ViewerAndPagingOfCommercialTextmGFQGkGm>))]
        [CustomAuthorization]
        [HttpGet]
        [Route("GetCommercialDiscussionByCriter")]
        public async Task<IHttpActionResult> GetCommercialDiscussionFromModelByCritera(Guid pAccountKey)
        {
            try
            {
                ApplicationTrace.Log("CommercialGateAPI", "GetCommercialDiscussionFromModelByCritera", Status.Started);
                var listProxy = new ListProxy(Request.Headers);
                var model = await listProxy.GetCommercialDiscussionFromModelByCritera(pAccountKey);
                ApplicationTrace.Log("CommercialGateAPI", "GetCommercialDiscussionFromModelByCritera", Status.Completed);
                return new ResponseModel<ViewerAndPagingOfCommercialTextmGFQGkGm>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Send document space creation for Commercial Gate 
        /// </summary>
        /// <remarks>
        /// Send document space creation for Commercial Gate 
        /// </remarks>
        /// <returns>
        /// returns true/false
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<bool>))]
        [CustomAuthorization]
        [Route("SendDocumentSpaceCreation")]
        [HttpPost]
        public async Task<IHttpActionResult> SendDocumentSpaceCreationReqeustToSystemsAdmin([FromBody]Guid pAccountKey)
        {
            try
            {
                if (pAccountKey == null || pAccountKey == Guid.Empty)
                {
                    return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong with request model.");
                }
                else
                {
                    ApplicationTrace.Log("CommercialGateAPI", "SendDocumentSpaceCreation", Status.Started);
                    var listProxy = new ListProxy(Request.Headers);
                    var model = await listProxy.SendDocumentSpaceCreationReqeustToSystemsAdmin(pAccountKey);
                    ApplicationTrace.Log("CommercialGateAPI", "SendDocumentSpaceCreation", Status.Completed);
                    return new ResponseModel<bool>(model, Request);
                }
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Approve account for Commercial Gate 
        /// </summary>
        /// <remarks>
        /// Approve account for Commercial Gate 
        /// </remarks>
        /// <returns>
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<bool>))]
        [CustomAuthorization]
        [Route("ApproveAccount")]
        [HttpPost]
        public async Task<IHttpActionResult> ApproveAccount([FromBody]ApprovalRejectionModel RequestModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationTrace.Log("CommercialGateAPI", "ApproveAccount", Status.Started);
                    var listProxy = new ListProxy(Request.Headers);
                    var model = await listProxy.ApproveAccount(RequestModel.pUniqueKey);
                    ApplicationTrace.Log("CommercialGateAPI", "ApproveAccount", Status.Completed);
                    return new ResponseModel<bool>(model, Request);
                }
                else
                {
                    return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong with request model.");
                }
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Approve Queue for Commercial Gate 
        /// </summary>
        /// <remarks>
        /// Approve Queue for Commercial Gate 
        /// </remarks>
        /// <returns>
        /// returns true/false
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<bool>))]
        [CustomAuthorization]
        [Route("ApproveQueue")]
        [HttpPost]
        public async Task<IHttpActionResult> ApproveQueue([FromBody]ApprovalRejectionModel RequestModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationTrace.Log("CommercialGateAPI", "ApproveQueue", Status.Started);
                    var listProxy = new ListProxy(Request.Headers);
                    var model = await listProxy.ApproveQueue(RequestModel.pUniqueKey);
                    ApplicationTrace.Log("CommercialGateAPI", "ApproveQueue", Status.Completed);
                    return new ResponseModel<bool>(model, Request);
                }
                else
                {
                    return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong with request model.");
                }
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Reject Account for Commercial Gate 
        /// </summary>
        /// <remarks>
        /// Reject Account for Commercial Gate 
        /// </remarks>
        /// <returns>
        /// returns true/false
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<bool>))]
        [CustomAuthorization]
        [Route("RejectAccount")]
        [HttpPost]
        public async Task<IHttpActionResult> RejectAccount([FromBody]ApprovalRejectionModel RequestModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationTrace.Log("CommercialGateAPI", "RejectAccount", Status.Started);
                    var listProxy = new ListProxy(Request.Headers);
                    var model = await listProxy.RejectAccount(RequestModel.pUniqueKey);
                    ApplicationTrace.Log("CommercialGateAPI", "RejectAccount", Status.Completed);
                    return new ResponseModel<bool>(model, Request);
                }
                else
                {
                    return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong with request model.");
                }
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Reject Queue for Commercial Gate 
        /// </summary>
        /// <remarks>
        /// Reject Queue for Commercial Gate 
        /// </remarks>
        /// <returns>
        /// returns true/false
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<bool>))]
        [CustomAuthorization]
        [Route("RejectQueue")]
        [HttpPost]
        public async Task<IHttpActionResult> RejectQueue([FromBody]ApprovalRejectionModel RequestModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationTrace.Log("CommercialGateAPI", "RejectQueue", Status.Started);
                    var listProxy = new ListProxy(Request.Headers);
                    var model = await listProxy.RejectQueue(RequestModel.pUniqueKey);
                    ApplicationTrace.Log("CommercialGateAPI", "RejectQueue", Status.Completed);

                    return new ResponseModel<bool>(model, Request);
                }
                else
                {
                    return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong with request model.");
                }
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        [CustomAuthorization]
        [Route("FetchToExport12")]
        [HttpGet]
        private async Task<IHttpActionResult> FetchToExport1()
        {
            try
            {
                var listProxy = new ListProxy(Request.Headers);
                var model = await listProxy.GetCommercialGateFlatModelsAsync();
                return new ResponseModel<ViewerAndPagingOfCommercialGateFlatModelTrimmedmGFQGkGm>(model, Request);

            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Export data for Commercial gate
        /// </summary>
        /// <remarks>
        /// Export data for Commercial gate
        /// </remarks>
        /// <returns>
        /// .xlsx file will be downloaded
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<System.Web.Mvc.FileContentResult>))]
        [CustomAuthorization]
        [Route("FetchToExport")]
        [HttpGet]
        public async Task<IHttpActionResult> FetchToExport(/*SearchCriteria pCriteria, PagingClass pPagingClass*/)
        {
            try
            {
                SearchCriteria pCriteria = new SearchCriteria();
                ListServiceProxy.ServiceReference.PagingClass pPagingClass = new ListServiceProxy.ServiceReference.PagingClass();

                //Guid Acctguid = Guid.Parse("0a655e9e-5983-4905-86be-d2a9546190d5");
                //pPagingClass = new PagingClass { CurrentPage = 1, ItemsPerPage = int.MaxValue };
                //pCriteria.SearchFiltersAndValues = new List<SearchFilter>();
                //pCriteria.SearchFiltersAndValues.Add(new SearchFilter { SearchParameter = "AccountKey", SearchValue = Acctguid, SearchExp = SearchExpression.And });

                string fileName = "CommercialGate_" + DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString() + ".xlsx";
                var listProxy = new ListProxy(Request.Headers);
                var ServerResponse = await listProxy.GetCommercialGateFlatModelsExport(pCriteria, pPagingClass);
                var requestModel = new FinancialExportAsExcelRequestModel();
                requestModel.TemplateKeys = new List<Guid>();
                requestModel.TemplateKeys.Add(Guid.Parse("38a0ebfc-45ef-4f4e-a842-5e0f0265993d"));
                byte[] Bresult;
                if (ServerResponse != null && (ServerResponse.CommercialQueues.Count > 0 || ServerResponse.Currencies.Count > 0))
                {
                    Bresult = new OpenXMLExcelHelper().CreateExcelEPPlusList(ServerResponse);
                }
                else
                {
                    Bresult = new OpenXMLExcelHelper().CreateAndExportEmptyExcel();
                }
                var stream = new MemoryStream(Bresult);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(stream.ToArray())
                };
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = ResponseMessage(result);
                return response;
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        [HttpGet]
        [Route("GetFile")]
        private HttpResponseMessage GetFile()
        {
            string fileName = "D:\abc.png";
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            string filePath = fileName;
            if (!File.Exists(filePath))
            {
                //Throw 404 (Not Found) exception if File not found.
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("File not found: {0} .", fileName);
                throw new HttpResponseException(response);
            }
            byte[] bytes = File.ReadAllBytes(filePath);
            response.Content = new ByteArrayContent(bytes);
            response.Content.Headers.ContentLength = bytes.LongLength;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));
            return response;
        }

        #endregion CommercialGate

    }
}
