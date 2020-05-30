using System.Collections.Generic;

namespace AuthExample.AuthTokenBinding
{
    public class AuthenticationConfig
    {
        public string Audience { get; set; }
        public string ClientID { get; set; }
        public string Authority { get; set; }
        public List<string> ValidIssuers { get; set; }
    }
}