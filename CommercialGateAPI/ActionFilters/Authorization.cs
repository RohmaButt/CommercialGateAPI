using Afiniti.Framework.LoggingTracing;
using Common;
using SecurityServiceProxy;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CommercialGateAPI.ActionFilters
{
    //Authorization deals with controller access only for user.
    //P.S : authentication of SSOToken is a pre-req to fulfill.so this  filter works for Authentication and Authorizatio both at same time.
    public class CustomAuthorization : AuthorizationFilterAttribute
    {
        public override Task OnAuthorizationAsync(HttpActionContext context, CancellationToken cancellationToken)
        {
            try
            {
                string RemoteKey = "";
                var cgate_api = "";
                string SSOToken = "";
                var decrypt = "";
                if (context.Request.Headers != null && context.Request.Headers.Contains("cgate_api"))
                {
                    if (context.Request.Headers.GetValues("cgate_api").FirstOrDefault().ToString() == "undefined" || string.IsNullOrEmpty(context.Request.Headers.GetValues("cgate_api").FirstOrDefault().ToString()) || String.IsNullOrWhiteSpace(context.Request.Headers.GetValues("cgate_api").FirstOrDefault().ToString()))
                    {
                        context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized.");
                        return Task.FromResult<object>(context.Response);
                    }
                    cgate_api = context.Request.Headers.GetValues("cgate_api")?.FirstOrDefault();
                    if (!string.IsNullOrEmpty(cgate_api))
                    {
                        decrypt = Encryption.Encrypt_Decrypt(cgate_api, "AFINITIGENIECODE", "AFINITIGENIECODE", 1, 128, false);//always decrypt
                        if (!string.IsNullOrEmpty(decrypt))
                        {
                            string[] list = decrypt?.Split('|');
                            SSOToken = list[0]?.ToString();
                            string jSession = list[1]?.ToString();
                            RemoteKey = list[2]?.ToString();
                            string UserName = list[3]?.ToString();
                        }
                        else
                        {
                            context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, "BadRequest");
                            return Task.FromResult<object>(context.Response);
                        }
                    }
                    else
                    {
                        context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized.");
                        return Task.FromResult<object>(context.Response);
                    }
                }
                else
                {
                    context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
                    return Task.FromResult<object>(context.Response);
                }
                var ControllerUrl = context.Request.RequestUri.AbsolutePath;
                int startPosition = ControllerUrl.IndexOf("api/");
                ControllerUrl = ControllerUrl.Substring(startPosition);
                if (string.IsNullOrEmpty(SSOToken) || string.IsNullOrEmpty(RemoteKey) || string.IsNullOrEmpty(ControllerUrl))
                {
                    context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
                    return Task.FromResult<object>(context.Response);
                }
                ApplicationTrace.Log("Authorization", "CustomAuthorization", Status.Started);

                ApplicationTrace.Log("Authorization", "CustomAuthorization Cookie:" + cgate_api, Status.Started);
                ApplicationTrace.Log("Authorization", "CustomAuthorization decryptCookie:" + decrypt, Status.Completed);


                SecurityProxy ProxyObj = new SecurityProxy();
                var UserDTO = ProxyObj.AuthenticateAndAuthorizeByCrowd_CG(SSOToken, "", ControllerUrl.ToLower());
                if (UserDTO != null)
                {
                    if (string.IsNullOrEmpty(UserDTO.CrowdObj.UserName) || string.IsNullOrEmpty(UserDTO.CrowdObj.JSessionID))
                    {
                        context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
                        return Task.FromResult<object>(context.Response);
                    }
                    if (UserDTO.Permissions != null && UserDTO.Permissions.Count > 0 && UserDTO.Permissions.Any(x => x.URL == ControllerUrl))
                    {
                        return Task.FromResult<object>(null);
                    }
                    else if (UserDTO.Permissions == null || UserDTO.Permissions.Count == 0 || !UserDTO.Permissions.Any(x => x.URL == ControllerUrl))
                    {
                        context.Response = context.Request.CreateResponse(HttpStatusCode.Forbidden, "Forbidden");
                        return Task.FromResult<object>(context.Response);
                    }
                }
                else
                {
                    context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
                    return Task.FromResult<object>(context.Response);
                }
                return Task.FromResult<object>(context.Response);
            }
            catch (Exception ex) when (ex.Message == "Invalid length for a Base-64 char array or string.")
            //if there is issue with descryption of cgate_api then React app requires to receive 401 for this exception rather than general exception of 500(Discussion with Bilal)
            {
                LoggingUtility.LogException(ex);
                context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized :" + ex);
                return Task.FromResult<object>(context.Response);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogException(ex);
                context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, "InternalServerError :" + ex);
                return Task.FromResult<object>(context.Response);
                // throw new Exception("Something went wrong with Authorization. Please contact Connect.");
            }
        }
    }
}


