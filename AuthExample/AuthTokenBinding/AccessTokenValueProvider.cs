using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace AuthExample.AuthTokenBinding
{
    /// <summary>
    /// Creates a <see cref="ClaimsPrincipal"/> instance for the supplied header and configuration values.
    /// </summary>
    /// <remarks>
    /// This is where the actual authentication happens - replace this code to implement a different authentication solution.
    /// </remarks>
    public class AccessTokenValueProvider : IValueProvider
    {
        private const string AUTH_HEADER_NAME = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";
        private HttpRequest request;


        private readonly TokenValidationParameters parameters;
        private readonly ConfigurationManager<OpenIdConnectConfiguration> manager;
        private readonly JwtSecurityTokenHandler handler;
        private readonly IOptions<AuthenticationConfig> authConfig;

        public AccessTokenValueProvider(HttpRequest request, IOptions<AuthenticationConfig> authConfig)
        {
            this.request = request;
            this.authConfig = authConfig;
        }

        public async Task<object> GetValueAsync()
        {
            try
            {
                // Get the token from the header
                if (request.Headers.ContainsKey(AUTH_HEADER_NAME) &&
                   request.Headers[AUTH_HEADER_NAME].ToString().StartsWith(BEARER_PREFIX))
                {
                    var accessToken = request.Headers["Authorization"].ToString().Substring(BEARER_PREFIX.Length);

                    // Debugging purposes only, set this to false for production
                    Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

                    ConfigurationManager<OpenIdConnectConfiguration> configManager =
                        new ConfigurationManager<OpenIdConnectConfiguration>(
                            $"{authConfig.Value.Authority}/.well-known/openid-configuration",
                            new OpenIdConnectConfigurationRetriever());

                    OpenIdConnectConfiguration config = null;
                    config = await configManager.GetConfigurationAsync();

                    ISecurityTokenValidator tokenValidator = new JwtSecurityTokenHandler();

                    // Initialize the token validation parameters
                    TokenValidationParameters validationParameters = new TokenValidationParameters
                    {
                        // App Id URI and AppId of this service application are both valid audiences.
                        ValidAudiences = new[] { authConfig.Value.Audience, authConfig.Value.ClientID },

                        // Support Azure AD V1 and V2 endpoints.
                        ValidIssuers = authConfig.Value.ValidIssuers,
                        IssuerSigningKeys = config.SigningKeys
                    };

                    var claimsPrincipal = tokenValidator.ValidateToken(accessToken, validationParameters, out SecurityToken securityToken);
                    return AccessTokenResult.Success(claimsPrincipal, securityToken);
                }
                else
                {
                    return AccessTokenResult.NoToken();
                }
            }
            catch (SecurityTokenExpiredException)
            {
                return AccessTokenResult.Expired();
            }
            catch (Exception ex)
            {
                return AccessTokenResult.Error(ex);
            }
        }

        public Type Type => typeof(ClaimsPrincipal);

        public string ToInvokeString() => string.Empty;
    }
}
