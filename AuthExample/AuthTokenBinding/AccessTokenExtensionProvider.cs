using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Options;

namespace AuthExample.AuthTokenBinding
{
    /// <summary>
    /// Wires up the attribute to the custom binding.
    /// </summary>
    public class AccessTokenExtensionProvider : IExtensionConfigProvider
    {
        private readonly IOptions<AuthenticationConfig> authConfig;

        public AccessTokenExtensionProvider(IOptions<AuthenticationConfig> authConfig)
        {
            this.authConfig = authConfig;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            // Creates a rule that links the attribute to the binding
            var provider = new AccessTokenBindingProvider(authConfig);
            var rule = context.AddBindingRule<AccessTokenAttribute>().Bind(provider);
        }
    }
}
