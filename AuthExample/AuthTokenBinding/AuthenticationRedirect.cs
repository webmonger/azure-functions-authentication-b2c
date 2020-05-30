using AuthExample.AuthTokenBinding;
using Microsoft.AspNetCore.Mvc;

namespace AuthExample.AuthTokenBinding
{
    public static class AuthenticationRedirect
    {
        public static ActionResult ReturnFailedResult(AccessTokenStatus status)
        {
            switch (status)
            {
                case AccessTokenStatus.NoToken:
                    return (ActionResult)new BadRequestResult();
                case AccessTokenStatus.Expired:
                    return (ActionResult)new UnauthorizedResult();
                case AccessTokenStatus.Error:
                    return (ActionResult)new BadRequestResult();
                default:
                    return (ActionResult)new NotFoundResult();
            }
        }
    }
}