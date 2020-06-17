using System;
using System.Collections.Generic;
using System.Text;

namespace Office365UWP
{
    public class AuthenticationProperties
    {
        public string ClientId;
        public string ClientSecret;
        public string TenantId;

        public string[] Scopes;
    }
}
