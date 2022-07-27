using Common;
using Common.SharedModels;
using ListServiceProxy.ExtendedModels;
using ListServiceProxy.Models;
using ListServiceProxy.ServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ListServiceProxy
{
    public class ListProxy
    {
        #region Constructor
        public ListProxy(HttpRequestHeaders headers)
        {
            if (headers != null && headers.Contains("cgate_api"))
            {
                //var cgate_api = "obDMsnSDAvDtpxnuKanDKnCw7V9VLzI/W0se9FjzUVXI1VMymdKVPSpMAZiKuMUgiM9+upighlVkMPl0vGEiV0PltUGuEAIC1lMT1A2DWDBzfTxQkW4kaRN7jkxl7ZpCkQlVEy5Tb+OnhKBXRNLWHQ==";
                var cgate_api = headers.GetValues("cgate_api")?.FirstOrDefault();
                if (!string.IsNullOrEmpty(headers.GetValues("cgate_api")?.FirstOrDefault()))
                {
                    var decrypt = Encryption.Encrypt_Decrypt(cgate_api, "AFINITIGENIECODE", "AFINITIGENIECODE", 1, 128, false);//always decrypt
                    if (!string.IsNullOrEmpty(decrypt))
                    {
                        string[] list = decrypt.Split('|');
                        var remoteKey = list[2]?.ToString();

                        MessageHeader<String> session = new MessageHeader<String>("asd");
                        MessageHeader<String> userid = new MessageHeader<String>(remoteKey);
                        MessageHeader<String> remoteaddr = new MessageHeader<String>("192.168.1.1");
                        MessageHeader<String> culture = new MessageHeader<String>("en-US");
                        MessageHeader<String> usertoken = new MessageHeader<String>("123456");
                        OperationContextScope contextScope = new OperationContextScope(ServiceObject.InnerChannel);
                        OperationContext.Current.OutgoingMessageHeaders.Add(session.GetUntypedHeader("session", "ns"));
                        OperationContext.Current.OutgoingMessageHeaders.Add(userid.GetUntypedHeader("userid", "ns"));
                        OperationContext.Current.OutgoingMessageHeaders.Add(remoteaddr.GetUntypedHeader("remoteaddr", "ns"));
                        OperationContext.Current.OutgoingMessageHeaders.Add(culture.GetUntypedHeader("culture", "ns"));
                        OperationContext.Current.OutgoingMessageHeaders.Add(usertoken.GetUntypedHeader("usertoken", "ns"));
                        reflection = contextScope;
                    }
                }
            }
        }
        public ListProxy(string remoteKey)
        {
            if (!string.IsNullOrEmpty(remoteKey))
            {
                MessageHeader<String> session = new MessageHeader<String>("asd");
                MessageHeader<String> userid = new MessageHeader<String>(remoteKey);
                MessageHeader<String> remoteaddr = new MessageHeader<String>("192.168.1.1");
                MessageHeader<String> culture = new MessageHeader<String>("en-US");
                MessageHeader<String> usertoken = new MessageHeader<String>("123456");
                OperationContextScope contextScope = new OperationContextScope(ServiceObject.InnerChannel);
                OperationContext.Current.OutgoingMessageHeaders.Add(session.GetUntypedHeader("session", "ns"));
                OperationContext.Current.OutgoingMessageHeaders.Add(userid.GetUntypedHeader("userid", "ns"));
                OperationContext.Current.OutgoingMessageHeaders.Add(remoteaddr.GetUntypedHeader("remoteaddr", "ns"));
                OperationContext.Current.OutgoingMessageHeaders.Add(culture.GetUntypedHeader("culture", "ns"));
                OperationContext.Current.OutgoingMessageHeaders.Add(usertoken.GetUntypedHeader("usertoken", "ns"));
                reflection = contextScope;
            }

        }
        #endregion

        #region Configuration

        private static ServiceReference.ListServiceClient _serviceClient = null;
        private static ServiceReference.ListServiceClient ServiceObject
        {
            get
            {
                if (_serviceClient == null ||
                            (_serviceClient != null && _serviceClient.State != System.ServiceModel.CommunicationState.Created &&
                            _serviceClient.State != System.ServiceModel.CommunicationState.Opened)
                   )
                {
                    _serviceClient = new ServiceReference.ListServiceClient();
                }
                return _serviceClient;
            }
        }
        private static OperationContextScope reflection = null;

        #endregion

        #region SummaryPipeline

        public Task<ViewerAndPagingOfPipelineSummaryFlatModelmGFQGkGm> GetPiplineSummaryFlatModel(SearchCriteria pCriteria, PagingClass pPagingClass)
        {
            return ServiceObject.GetPiplineSummaryFlatModelAsync(pCriteria, pPagingClass);
        }

        public Task<ViewerAndPagingOfPipelineSummaryFlatModelmGFQGkGm> GetPiplineSummaryFlatModelExport(SearchCriteria pCriteria, PagingClass pPagingClass)
        {
            return ServiceObject.GetPiplineSummaryFlatModelForExportAsync(pCriteria, pPagingClass);
        }

        public Task<bool> UpdatePipelineSummary(Guid pUniqueKey, string pProperty, string pPropertyValue)
        {
            return ServiceObject.UpdatePipelineSummaryAsync(pUniqueKey, pProperty, pPropertyValue);
        }

        public async Task<SummaryPipelineMetaDataCustom> GetPipelineMetaData()
        {
            SummaryPipelineMetaDataCustom customModel = new SummaryPipelineMetaDataCustom();
            List<KeyValPairShortModelCustom> KeyPairListModel = new List<KeyValPairShortModelCustom>();
            List<KeyValPairGuidModelCustom> KeyPairGuidModel = new List<KeyValPairGuidModelCustom>();
            List<KeyValPairModelCustom> KeyPairStrModel = new List<KeyValPairModelCustom>();

            customModel.QueueStatusList = new List<KeyValPairShortModelCustom>();
            customModel.Accounts = new List<KeyValPairGuidModelCustom>();
            customModel.Industries = new List<KeyValPairGuidModelCustom>();
            customModel.Countries = new List<KeyValPairGuidModelCustom>();
            customModel.People = new List<KeyValPairGuidModelCustom>();
            customModel.Roles = new List<KeyValPairGuidModelCustom>();
            customModel.AccountPhase = new List<KeyValPairGuidModelCustom>();
            customModel.QueueType = new List<KeyValPairShortModelCustom>();
            customModel.CC1Status = new List<KeyValPairModelCustom>();
            customModel.CC2Status = new List<KeyValPairModelCustom>();

            var data = await ServiceObject.GetPipelineMetaDataAsync();

            KeyPairListModel = KeyValPairModelMapper.CustomizeShortKeyPair(data.QueueStatusList.ToList<dynamic>());
            customModel.QueueStatusList.AddRange(KeyPairListModel);

            KeyPairListModel = KeyValPairModelMapper.CustomizeShortKeyPair(data.QueueType.ToList<dynamic>());
            customModel.QueueType.AddRange(KeyPairListModel);

            KeyPairGuidModel = KeyValPairModelMapper.CustomizeGuidKeyPair(data.Accounts.ToList<dynamic>());
            customModel.Accounts.AddRange(KeyPairGuidModel);

            KeyPairGuidModel = KeyValPairModelMapper.CustomizeGuidKeyPair(data.Industries.ToList<dynamic>());
            customModel.Industries.AddRange(KeyPairGuidModel);

            KeyPairGuidModel = KeyValPairModelMapper.CustomizeGuidKeyPair(data.Countries.ToList<dynamic>());
            customModel.Countries.AddRange(KeyPairGuidModel);

            KeyPairGuidModel = KeyValPairModelMapper.CustomizeGuidKeyPair(data.People.ToList<dynamic>());
            customModel.People.AddRange(KeyPairGuidModel);

            KeyPairGuidModel = KeyValPairModelMapper.CustomizeGuidKeyPair(data.Roles.ToList<dynamic>());
            customModel.Roles.AddRange(KeyPairGuidModel);

            KeyPairGuidModel = KeyValPairModelMapper.CustomizeGuidKeyPair(data.AccountPhase.ToList<dynamic>());
            customModel.AccountPhase.AddRange(KeyPairGuidModel);

            KeyPairStrModel = KeyValPairModelMapper.CustomizeStringKeyPair(data.CC1Status.ToList<dynamic>());
            customModel.CC1Status.AddRange(KeyPairStrModel);

            KeyPairStrModel = KeyValPairModelMapper.CustomizeStringKeyPair(data.CC2Status.ToList<dynamic>());
            customModel.CC2Status.AddRange(KeyPairStrModel);

            return customModel;
        }

        #endregion SummaryPipeline

        #region CommercialGate

        public Task<CommercialGateFlatModel> GetSubmittedCommercialGateFlatModelByAccountKey(Guid pAccountKey)
        {
            return ServiceObject.GetSubmittedCommercialGateFlatModelByAccountKeyAsync(pAccountKey);
        }

        public Task<CommercialGateMetaData> GetCommercialGateMetaDataAsync()
        {
            return ServiceObject.GetCommercialGateMetaDataAsync();
        }

        public async Task<CommercialGateMetaDataCustom> GetCommercialGateMetaData1()
        {
            CommercialGateMetaDataCustom customModel = new CommercialGateMetaDataCustom();
            List<KeyValPairModelCustom> KeyPairListModel = new List<KeyValPairModelCustom>();
            List<KeyValPairIntModelCustom> KeyPairIntListModel = new List<KeyValPairIntModelCustom>();
            customModel.ACDList = new List<KeyValPairModelCustom>();
            customModel.AccountTeamLocation = new List<KeyValPairModelCustom>();
            customModel.DeploymentComplexity = new List<KeyValPairModelCustom>();
            customModel.DeploymentLocation = new List<KeyValPairModelCustom>();
            customModel.LevelList = new List<KeyValPairModelCustom>();
            customModel.NewDeployment = new List<KeyValPairModelCustom>();
            customModel.OptimizationMetricType = new List<KeyValPairIntModelCustom>();
            customModel.PilotPricing = new List<KeyValPairModelCustom>();
            customModel.PricingLogic = new List<KeyValPairIntModelCustom>();
            customModel.QueuePhase = new List<KeyValPairModelCustom>();
            customModel.UpliftType = new List<KeyValPairIntModelCustom>();
            customModel.YearsList = new List<KeyValPairModelCustom>();

            var data = await ServiceObject.GetCommercialGateMetaDataAsync();

            KeyPairListModel = KeyValPairModelMapper.CustomizeStringKeyPair(data.ACDList.ToList<dynamic>());
            customModel.ACDList.AddRange(KeyPairListModel);

            KeyPairListModel = KeyValPairModelMapper.CustomizeStringKeyPair(data.AccountTeamLocation.ToList<dynamic>());
            customModel.AccountTeamLocation.AddRange(KeyPairListModel);

            KeyPairListModel = KeyValPairModelMapper.CustomizeStringKeyPair(data.DeploymentComplexity.ToList<dynamic>());
            customModel.DeploymentComplexity.AddRange(KeyPairListModel);

            KeyPairListModel = KeyValPairModelMapper.CustomizeStringKeyPair(data.DeploymentLocation.ToList<dynamic>());
            customModel.DeploymentLocation.AddRange(KeyPairListModel);

            KeyPairListModel = KeyValPairModelMapper.CustomizeStringKeyPair(data.LevelList.ToList<dynamic>());
            customModel.LevelList.AddRange(KeyPairListModel);

            KeyPairListModel = KeyValPairModelMapper.CustomizeStringKeyPair(data.NewDeployment.ToList<dynamic>());
            customModel.NewDeployment.AddRange(KeyPairListModel);

            KeyPairIntListModel = KeyValPairModelMapper.CustomizeIntKeyPair(data.OptimizationMetricType.ToList<dynamic>());
            customModel.OptimizationMetricType.AddRange(KeyPairIntListModel);

            KeyPairListModel = KeyValPairModelMapper.CustomizeStringKeyPair(data.PilotPricing.ToList<dynamic>());
            customModel.PilotPricing.AddRange(KeyPairListModel);

            KeyPairIntListModel = KeyValPairModelMapper.CustomizeIntKeyPair(data.PricingLogic.ToList<dynamic>());
            customModel.PricingLogic.AddRange(KeyPairIntListModel);

            KeyPairListModel = KeyValPairModelMapper.CustomizeStringKeyPair(data.QueuePhase.ToList<dynamic>());
            customModel.QueuePhase.AddRange(KeyPairListModel);

            KeyPairIntListModel = KeyValPairModelMapper.CustomizeIntKeyPair(data.UpliftType.ToList<dynamic>());
            customModel.UpliftType.AddRange(KeyPairIntListModel);

            KeyPairListModel = KeyValPairModelMapper.CustomizeStringKeyPair(data.YearsList.ToList<dynamic>());
            customModel.YearsList.AddRange(KeyPairListModel);

            return customModel;
        }

        public Task<ViewerAndPagingOfCommercialGateFlatModelTrimmedmGFQGkGm> GetCommercialGateFlatModelsAsync(/*SearchCriteria pCriteria, PagingClass pPagingClass*/)
        {
            SearchCriteria pCriteria = new SearchCriteria { SearchFiltersAndValues = new List<SearchFilter>() };
            //pCriteria.SearchFiltersAndValues.Add(new SearchFilter { SearchParameter = "AccountKey", SearchValue = "", SearchExp = SearchExpression.And });
            // SortCriteria sc = new SortCriteria() { SortColumn = "AccountName", SortDirection = SortOrder.Ascending };
            return ServiceObject.GetCommercialGateFlatModelsAsync(/*pCriteria,pSortCriteria */pCriteria, new PagingClass { CurrentPage = 1, ItemsPerPage = int.MaxValue });
        }

        public Task<List<KeyValPairGuid>> SearchInternalContacts(string searchVal)
        {
            return ServiceObject.SearchInternalContactsAsync(searchVal);
        }

        public Task<List<KeyValPairGuid>> SearchExternalContacts(string searchVal)
        {
            return ServiceObject.SearchExternalContactsAsync(searchVal);
        }

        public Task<List<KeyValPairGuid>> SearchBoardContacts(string searchVal)
        {
            return ServiceObject.SearchBoardContactsAsync(searchVal);
        }

        public Task<bool> RecommendChangesToCommercialGateFlat(Guid pUniqueKey, string pProperty, string pPropertyValue)
        {
            return ServiceObject.RecommendChangesToCommercialGateFlatAsync(pUniqueKey, pProperty, pPropertyValue);
        }

        public Task<CommercialGateModel> AddCommercialGate(Guid pAccountkey, Guid pQueueKey)
        {
            return ServiceObject.AddCommercialGateAsync(pAccountkey, pQueueKey);
        }

        public Task<CommercialGateQueueModel> AddCommercialGateQueueModel(Guid pAccountkey, Guid pQueueKey)
        {
            return ServiceObject.AddCommercialGateQueueModelAsync(pAccountkey, pQueueKey);
        }

        public Task<CommercialText> AddCommercialDiscussionToModel(Guid pAccountKey, CommercialText pModel)
        {
            return ServiceObject.AddCommercialDiscussionToModelAsync(pAccountKey, pModel);
        }

        public Task<ViewerAndPagingOfCommercialTextmGFQGkGm> GetCommercialDiscussionFromModelByCritera(Guid pAccountKey)
        {
            return ServiceObject.GetCommercialDiscussionFromModelByCriteraAsync(pAccountKey);
        }

        public Task<bool> SendDocumentSpaceCreationReqeustToSystemsAdmin(Guid pAccountKey)
        {
            return ServiceObject.SendDocumentSpaceCreationReqeustToSystemsAdminAsync(pAccountKey);
        }

        public Task<bool> ApproveAccount(Guid pUniqueKey)
        {
            return ServiceObject.ApproveAccountAsync(pUniqueKey);
        }

        public Task<bool> ApproveQueue(Guid pUniqueKey)
        {
            return ServiceObject.ApproveQueueAsync(pUniqueKey);
        }

        public Task<bool> RejectAccount(Guid pUniqueKey)
        {
            return ServiceObject.RejectAccountAsync(pUniqueKey);
        }

        public Task<bool> RejectQueue(Guid pUniqueKey)
        {
            return ServiceObject.RejectQueueAsync(pUniqueKey);
        }

        public Task<FlatModelExport> GetCommercialGateFlatModelsExport(SearchCriteria pCriteria, PagingClass pPagingClass)
        {
            return ServiceObject.GetCommercialGateFlatModelForExportAsync(pCriteria, pPagingClass);
        }

        #endregion CommercialGate

        #region Airo


        public Task<AvayaContactModel> AddAiroContact(AvayaContactModel model)
        {
            return ServiceObject.CreateAvayaContactAsync(model);
        }

        public Task<bool> UpdateAiroAccount(AiroUpdateRequestModel model)
        {
            return ServiceObject.UpdateAvayaSalesSuiteAsync(model.pUniqueKey, model.pProperty, model.pPropertyValue);
        }

        public Task<ViewerAndPagingOfAvayaAccountsFlatModelmGFQGkGm> GetAiroFlatModel(SearchCriteria pCriteria, PagingClass pPagingClass)
        {
            return ServiceObject.GetAvayaAccountsFlatModelAsync(pCriteria, pPagingClass);
        }

        public Task<ViewerAndPagingOfAvayaAccountsFlatModelmGFQGkGm> GetAiroFlatModelExport(SearchCriteria pCriteria, PagingClass pPagingClass)
        {
            return ServiceObject.GetAvayaAccountsFlatModelForExportAsync(pCriteria, pPagingClass);
        }

        public async Task<AiroMetaDataCustom> GetAiroMetaData()
        {
            AiroMetaDataCustom customModel = new AiroMetaDataCustom();
            List<KeyValPairGuidModelCustom> KeyPairGuidModel = new List<KeyValPairGuidModelCustom>();
            List<KeyValPairGuidWithAdditionalModelCustom> KeyPairGuidWithAdditionalModel = new List<KeyValPairGuidWithAdditionalModelCustom>();
            List<KeyValPairShortModelCustom> KeyPairShortModel = new List<KeyValPairShortModelCustom>();

            customModel.Airport = new List<KeyValPairShortModelCustom>();
            customModel.AvailableQueues = new List<KeyValPairGuidWithAdditionalModelCustom>();
            customModel.AvayaPeople = new List<KeyValPairGuidModelCustom>();
            customModel.CLevelMeetingStatus = new List<KeyValPairShortModelCustom>();
            customModel.LeadWith = new List<KeyValPairShortModelCustom>();
            customModel.Priority = new List<KeyValPairShortModelCustom>();
            customModel.Region = new List<KeyValPairShortModelCustom>();
            customModel.Segment = new List<KeyValPairShortModelCustom>();
            customModel.Title = new List<KeyValPairGuidModelCustom>();

            var data = await ServiceObject.GetAvayaSalesSuiteMetaDataAsync();
            if (data.Airport != null)
            {
                KeyPairShortModel = KeyValPairModelMapper.CustomizeShortKeyPair(data.Airport.ToList<dynamic>());
                customModel.Airport.AddRange(KeyPairShortModel);
            }

            if (data.AvailableQueues != null)
            {
                KeyPairGuidWithAdditionalModel = KeyValPairModelMapper.CustomizeGuidKeyPairAdditional(data.AvailableQueues.ToList<dynamic>());
                customModel.AvailableQueues.AddRange(KeyPairGuidWithAdditionalModel);
            }

            if (data.AvayaPeople != null)
            {
                KeyPairGuidModel = KeyValPairModelMapper.CustomizeGuidKeyPair(data.AvayaPeople.ToList<dynamic>());
                customModel.AvayaPeople.AddRange(KeyPairGuidModel);
            }

            if (data.CLevelMeetingStatus != null)
            {
                KeyPairShortModel = KeyValPairModelMapper.CustomizeShortKeyPair(data.CLevelMeetingStatus.ToList<dynamic>());
                customModel.CLevelMeetingStatus.AddRange(KeyPairShortModel);
            }

            if (data.LeadWith != null)
            {
                KeyPairShortModel = KeyValPairModelMapper.CustomizeShortKeyPair(data.LeadWith.ToList<dynamic>());
                customModel.LeadWith.AddRange(KeyPairShortModel);
            }
            if (data.Priority != null)
            {
                KeyPairShortModel = KeyValPairModelMapper.CustomizeShortKeyPair(data.Priority.ToList<dynamic>());
                customModel.Priority.AddRange(KeyPairShortModel);
            }

            if (data.Region != null)
            {
                KeyPairShortModel = KeyValPairModelMapper.CustomizeShortKeyPair(data.Region.ToList<dynamic>());
                customModel.Region.AddRange(KeyPairShortModel);
            }

            if (data.Segment != null)
            {
                KeyPairShortModel = KeyValPairModelMapper.CustomizeShortKeyPair(data.Segment.ToList<dynamic>());
                customModel.Segment.AddRange(KeyPairShortModel);
            }

            if (data.Title != null)
            {
                KeyPairGuidModel = KeyValPairModelMapper.CustomizeGuidKeyPair(data.Title.ToList<dynamic>());
                customModel.Title.AddRange(KeyPairGuidModel);


                //KeyPairShortModel = KeyValPairModelMapper.CustomizeShortKeyPair(data.Title.ToList<dynamic>());
                //customModel.Title.AddRange(KeyPairShortModel);
            }
            return customModel;
        }

        public async Task<List<KeyValPairGuidModelCustom>> GetAiRoSearchContacts(string searchVal)
        {
            List<KeyValPairGuidModelCustom> KeyPairGuidModel = new List<KeyValPairGuidModelCustom>();
            var data = ServiceObject.AiroSearchContactsAsync(searchVal);
            KeyPairGuidModel = KeyValPairModelMapper.CustomizeGuidKeyPair(data.Result.ToList<dynamic>());
            return KeyPairGuidModel;
        }

        #endregion Airo
    }
}
