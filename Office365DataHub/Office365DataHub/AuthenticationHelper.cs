using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

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

        public ServiceException ServiceException = new ServiceException();

        public string Token
        {
            get { return token; }

        }

        private GraphServiceClient graphClient = null;

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

        public GraphServiceClient GetAuthenticatedClient()
        {
            if (graphClient == null)
            {
                // Create Microsoft Graph client.
                try
                {
                    graphClient = new GraphServiceClient(
                        UseBetaAPI ? "https://graph.microsoft.com/beta" : "https://graph.microsoft.com/v1.0",
                        new DelegateAuthenticationProvider(
                            async (requestMessage) =>
                            {
                                token = UserLogon ? await GetTokenForUserAsync() : await GetTokenForAppAsync();

                                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);

                            }));
                    return graphClient;
                }

                catch (Exception ex)
                {
                    ServiceException.Error = ServiceError.AuthenticationClientError;
                    ServiceException.Exception = ex;
                }
            }

            return graphClient;
        }

        public async Task<string> GetTokenForAppAsync()
        {
            string token = "";

            string url = string.Format("https://login.microsoftonline.com/{0}/oauth2/token", Authentication.TenantId);

            string body = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&resource=https%3A%2F%2Fgraph.microsoft.com", Authentication.ClientId, Authentication.ClientSecret);

            using (HttpClient client = new HttpClient())
            {
                var buffer = System.Text.Encoding.UTF8.GetBytes(body);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage result = await client.PostAsync(url, byteContent);

                string content = await result.Content.ReadAsStringAsync();

                TokenResult tokenResult = JsonConvert.DeserializeObject<TokenResult>(content);

                token = tokenResult.access_token;
            }

            return token;
        }

        /// <summary>
        /// Get Token for User.
        /// </summary>
        /// <returns>Token for user.</returns>
        public async Task<string> GetTokenForUserAsync()
        {
            // the following code is the new way of authenticating and getting a token for Graph API
            // However this requires version 3.0.8 of Microsoft.Identity.Client nuget package. This
            // package incorporates the System.Runtime.Serialization.Formatters which conflicts with a 
            // version inside the WinRTLegacy reference. WinRTLegacy is included automatically 
            /// in the resharp project by Unity for legacy calls from their own system. 
            /// We need to see if this problem resolves with later versions of Unity. Currently using
            /// Unity 2017.4.26f1

            var app = PublicClientApplicationBuilder.Create(Authentication.ClientId).WithRedirectUri(RedirectUri).Build();
            //var app = PublicClientApplicationBuilder.Create(Authentication.ClientId).Build();

            var accounts = await app.GetAccountsAsync();
            AuthenticationResult result;
            try
            {
                result = await app.AcquireTokenSilent(Authentication.Scopes, accounts.FirstOrDefault()).ExecuteAsync();
                TokenForUser = result.AccessToken;
            }
            catch (MsalUiRequiredException)
            {
                result = await app.AcquireTokenInteractive(Authentication.Scopes).ExecuteAsync();
                TokenForUser = result.AccessToken;
            }

            return TokenForUser;
        }
    }
}
