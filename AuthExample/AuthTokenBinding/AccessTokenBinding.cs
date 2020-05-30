using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.Options;

namespace AuthExample.AuthTokenBinding
{
    /// <summary>
    /// Runs on every request and passes the function context (e.g. Http request and host configuration) to a value provider.
    /// </summary>
    public class AccessTokenBinding : IBinding
    {
        private readonly IOptions<AuthenticationConfig> authConfig;

        public AccessTokenBinding(IOptions<AuthenticationConfig> authConfig)
        {
            this.authConfig = authConfig;
        }

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            // Get the HTTP request
            var request = context.BindingData["req"] as HttpRequest;

            return Task.FromResult<IValueProvider>(new AccessTokenValueProvider(request, authConfig));
        }

        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) => null;

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();
    }
}
