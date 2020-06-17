using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Office365UWP
{

    public class AuthenticationHelper
    {
        public string TokenForUser = null;

        public AuthenticationProperties Authentication { get; set; }

        public string RedirectUri { get; set; }

        public AuthenticationHelper()
        {
            Authentication = new AuthenticationProperties();
        }

        /// <summary>
        /// Get Token for User.
        /// </summary>
        /// <returns>Token for user.</returns>
        public async Task<string> GetTokenForUserAsync()
        {
            var app = PublicClientApplicationBuilder.Create(Authentication.ClientId).WithRedirectUri(RedirectUri).Build();

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
