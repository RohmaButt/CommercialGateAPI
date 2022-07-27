using Common;
using SecurityServiceProxy;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace CommercialGateAPI.ActionFilters
{
    //Authentication of user deals with CrowdSSOKey only and its validation from Security Service.
    public class CustomAuthentication : ActionFilterAttribute, IAuthenticationFilter
    {
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            try
            {
                string SSOToken = "";
                if (context.Request.Headers != null && context.Request.Headers.Contains("CrowdSSOKey"))
                {
                    SSOToken = context.Request.Headers.GetValues("CrowdSSOKey").FirstOrDefault();
                }
                else
                {
                    context.ErrorResult = new BadRequestResult(context.Request);
                    return;
                }
                if (string.IsNullOrEmpty(SSOToken))
                {
                    context.ErrorResult = new BadRequestResult(context.Request);
                    return;
                }
                SecurityProxy ProxyObj = new SecurityProxy();
                var UserDTO = await ProxyObj.AuthenticateByCrowd_CG(SSOToken);
                if (UserDTO != null)
                {
                    if (string.IsNullOrEmpty(UserDTO.UserName) || string.IsNullOrEmpty(UserDTO.JSessionID))
                    {
                        context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                    return;
                }
            }
            catch (Exception ex)
            {
                context.ErrorResult = new InternalServerErrorResult(context.Request);
                LoggingUtility.LogException(ex);
                throw new Exception("Something went wrong with Authentication. Please contact Connect.");
            }
        }
        //context.ErrorResult = new NotFoundResult(context.Request);
        //context.ErrorResult = new StatusCodeResult(HttpStatusCode.Forbidden, context.Request);
        //context.ErrorResult = new BadRequestResult(context.Request);

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)//uses to update some app level parameters based on succesfull authentication.
        {
            return Task.FromResult(0);
        }
    }
}