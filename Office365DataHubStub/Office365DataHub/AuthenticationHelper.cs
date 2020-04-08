using System;

namespace Office365DataHub
{
    public class TokenResult
    {
        public string access_token = "";
    }

    public class Authentication
    {
        public string ClientId;
        public string ClientSecret;
        public string TenantId;

        public string[] Scopes;
    }

    public class AuthenticationHelper
    {
        private static AuthenticationHelper instance = null;
        private string token = "";

        public string TokenForUser = null;
        public DateTimeOffset Expiration;

        public Authentication Authentication { get; set; }
        public bool UseBetaAPI { get; set; }

        public bool UserLogon { get; set; }

        public string RedirectUri { get; set; }

        public string Token
        {
            get { return token; }

        }

        private AuthenticationHelper()
        {
            Authentication = new Authentication();
            UseBetaAPI = false;
        }

        public static AuthenticationHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AuthenticationHelper();
                }

                return instance;
            }
        }
    }
}
