using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using AuthExample.AuthTokenBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using System.Web.Http;
using System.Net.Http;
using System.Net;

namespace AuthExample
{
    public class FunctionExample
    {

        [FunctionName(nameof(GetExample))]
        public async Task<IActionResult> GetExample(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "authenticated")] HttpRequest req,
            [AccessToken] AccessTokenResult accessTokenResult,
            ILogger log)
        {
            if (accessTokenResult.Status != AccessTokenStatus.Valid)
            {
                return AuthenticationRedirect.ReturnFailedResult(accessTokenResult.Status);
            }

            var id = accessTokenResult.Principal.Claims.Where(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").FirstOrDefault().Value;

            return new NoContentResult();
        }
    }
}
