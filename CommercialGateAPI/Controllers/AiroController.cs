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
using static ListServiceProxy.Helpers.OpenXMLExcelHelper;
using AvayaAccountsFlatModel = ListServiceProxy.ServiceReference.AvayaAccountsFlatModel;
using AvayaAttribute = ListServiceProxy.ServiceReference.AvayaAttribute;
//using ListServiceProxy.Helpers.OpenXMLExcelHelper;


namespace CommercialGateAPI.Controllers
{
    /// <summary>
    /// AiRo Sales Controller
    /// </summary>
    [RoutePrefix("api/AiRo")]
    public class AiroController : ApiController
    {
        /// <summary>
        /// Ping to AiRo Sales controller
        /// </summary>
        /// <returns></returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<string>))]
        [HttpGet]
        [Route("Ping")]
        public string Ping()
        {
            ApplicationTrace.Log("Airo", "Ping", Status.Started);
            ApplicationTrace.Log("Airo", "Ping", Status.Completed);
            return "Yayy i am in for Airo API:)";
        }

        #region Authentication

        /// <summary>
        /// Get User Admin Menu for AiRo Sales
        /// </summary>
        /// <remarks>
        /// Get Admin Menu details of User for AiRo Sales
        /// </remarks>
        /// <returns>
        /// Menu permissions list is returned for logged in user
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<List<MenuPermission>>))]
        [CustomAuthorization]
        [HttpGet]
        [Route("GetUserAdminMenu")]
        public async Task<IHttpActionResult> GetUserAdminMenu()
        {
            try
            {
                ApplicationTrace.Log("AiroAPI", "GetUserAdminMenu", Status.Started);
                APIHelper helper = new APIHelper();
                var SSOToken = helper.GetSSOTokenFromHeaders(Request);
                SecurityProxy obj = new SecurityProxy();
                var model = await obj.GetGenieAdminMenuForUser(SSOToken, "");
                ApplicationTrace.Log("AiroAPI", "GetUserAdminMenu", Status.Completed);

                return new ResponseModel<List<MenuPermission>>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Get User Menu List for AiRo Sales
        /// </summary>
        /// <remarks>
        /// Get User Menu List of User for AiRo Sales
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
                ApplicationTrace.Log("AiroAPI", "GetUserLeftMenu", Status.Started);
                APIHelper helper = new APIHelper();
                var SSOToken = helper.GetSSOTokenFromHeaders(Request);
                SecurityProxy obj = new SecurityProxy();
                var model = await obj.GetGenieLeftMenuForUser(SSOToken, "");
                ApplicationTrace.Log("AiroAPI", "GetUserLeftMenu", Status.Completed);
                return new ResponseModel<List<MenuPermission>>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Get logged user details for AiRo Sales
        /// </summary>
        /// <remarks>
        /// Get logged user information for AiRo Sales
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
                ApplicationTrace.Log("AiroAPI", "GetLoggedInUserInfo", Status.Started);
                APIHelper helper = new APIHelper();
                UserModel model = new UserModel();
                model = helper.GetUserModelInfo(Request);
                model.imageUrl = "/ContactProfile/Image?name=" + model.userKey;
                ApplicationTrace.Log("AiroAPI ", "GetLoggedInUserInfo", Status.Completed);

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
        /// Add Contact for AiRo Sales
        /// </summary>
        /// <remarks>
        /// Contact information is added for AiRo Sales
        /// </remarks>
        /// <returns>
        /// returns true/false
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<AvayaContactModel>))]
        [CustomAuthorization]
        [Route("AddContact")]
        [HttpPost]
        public async Task<IHttpActionResult> AddAiroContact([FromBody] AiroAddRequestModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName))
                {
                    return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong with request model.");
                }
                else
                {
                    ApplicationTrace.Log("AiroAPI", "AddAiroContact", Status.Started);
                    var listProxy = new ListProxy(Request.Headers);
                    AvayaContactModel avayaContact = new AvayaContactModel
                    {
                        ContactNumber = model.ContactNumber,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        MiddleName = model.MiddleName,
                        Title = model.Title
                    };

                    var ResponseModel = await listProxy.AddAiroContact(avayaContact);
                    ApplicationTrace.Log("AiroAPI", "AddAiroContact", Status.Completed);
                    return new ResponseModel<AvayaContactModel>(ResponseModel, Request);
                }
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Update Account for AiRo Sales
        /// </summary>
        /// <remarks>
        /// Account information is updated for AiRo Sales and returns true/false
        /// </remarks>
        /// <returns>
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<bool>))]
        [CustomAuthorization]
        [Route("UpdateAccount")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateAiroAccount([FromBody] AiroUpdateRequestModel model)
        {
            try
            {
                //pPropertyValue can be null/'' as  per discussion with bilal/Faisal 
                if (model.pUniqueKey == null || model.pUniqueKey == Guid.Empty || string.IsNullOrEmpty(model.pProperty))// || string.IsNullOrEmpty(model.pPropertyValue)
                {
                    return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong with request model.");
                }
                else
                {
                    ApplicationTrace.Log("AiroAPI", "UpdateAiroAccount", Status.Started);
                    var listProxy = new ListProxy(Request.Headers);
                    var ResponseModel = await listProxy.UpdateAiroAccount(model);
                    ApplicationTrace.Log("AiroAPI", "UpdateAiroAccount", Status.Completed);
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
        /// Get MetaData for AiRo Sales
        /// </summary>
        /// <remarks>
        /// MetaData information is received for AiRo Sales
        /// </remarks>
        /// <returns>
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<AiroMetaDataCustom>))]
        [HttpGet]
        [CustomAuthorization]
        [Route("GetMetaData")]
        public async Task<IHttpActionResult> GetAiroMetaData()
        {
            try
            {
                ApplicationTrace.Log("AiroAPI", "GetAiroMetaData", Status.Started);
                var listProxy = new ListProxy(Request.Headers);
                var model = await listProxy.GetAiroMetaData();
                ApplicationTrace.Log("AiroAPI", "GetAiroMetaData", Status.Completed);
                return new ResponseModel<AiroMetaDataCustom>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Export data for AiRo Sales
        /// </summary>
        /// <remarks>
        /// Export AiRo Sales information 
        /// </remarks>
        /// <returns>
        /// .xlsx file will be downloaded
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<System.Web.Mvc.FileContentResult>))]
        [CustomAuthorization]
        [Route("Export")]
        [HttpPost]
        public async Task<IHttpActionResult> FetchToExport(AiroRequestModel pRequest)
        {
            try
            {
                ApplicationTrace.Log("ExportAiro", Status.Started);
                ApplicationTrace.Log("ExportAiro", "Data is coming down the line -- " + JsonConvert.SerializeObject(pRequest), Status.Started);
                var search = pRequest.data.SearchParms;
                SearchCriteria pCriteria;
                var searchCriteria = "";

                pCriteria = CreateSearchCriteriaForAiro(pRequest.data.SearchParms);
                searchCriteria = JsonConvert.SerializeObject(pCriteria);
                ApplicationTrace.Log("ExportAiro", "çriteria: " + JsonConvert.SerializeObject(pCriteria), Status.Started);
                PagingClass pPagingClass = new PagingClass() { CurrentPage = 1, ItemsPerPage = int.MaxValue };
                if (pRequest.data.length > 0)
                {
                    pPagingClass.CurrentPage = pRequest.data.currentPage;
                    pPagingClass.ItemsPerPage = pRequest.data.length;
                }
                string fileName = "AiRoSales_" + DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString() + ".xlsx";
                ApplicationTrace.Log("ExportAiro", fileName, Status.Started);
                var listProxy = new ListProxy(Request.Headers);
                var ServerResponse = await listProxy.GetAiroFlatModelExport(pCriteria, pPagingClass);
                byte[] Bresult;
                if (ServerResponse != null && ServerResponse.ReviewResults.Count > 0)
                {
                    Bresult = new OpenXMLExcelHelper().CreateExcelForAiRo(ServerResponse);
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
                ApplicationTrace.Log("ExportAiro", fileName, Status.Completed);

                return response;
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        /// Get Flat Model for AiRo Sales
        /// </summary>
        /// <remarks>
        /// Flat Model of AiRo Sales with Viewer and Paging will be retrieved
        /// </remarks>
        /// <returns>
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<ViewerAndPagingOfAvayaAccountsFlatModelmGFQGkGm>))]
        [Route("GetFlatModel")]
        [CustomAuthorization]
        [HttpPost]
        public async Task<IHttpActionResult> GetAiroFlatModel(AiroRequestModel pRequest)
        {
            try
            {
                var search = pRequest.data.SearchParms;
                SearchCriteria pCriteria;
                var searchCriteria = "";
                ApplicationTrace.Log("AiroAPI", "GetAiroFlatModel", Status.Started);
                ApplicationTrace.Log("AiroAPI", "GetAiroFlatModel:Data is coming down the line -- " + JsonConvert.SerializeObject(pRequest), Status.Started);

                pCriteria = CreateSearchCriteriaForAiro(pRequest.data.SearchParms);
                searchCriteria = JsonConvert.SerializeObject(pCriteria);
                ApplicationTrace.Log("AiroAPI", "criteria:" + JsonConvert.SerializeObject(pCriteria), Status.Started);
                ApplicationTrace.Log("AiroAPI", "SearchCriteria:" + JsonConvert.SerializeObject(searchCriteria), Status.Started);

                PagingClass pPagingClass = new PagingClass() { CurrentPage = 1, ItemsPerPage = int.MaxValue };
                if (pRequest.data.length > 0)
                {
                    pPagingClass.CurrentPage = pRequest.data.currentPage;
                    pPagingClass.ItemsPerPage = pRequest.data.length;
                }
                var listProxy = new ListProxy(Request.Headers);
                var model = await listProxy.GetAiroFlatModel(pCriteria, pPagingClass);
                ApplicationTrace.Log("AiroAPI", "GetAiroFlatModel", Status.Completed);
                return new ResponseModel<ViewerAndPagingOfAvayaAccountsFlatModelmGFQGkGm>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /// <summary>
        ///Search Contacts for AiRo Sales
        /// </summary>
        /// <remarks>
        ///Get search contacts for AiRo Sales and return Key Value Guid Pair 
        /// </remarks>
        /// <returns>
        /// </returns>
        ///<response code="200"></response>
        [ResponseType(typeof(IEnumerable<List<KeyValPairGuidModelCustom>>))]
        [CustomAuthorization]
        [HttpGet]
        [Route("GetSearchContacts")]
        public async Task<IHttpActionResult> GetSearchContacts(string searchVal)
        {
            try
            {
                ApplicationTrace.Log("AiroAPI", "GetSearchContacts", Status.Started);
                var listProxy = new ListProxy(Request.Headers);
                var model = await listProxy.GetAiRoSearchContacts(searchVal);
                ApplicationTrace.Log("AiroAPI", "GetSearchContacts", Status.Completed);
                return new ResponseModel<List<KeyValPairGuidModelCustom>>(model, Request);
            }
            catch (Exception exc)
            {
                LoggingUtility.LogException(exc);
                return new ResponseModel<string>(Request, HttpStatusCode.InternalServerError, "Something went wrong:" + exc);
            }
        }

        /*
        public ViewerAndPagingOfAvayaAccountsFlatModelmGFQGkGm CreateDummyFlatModel()
        {
            PagingClass pPagingClass = new PagingClass() { CurrentPage = 1, ItemsPerPage = int.MaxValue };
            List<AvayaAccountsFlatModel> FlatmodelList = new List<AvayaAccountsFlatModel>();

            #region 1stAiroAcct

            AvayaAccountsFlatModel Flatmodel = new AvayaAccountsFlatModel();
            //  Flatmodel.AccountKey = Guid.NewGuid();
            //    Flatmodel.UniqueKey = Guid.NewGuid();

            AvayaAttribute avayaAcct_Attribute = new AvayaAttribute
            {
                AttributeName = "Account",
                AttributeDataType = "Text",
                DisplayString = "Verizon Wireline",
                IsEditable = false,
                MaxLength = 100,
                ValueString = "Verizon Wireline",
                LookUpTable = "Accounts"
            };
            Flatmodel.Account = avayaAcct_Attribute;

            AvayaAttribute avayaAcctLead_Attribute = new AvayaAttribute
            {
                AttributeName = "AccountLead",
                AttributeDataType = "Text",
                DisplayString = "Bob Dunn",
                IsEditable = false,
                MaxLength = 100,
                ValueString = "Bob Dunn",
                LookUpTable = ""
            };
            Flatmodel.AccountLead = avayaAcctLead_Attribute;

            AvayaAttribute avayaAcctPhase_Attribute = new AvayaAttribute
            {
                AttributeName = "AccountPhase",
                AttributeDataType = "Text",
                DisplayString = "",
                IsEditable = false,
                MaxLength = 100,
                ValueString = "",
                LookUpTable = "AccountPhase"
            };
            Flatmodel.AccountPhase = avayaAcctPhase_Attribute;

            AvayaAttribute avayaAcctStatus_Attribute = new AvayaAttribute
            {
                AttributeName = "AccountStatus",
                AttributeDataType = "Text",
                DisplayString = "",
                IsEditable = false,
                MaxLength = 100,
                ValueString = "",
                LookUpTable = "AccountStatus"
            };
            Flatmodel.AccountStatus = avayaAcctStatus_Attribute;

            AvayaAttribute avayaACD_Attribute = new AvayaAttribute
            {
                AttributeName = "ACD",
                AttributeDataType = "Text",
                DisplayString = "Avaya",
                IsEditable = false,
                MaxLength = 100,
                ValueString = "Avaya",
                LookUpTable = ""
            };
            Flatmodel.ACD = avayaACD_Attribute;

            AvayaAttribute avayaAirportAttribute = new AvayaAttribute
            {
                AttributeName = "Airport",
                AttributeDataType = "DropDown",
                DisplayString = "EWR",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "EWR",
                LookUpTable = "Airport"
            };
            Flatmodel.Airport = avayaAirportAttribute;

            AvayaAttribute avayaAE_Attribute = new AvayaAttribute
            {
                AttributeName = "AvayaAE",
                AttributeDataType = "Text",
                DisplayString = "ANTHONY DI CARO",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "ANTHONY DI CARO",
                LookUpTable = "AvayaAE"
            };
            Flatmodel.AvayaAE = avayaAE_Attribute;

            AvayaAttribute avayaArea_Attribute = new AvayaAttribute
            {
                AttributeName = "AvayaAreaLeaders",
                AttributeDataType = "Text",
                DisplayString = "Dan Plunkett",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "Dan Plunkett",
                LookUpTable = "AvayaAreaLeaders"
            };
            Flatmodel.AvayaAreaLeaders = avayaArea_Attribute;

            AvayaAttribute avayaCSD_Attribute = new AvayaAttribute
            {
                AttributeName = "AvayaCSD",
                AttributeDataType = "Text",
                DisplayString = "Leland Gibbs",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "Leland Gibbs",
                LookUpTable = "AvayaCSD"
            };
            Flatmodel.AvayaCSD = avayaCSD_Attribute;

            AvayaAttribute avayaNotes_Attribute = new AvayaAttribute
            {
                AttributeName = "AvayaNotes",
                AttributeDataType = "Text",
                DisplayString = " 7-8K agents, 1M calls per month",
                IsEditable = true,
                MaxLength = 100,
                ValueString = " 7-8K agents, 1M calls per month",
                LookUpTable = ""
            };
            Flatmodel.AvayaNotes = avayaNotes_Attribute;

            AvayaAttribute AvayaPartner_Attribute = new AvayaAttribute
            {
                AttributeName = "AvayaPartner",
                AttributeDataType = "Text",
                DisplayString = "",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "",
                LookUpTable = ""
            };
            Flatmodel.AvayaPartner = AvayaPartner_Attribute;

            AvayaAttribute AvayaRegionalSalesLeaders_Attribute = new AvayaAttribute
            {
                AttributeName = "AvayaRegionalSalesLeaders",
                AttributeDataType = "Text",
                DisplayString = "Roger Mitchell",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "Roger Mitchell",
                LookUpTable = "AvayaRegionalSalesLeaders"
            };
            Flatmodel.AvayaRegionalSalesLeaders = AvayaRegionalSalesLeaders_Attribute;

            AvayaAttribute AvayaRelease_Attribute = new AvayaAttribute
            {
                AttributeName = "AvayaRelease",
                AttributeDataType = "Text",
                DisplayString = "",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "",
                LookUpTable = "AvayaRelease"
            };
            Flatmodel.AvayaRelease = AvayaRelease_Attribute;

            AvayaAttribute AvayaSE_Attribute = new AvayaAttribute
            {
                AttributeName = "AvayaSE",
                AttributeDataType = "Text",
                DisplayString = "",
                IsEditable = true,
                MaxLength = 100,
                LookUpTable = "AvayaSE",
                ValueString = ""
            };
            Flatmodel.AvayaSE = AvayaSE_Attribute;

            AvayaAttribute avayaCEOMeeting_Attribute = new AvayaAttribute
            {
                AttributeName = "CEOMeeting",
                AttributeDataType = "DropDown",
                DisplayString = "⬤",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "2",
                LookUpTable = "CEOMeeting"
            };
            Flatmodel.CEOMeeting = avayaCEOMeeting_Attribute;

            AvayaAttribute avayaCOOMeeting_Attribute = new AvayaAttribute
            {
                AttributeName = "COOMeeting",
                AttributeDataType = "DropDown",
                DisplayString = "⬤",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "0",
                LookUpTable = "COOMeeting"
            };
            Flatmodel.COOMeeting = avayaCOOMeeting_Attribute;

            AvayaAttribute avayaCTOMeetingAttribute = new AvayaAttribute
            {
                AttributeName = "CTOMeeting",
                AttributeDataType = "DropDown",
                DisplayString = "⬤",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "1",
                LookUpTable = "CTOMeeting"
            };
            Flatmodel.CTOMeeting = avayaCTOMeetingAttribute;

            AvayaAttribute avayaFirstQueueAttribute = new AvayaAttribute
            {
                AttributeName = "FirstQueue",
                AttributeDataType = "Text",
                DisplayString = "",
                IsEditable = false,
                MaxLength = 100,
                ValueString = "",
                LookUpTable = "FirstQueue"
            };
            Flatmodel.FirstQueue = avayaFirstQueueAttribute;

            AvayaAttribute avayaFirstQueueStatusAttribute = new AvayaAttribute
            {
                AttributeName = "FirstQueueStatus",
                AttributeDataType = "Text",
                DisplayString = "",
                IsEditable = false,
                MaxLength = 100,
                ValueString = "",
                LookUpTable = "FirstQueueStatus"
            };
            Flatmodel.FirstQueueStatus = avayaFirstQueueStatusAttribute;

            AvayaAttribute avayaJAPSAttribute = new AvayaAttribute
            {
                AttributeName = "JAPS",
                AttributeDataType = "DateTime",
                DisplayString = "2/20 AM",
                IsEditable = true,
                MaxLength = 100,
                ValueString = DateTime.Now.ToString()//"2/20 AM" 
                ,
                LookUpTable = ""
            };
            Flatmodel.JAPS = avayaJAPSAttribute;

            AvayaAttribute avayaLeadWithAttribute = new AvayaAttribute
            {
                AttributeName = "LeadWith",
                AttributeDataType = "DropDown",
                DisplayString = "EBP",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "EBP",
                LookUpTable = "LeadWith"
            };
            Flatmodel.LeadWith = avayaLeadWithAttribute;

            AvayaAttribute avayaPriorityAttribute = new AvayaAttribute
            {
                AttributeName = "Priority",
                AttributeDataType = "DropDown",
                DisplayString = "Diamond",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "Diamond",
                LookUpTable = "Priority"
            };
            Flatmodel.Priority = avayaPriorityAttribute;

            AvayaAttribute avayaRegionAttribute = new AvayaAttribute
            {
                AttributeName = "Region",
                AttributeDataType = "DropDown",
                DisplayString = "NE",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "NE",
                LookUpTable = "Region"
            };
            Flatmodel.Region = avayaRegionAttribute;

            AvayaAttribute avayaSalesNotesAttribute = new AvayaAttribute
            {
                AttributeName = "SalesNotes",
                AttributeDataType = "Text",
                DisplayString = "wireline knows what they are doing.about to do a pilot on ALL avaya, ",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "wireline knows what they are doing.about to do a pilot on ALL avaya, ",
                LookUpTable = ""
            };
            Flatmodel.SalesNotes = avayaSalesNotesAttribute;

            AvayaAttribute avayaSeatsAttribute = new AvayaAttribute
            {
                AttributeName = "Seats",
                AttributeDataType = "Text",
                DisplayString = "7500",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "7500",
                LookUpTable = ""
            };
            Flatmodel.Seats = avayaSeatsAttribute;

            AvayaAttribute avayaSegmentAttribute = new AvayaAttribute
            {
                AttributeName = "Segment",
                AttributeDataType = "DropDown",
                DisplayString = "",
                IsEditable = true,
                MaxLength = 100,
                ValueString = "",
                LookUpTable = "Segment"
            };
            Flatmodel.Segment = avayaSegmentAttribute;

            AvayaAttribute avayaVerticalAttribute = new AvayaAttribute
            {
                AttributeName = "Vertical",
                AttributeDataType = "Text",
                DisplayString = "Communications",
                IsEditable = false,
                MaxLength = 100,
                ValueString = "Communications",
                LookUpTable = ""
            };
            Flatmodel.Vertical = avayaVerticalAttribute;
            FlatmodelList.Add(Flatmodel);
            #endregion 1stAiroAcct

            List<string> str = new List<string> { "Quicken Loans", "Disney Cruise Lines", "Synchrony", "MGM", "Centene", "CenturyLink", "Cox Communications ", "AT&T", "CVS | AETNA", "Humana", "AAA", "Avis", "Sands", "Wynn", "Carnival Cruise Lines", "Norwegian Cruise Line", "Royal Caribbean Cruise Line", "ADT", "Teleperformance", "Exelon", "Johnson Controls US", "Goldman", "Prudential", "Highmark", "Abt Electronics", "Standard Charter", "Peckham, Inc", "AAA NorthEast", "ICE", "MetLife", "American Equity", "Carnival Cruise Lines", "Allstate", "Tesla", "McKinsey", "Avaya Service Desk", "Four Seasons", "TiVo", "Frost Bank", "Brinks Home Security", "Aeroflot" };
            foreach (var item in str)
            {
                var serialized = JsonConvert.SerializeObject(Flatmodel);
                var tt = JsonConvert.DeserializeObject<ListServiceProxy.ServiceReference.AvayaAccountsFlatModel>(serialized);
                // tt.UniqueKey = Guid.NewGuid();
                tt.Account.DisplayString = tt.Account.ValueString = item;
                tt.CEOMeeting.ValueString = "3";
                FlatmodelList.Add(tt);
            }

            ViewerAndPagingOfAvayaAccountsFlatModelmGFQGkGm viewerAndPaging = new ViewerAndPagingOfAvayaAccountsFlatModelmGFQGkGm();
            viewerAndPaging.PageInformation = pPagingClass;
            viewerAndPaging.Message = "Success";
            viewerAndPaging.Code = "200";
            viewerAndPaging.ReviewResults = FlatmodelList;
            return viewerAndPaging;
        }
        */

        #region General
        private SearchCriteria CreateSearchCriteriaForAiro(SearchParms pSearchCriteria)
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
                            if (strParamName == "AccountLeadKey" || strParamName == "AccountPhaseKey")//these are Text attributeDataType in Metadata for now..will be changed later on as per need to give filters to screen
                            {
                                castedValue = Guid.Parse(filter.value);
                            }
                            else if (strParamName == "RegionKey")
                            {
                                castedValue = short.Parse(filter.value);
                            }
                            else//AccountStatusKey is also added in it
                            {
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
                //case "account":
                //    return "AccountKey";
                //case "priority":
                //    return "PriorityKey";
                //case "leadwith":
                //    return "LeadWithKey";
                case "accountstatus":
                    return "AccountStatusKey";
                case "accountphase":
                    return "AccountPhaseKey";
                //case "firstqueue":
                //    return "FirstQueueKey";
                //case "firstqueuestatus":
                //    return "FirstQueueStatusKey";
                //case "ceomeeting":
                //    return "CEOMeetingKey";
                //case "ctomeeting":
                //    return "CTOMeetingKey";
                //case "coomeeting":
                //    return "COOMeetingKey";
                case "accountlead":
                    return "AccountLeadKey";
                //case "vertical":
                //    return "VerticalKey";
                //case "seats":
                //    return "SeatsKey";
                //case "acd":
                //    return "ACDKey";
                //case "avayaarealeaders":
                //    return "AvayaAreaLeadersKey";
                //case "avayaregionalsalesleaders":
                //    return "AvayaRegionalSalesLeadersKey";
                //case "avayaae":
                //    return "AvayaAEKey";
                //case "avayase":
                //    return "AvayaSEKey";
                //case "avayacsd":
                //    return "AvayaCSDKey";
                //case "japs":
                //    return "JAPSKey";
                case "region":
                    return "RegionKey";
                //case "airport":
                //    return "AirportKey";
                //case "country":
                //    return "CountryKey";
                default:
                    return "";
            }
        }


        #endregion General
    }
}

