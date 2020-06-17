using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Office365UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Execute();
        }

        async void Execute()
        {
            AuthenticationHelper helper = new AuthenticationHelper();

            helper.Authentication.ClientId = "8924f069-832c-40c0-bcb2-95f9ab328bd7";
            helper.Authentication.Scopes = new string[] { "User.Read", "User.Read.All" };
            helper.RedirectUri = "https://www.appzinside.com";

            string token = await helper.GetTokenForUserAsync();
        }
    }
}
