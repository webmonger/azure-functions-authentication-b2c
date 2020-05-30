using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Options;

namespace AuthExample.AuthTokenBinding
{
    /// <summary>
    /// Provides a new binding instance for the function host.
    /// </summary>
    public class AccessTokenBindingProvider : IBindingProvider
    {
        private readonly IOptions<AuthenticationConfig> authConfig;

        public AccessTokenBindingProvider(IOptions<AuthenticationConfig> authConfig)
        {
            this.authConfig = authConfig;
        }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            IBinding binding = new AccessTokenBinding(authConfig);
            return Task.FromResult(binding);
        }
    }
}