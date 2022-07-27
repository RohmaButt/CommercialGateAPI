using Afiniti.Framework.LoggingTracing;
using Common;
using SecurityServiceProxy.ExtendedModels;
using SecurityServiceProxy.Models;
using SecurityServiceProxy.SecurityServiceReference;
//using SecurityServiceProxy.ServiceReference_Local;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SecurityServiceProxy
{
    public class SecurityProxy
    {
        #region ClientCreation
        //private static ServiceReference_Local.SecurityService_CommercialGateClient _serviceClient;
        //private static ServiceReference_Local.SecurityService_CommercialGateClient ServiceObject
        //{
        //    get
        //    {
        //        if (_serviceClient == null ||
        //            (_serviceClient != null && _serviceClient.State != CommunicationState.Created &&
        //             _serviceClient.State != CommunicationState.Opened)
        //        )
        //        {
        //            _serviceClient = new ServiceReference_Local.SecurityService_CommercialGateClient();
        //        }
        //        return _serviceClient;
        //    }
        //}
        private static SecurityServiceReference.SecurityService_CommercialGateClient _serviceClient;
        private static SecurityServiceReference.SecurityService_CommercialGateClient ServiceObject
        {
            get
            {
                if (_serviceClient == null ||
                    (_serviceClient != null && _serviceClient.State != CommunicationState.Created &&
                     _serviceClient.State != CommunicationState.Opened)
                )
                {
                    _serviceClient = new SecurityServiceReference.SecurityService_CommercialGateClient();
                }
                return _serviceClient;
            }
        }

        #endregion ClientCreation

        public async Task<List<MenuPermission>> GetGenieAdminMenuForUser(string CrowdToken, string IPAddress)//Get AdminMenuList as per UserPermissions of controller in Security DB
        {
            List<MenuPermission> ResponseListObj = new List<MenuPermission>();
            var model = await ServiceObject.GetUsersSecurityDataByToken_CGAsync(CrowdToken, IPAddress);
            ApplicationTrace.Log("CommercialGateAPI", "GetGenieAdminMenuForUser", Status.Started);
            if (model == null)
            {
               ApplicationTrace.Log("CommercialGateAPI", " GetGenieAdminMenuForUser: model is null", Status.Started);

                return null;
            }
            if (model.Permissions.Any())
            {
               ApplicationTrace.Log("CommercialGateAPI", " GetGenieAdminMenuForUser: permissionmodel is not null", Status.Started);
                if (model.Permissions.Any(x => x.TypeKey == PermissionType.Attribute.Value))
                {
                    ApplicationTrace.Log("CommercialGateAPI", " GetGenieAdminMenuForUser: permissionmodel has menu permissions", Status.Started);
                   var mainMenus = model.Permissions
                          .Where(x => x.TypeKey == PermissionType.Attribute.Value
                              && x.AdminLevel == false
                              && x.ParentKey == null
                             && x.Key == "CanViewAdminAppLink")
                          .GroupJoin(model.Permissions
                              .Where(x => x.TypeKey == PermissionType.Attribute.Value
                                  && x.AdminLevel == false
                                  && x.ParentKey != null),
                              p => p.PermissionKey,
                              m => m.ParentKey,
                              (x, y) => new { Parent = x, Sub = y });
                    ResponseListObj = PermissionModelMapper.CustomizeAdminMenuList(mainMenus.ToList<dynamic>());
                    ApplicationTrace.Log("CommercialGateAPI", " GetGenieAdminMenuForUser: permissionmodel has set", Status.Started);
                }
                else
                {
                    return ResponseListObj;
                }
            }
            else
            {
                return ResponseListObj;
            }

            ApplicationTrace.Log("CommercialGateAPI", "GetGenieAdminMenuForUser", Status.Completed);

            return ResponseListObj;
        }


        public async Task<List<MenuPermission>> GetGenieLeftMenuForUser(string CrowdToken, string IPAddress)//Get MenuList as per UserPermissions of controller in Security DB
        {
            List<MenuPermission> ResponseListObj = new List<MenuPermission>();
            var model = await ServiceObject.GetUsersSecurityDataByToken_CGAsync(CrowdToken, IPAddress);
            ApplicationTrace.Log("CommercialGateAPI", "GetGenieLeftMenuForUser", Status.Started);
            if (model == null)
            {
               ApplicationTrace.Log("CommercialGateAPI", "GetGenieLeftMenuForUser GetGenieLeftMenuForUser: model is null", Status.Started);
                return null;
            }
            if (model.Permissions.Any())
            {
               ApplicationTrace.Log("CommercialGateAPI", "GetGenieLeftMenuForUser: permissionmodel is not null", Status.Started);
                if (model.Permissions.Any(x => x.TypeKey == PermissionType.Menu.Value))
                {
                    ApplicationTrace.Log("CommercialGateAPI", "GetGenieLeftMenuForUser: permissionmodel has menu permissions", Status.Started);
                    var mainMenus = model.Permissions
                          .Where(x => x.TypeKey == PermissionType.Menu.Value
                              && x.AdminLevel == false
                              && x.ParentKey == null)
                          .GroupJoin(model.Permissions
                              .Where(x => x.TypeKey == PermissionType.Menu.Value
                                  && x.AdminLevel == false
                                  && x.ParentKey != null),
                              p => p.PermissionKey,
                              m => m.ParentKey,
                              (x, y) => new { Parent = x, Sub = y });
                    ResponseListObj = PermissionModelMapper.CustomizeMenuList(mainMenus.ToList<dynamic>());
                    ApplicationTrace.Log("CommercialGateAPI", "GetGenieLeftMenuForUser: permissionmodel has set", Status.Started);
               }
                else
                {
                    return ResponseListObj;
                }
            }
            else
            {
                return ResponseListObj;
            }
             ApplicationTrace.Log("CommercialGateAPI", "GetGenieLeftMenuForUser", Status.Completed);
            return ResponseListObj;
        }

        public UserDTO AuthenticateAndAuthorizeByCrowd_CG(string CrowdToken, string IPAddress, string ControllerPath)// Authorize
        {
            return ServiceObject.AuthenticateAndAuthorizeByCrowd_CG(CrowdToken, IPAddress, ControllerPath);
        }

        public async Task<CrowdUserObj> AuthenticateByCrowd_CG(string CrowdToken)//Authenticate
        {
            return await ServiceObject.AuthenticateByCrowd_CGAsync(CrowdToken);
        }
    }
}
