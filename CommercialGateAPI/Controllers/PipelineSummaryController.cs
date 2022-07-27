using Afiniti.Framework.LoggingTracing;
using CommercialGateAPI.ActionFilters;
using CommercialGateAPI.Helpers;
using CommercialGateAPI.Models;
using CommercialGateAPI.Response;
using Common;
using Common.SharedModels;
using ListServiceProxy;
using ListServiceProxy.Helpers;
using ListServiceProxy.Models;
using ListServiceProxy.ServiceReference;
using Newtonsoft.Json;
using SecurityServiceProxy;
using SecurityServiceProxy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace CommercialGateAPI.Controllers
{
    /// <summary>
    /// Pipeline Summary Controller
    /// </summary>
    [RoutePrefix("api/PipelineSummary")]
    public class PipelineSummaryController : ApiController
    {
        /// <summary>
        /// Ping to Pipeline Summary controller
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Ping")]
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<string>))]
        public string Ping()
        {
            ApplicationTrace.Log("PipelineSummary", "Ping", Status.Started);
            ApplicationTrace.Log("PipelineSummary", "Ping", Status.Completed);

            return "Yayy i am in. Authentication/Authorization passed for PipelineSummary API:)";
        }

        #region Authentication

        /// <summary>
        /// Get User Admin Menu for Pipeline Summary
        /// </summary>
        /// <remarks>
        /// Get Admin Menu details of User for Pipeline Summary
        /// </remarks>
        /// <returns>
        /// Menu permissions list is returned
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<List<MenuPermission>>))]
        [CustomAuthorization]
        [HttpGet]
        [Route("GetUserAdminMenu")]
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<List<MenuPermission>>))]
        public async Task<IHttpActionResult> GetUserAdminMenu()
        {
            try
            {
                ApplicationTrace.Log("PipelineSummary", "GetUserAdminMenu", Status.Started);
                APIHelper helper = new APIHelper();
                var SSOToken = helper.GetSSOTokenFromHeaders(Request);
                SecurityProxy obj = new SecurityProxy();
                var model = await obj.GetGenieAdminMenuForUser(SSOToken, "");
                ApplicationTrace.Log("PipelineSummary", "GetUserAdminMenu", Status.Completed);

                return new ResponseModel<List<MenuPermission>>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Get User Menu List for Pipeline Summary
        /// </summary>
        /// <remarks>
        /// Get User Menu List of User for Pipeline Summary
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
                ApplicationTrace.Log("PipelineSummary", "GetUserLeftMenu", Status.Started);
                APIHelper helper = new APIHelper();
                var SSOToken = helper.GetSSOTokenFromHeaders(Request);
                SecurityProxy obj = new SecurityProxy();
                var model = await obj.GetGenieLeftMenuForUser(SSOToken, "");
                ApplicationTrace.Log("PipelineSummary", "GetUserLeftMenu", Status.Completed);
                return new ResponseModel<List<MenuPermission>>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Get logged user details for Pipeline Summary
        /// </summary>
        /// <remarks>
        /// Get logged user information for Pipeline Summary
        /// </remarks>
        /// <returns>
        /// returns user image and key
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
                ApplicationTrace.Log("PipelineSummary", "GetLoggedInUserInfo", Status.Started);
                UserModel model = new UserModel();
                APIHelper helper = new APIHelper();
                model = helper.GetUserModelInfo(Request);
                model.imageUrl = "/ContactProfile/Image?name=" + model.userKey;
                ApplicationTrace.Log("PipelineSummary", "GetLoggedInUserInfo", Status.Completed);

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
        /// Get meta data of for Pipeline Summary
        /// </summary>
        /// <remarks>
        /// Metdata information is retrieved for Pipeline Summary
        /// </remarks>
        /// <returns>
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<SummaryPipelineMetaDataCustom>))]
        [HttpGet]
        [CustomAuthorization]
        [Route("GetPipelineMetaData")]
        public async Task<IHttpActionResult> GetPipelineMetaData()
        {
            try
            {
                ApplicationTrace.Log("PiplineSummaryAPI", "GetPipelineMetaData", Status.Started);
                var listProxy = new ListProxy(Request.Headers);
                var model = await listProxy.GetPipelineMetaData();
                ApplicationTrace.Log("PiplineSummaryAPI", "GetPipelineMetaData", Status.Completed);
                return new ResponseModel<SummaryPipelineMetaDataCustom>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Update Pipeline Summary
        /// </summary>
        /// <remarks>
        /// Information for Pipeline Summary is updated
        /// </remarks>
        /// <returns>
        /// returns true/false
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<bool>))]
        [CustomAuthorization]
        [Route("UpdatePipelineSummary")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdatePipelineSummary([FromBody] PipelineSummaryUpdateModel model)
        {
            try
            {
                if (model.pUniqueKey == null || model.pUniqueKey == Guid.Empty || model.pProperty == null || model.pProperty == string.Empty || model.pPropertyValue == null || model.pPropertyValue == string.Empty)
                {
                    return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong with request model.");
                }
                else
                {
                    ApplicationTrace.Log("PiplineSummaryAPI", "UpdatePipelineSummary", Status.Started);
                    var listProxy = new ListProxy(Request.Headers);
                    var ResponseModel = await listProxy.UpdatePipelineSummary(model.pUniqueKey, model.pProperty, model.pPropertyValue);
                    ApplicationTrace.Log("PiplineSummaryAPI", "UpdatePipelineSummary", Status.Completed);
                    return new ResponseModel<bool>(ResponseModel, Request);
                }
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }


        /// <summary>
        /// Export data for Summary Pipeline
        /// </summary>
        /// <remarks>
        /// Export Summary Pipeline information 
        /// </remarks>
        /// <returns>
        /// .xlsx file will be downloaded
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<System.Web.Mvc.FileContentResult>))]
        [CustomAuthorization]
        [Route("ExportSummaryPipeline")]
        [HttpPost]
        public async Task<IHttpActionResult> FetchToExport_SummaryPipeline(PipelineSummaryRequestModel pRequest)
        {
            try
            {

                ApplicationTrace.Log("ExportSummaryPipeline", Status.Started);
                ApplicationTrace.Log("ExportSummaryPipeline", "Data is coming down the line -- " + JsonConvert.SerializeObject(pRequest), Status.Started);
                var search = pRequest.data.PipelineSearchParms;
                SearchCriteria pCriteria;
                var searchCriteria = "";

                pCriteria = CreateSearchCriteriaForPipelineSummary(pRequest.data.PipelineSearchParms);
                searchCriteria = JsonConvert.SerializeObject(pCriteria);
                ApplicationTrace.Log("ExportSummaryPipeline", "çriteria: " + JsonConvert.SerializeObject(pCriteria), Status.Started);
                PagingClass pPagingClass = new PagingClass() { CurrentPage = 1, ItemsPerPage = int.MaxValue };
                if (pRequest.data.length > 0)
                {
                    pPagingClass.CurrentPage = pRequest.data.currentPage;
                    pPagingClass.ItemsPerPage = pRequest.data.length;
                }
                string fileName = "SummaryPipeline_" + DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString() + ".xlsx";
                ApplicationTrace.Log("ExportSummaryPipeline", fileName, Status.Started);
                var listProxy = new ListProxy(Request.Headers);
                var ServerResponse = await listProxy.GetPiplineSummaryFlatModelExport(pCriteria, pPagingClass);
                byte[] Bresult;
                if (ServerResponse != null && ServerResponse.ReviewResults.Count > 0)
                {
                    Bresult = new OpenXMLExcelHelper().CreateExcelForPipelineSummary(ServerResponse);
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
                ApplicationTrace.Log("ExportSummaryPipeline", fileName, Status.Completed);

                return response;
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Get Pipline Summary Flat model
        /// </summary>
        /// <remarks>
        /// Flat model for Pipline Summary is retrieved
        /// </remarks>
        /// <returns>
        /// Flat model with Viewer and Paging will be returned
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<ViewerAndPagingOfPipelineSummaryFlatModelmGFQGkGm>))]
        [CustomAuthorization]
        [Route("GetPiplineSummaryFlat")]
        [HttpPost]
        public async Task<IHttpActionResult> GetPiplineSummaryFlatModel(PipelineSummaryRequestModel pRequest)
        {
            try
            {
                var search = pRequest.data.PipelineSearchParms;
                SearchCriteria pCriteria;
                var searchCriteria = "";
                ApplicationTrace.Log("GetPiplineSummaryFlatModel", "PiplineSummaryAPI:GetPiplineSummaryFlatModel", Status.Started);

                ApplicationTrace.Log("GetPiplineSummaryFlatModel", "PiplineSummaryAPI:GetPiplineSummaryFlatModel:Data is coming down the line -- " + JsonConvert.SerializeObject(pRequest), Status.Started);

                pCriteria = CreateSearchCriteriaForPipelineSummary(pRequest.data.PipelineSearchParms);
                searchCriteria = JsonConvert.SerializeObject(pCriteria);
                ApplicationTrace.Log("GetPiplineSummaryFlatModel", "criteria:" + JsonConvert.SerializeObject(pCriteria), Status.Started);

                ApplicationTrace.Log("GetPiplineSummaryFlatModel", "SearchCriteria:" + JsonConvert.SerializeObject(searchCriteria), Status.Started);

                PagingClass pPagingClass = new PagingClass() { CurrentPage = 1, ItemsPerPage = int.MaxValue };
                if (pRequest.data.length > 0)
                {
                    pPagingClass.CurrentPage = pRequest.data.currentPage;
                    pPagingClass.ItemsPerPage = pRequest.data.length;
                }
                var listProxy = new ListProxy(Request.Headers);
                var model = await listProxy.GetPiplineSummaryFlatModel(pCriteria, pPagingClass);
                ApplicationTrace.Log("GetPiplineSummaryFlatModel", "PiplineSummaryAPI:GetPiplineSummaryFlatModel", Status.Completed);

                return new ResponseModel<ViewerAndPagingOfPipelineSummaryFlatModelmGFQGkGm>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        #region General
        private SearchCriteria CreateSearchCriteriaForPipelineSummary(PipelineSearchParms pSearchCriteria)
        {
            SearchCriteria gF = new SearchCriteria();
            gF.SearchFiltersAndValues = new List<SearchFilter>();
            bool blnRootFound = false;
            string rootType = String.Empty;
            rootType = "Account";

            if (pSearchCriteria.GetType().GetProperty(rootType) != null && pSearchCriteria.GetType().GetProperty(rootType).GetValue(pSearchCriteria) != null)
            {
                var collection = (List<SearchParams>)pSearchCriteria.GetType().GetProperty(rootType).GetValue(pSearchCriteria);
                collection = collection.OrderBy(filter => filter.value).ToList<SearchParams>();

                if (collection != null && collection.Count > 0)
                {
                    gF.SearchFiltersAndValues.AddRange(CombineAllCriteriaWithORCondition(collection));
                    blnRootFound = true;
                }
            }
            foreach (PropertyInfo property in pSearchCriteria.GetType().GetProperties())
            {
                if (property.GetValue(pSearchCriteria, null) != null)
                {
                    try
                    {
                        var collection = (List<SearchParams>)property.GetValue(pSearchCriteria, null);
                        if (collection != null && collection.Count > 0)
                        {
                            if (blnRootFound == false)
                            {
                                gF.SearchFiltersAndValues.AddRange(CombineAllCriteriaWithORCondition(collection));
                                blnRootFound = true;
                                rootType = property.Name;
                            }
                            else
                            {
                                if (rootType != property.Name) //That Filter has been added as Root Filter so ignore it
                                {
                                    if (gF.SearchFiltersAndValues != null && gF.SearchFiltersAndValues.Count() > 0)
                                    {
                                        var crit = CombineAllCriteriaWithORCondition(collection);
                                        foreach (var item in gF.SearchFiltersAndValues)
                                        {
                                            if (item.CombiningClause == null)
                                            {
                                                item.CombiningClause = crit;
                                                item.SearchExp = SearchExpression.Or;
                                            }
                                            else
                                            {
                                                ApplyFilterToAllFiltersRecursive(item.CombiningClause, crit, property.Name.ToLower());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggingUtility.LogException(ex);
                    }
                }
            }
            return gF;
        }

        private List<SearchFilter> CombineAllCriteriaWithORCondition(List<SearchParams> pListOfSearchFilters)
        {
            List<SearchFilter> lstFiltersWithOr = new List<SearchFilter>();
            if (pListOfSearchFilters != null && pListOfSearchFilters.Count() > 0)
            {
                foreach (var filter in pListOfSearchFilters)
                {
                    string strParamName = GetSearchParamNameByType(filter.data);
                    if (!String.IsNullOrEmpty(strParamName))
                    {
                        object castedValue = null;
                        SearchComparison searchComp = SearchComparison.Equals;
                        try
                        {
                            if (strParamName == "AccountKey" || strParamName == "IndustryKey" || strParamName == "CountryKey" || strParamName == "PeopleKey" ||
                                strParamName == "RolesKey" || strParamName == "PhaseKey")
                            {
                                castedValue = Guid.Parse(filter.value);
                            }
                            else if (strParamName == "QueueStatus" || strParamName == "QueueType")
                            {
                                castedValue = short.Parse(filter.value);
                            }
                            else
                            {//CC1Status,CC2Status
                                castedValue = filter.value;
                            }
                        }
                        catch (Exception ex)
                        {
                            LoggingUtility.LogException(ex);
                        }
                        if (castedValue != null)
                        {
                            lstFiltersWithOr.Add(new SearchFilter { SearchParameter = strParamName, SearchValue = castedValue, SearchExp = SearchExpression.Or, Comparison = searchComp });
                        }
                    }
                }
            }
            return lstFiltersWithOr;
        }


        private List<SearchFilter> ApplyFilterToAllFiltersRecursive(List<SearchFilter> filter, List<SearchFilter> listOfSubFilters, string type)
        {
            foreach (var item in filter)
            {
                if (item.CombiningClause == null)
                {
                    item.CombiningClause = listOfSubFilters;
                    item.SearchExp = SearchExpression.Or;
                    continue;
                }
                else if (item.CombiningClause.FirstOrDefault().SearchParameter == GetSearchParamNameByType(type))
                {
                    return filter;
                }
                else
                {
                    return ApplyFilterToAllFiltersRecursive(item.CombiningClause, listOfSubFilters, type);
                }
            }
            return filter;
        }

        private string GetSearchParamNameByType(string pType)
        {
            switch (pType.ToLower())
            {
                case "account":
                    return "AccountKey";
                case "industry":
                    return "IndustryKey";
                case "country":
                    return "CountryKey";
                case "people":
                    return "PeopleKey";
                case "role":
                    return "RolesKey";
                case "roles":
                    return "RolesKey";
                case "phase":
                    return "PhaseKey";
                case "queuestatus":
                    return "QueueStatus";
                case "queuetype":
                    return "QueueType";
                case "cc1status":
                    return "CC1Status";
                case "cc2status":
                    return "CC2Status";
                //case "clientbusinesscase":
                //    return "ClientBusinessCase";
                //case "afinitibusinesscase":
                //    return "AfinitiBusinessCase";
                default:
                    return "";
            }
        }


        #endregion General
    }
}
