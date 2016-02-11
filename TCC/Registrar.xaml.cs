using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TCC
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Registrar : Page
    {
        public Registrar()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void login_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login));
        }



        private async void registrar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Uri url = new Uri("http://localhost/api/users/");
            User user = new User();
            user.email = txtEmail.Text;
            user.login = txtLogin.Text;
            user.password = txtPasswd.Password;
            user.active = true;
            try
            {
                var client = new HttpClient();
                CultureInfo ci = new CultureInfo(Windows.System.UserProfile.GlobalizationPreferences.Languages[0]);
                client.DefaultRequestHeaders.Add("Accept-Language", ci.TwoLetterISOLanguageName);
                var uri = new Uri(string.Format("http://192.168.173.1/api/users/", "action", "post", DateTime.Now.Ticks));
                string serialized = JsonConvert.SerializeObject(user);

                StringContent stringContent = new StringContent(
                    serialized,
                    Encoding.UTF8,
                    "application/json");

                var response = client.PostAsync(uri, stringContent);
                HttpResponseMessage x = await response;
                if (x.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageDialog errorbox = new MessageDialog("While puting: http://192.168.173.1/api/users/ we got the following status code: " + x.StatusCode);
                    await errorbox.ShowAsync();

                }
                HttpContent requestContent = x.Content;
                string jsonContent = requestContent.ReadAsStringAsync().Result;
                Debug.WriteLine(jsonContent);
                var retorno = JsonConvert.DeserializeObject<UserResponse>(jsonContent);
                App a = Application.Current as App;
                a.usuarioLogado = retorno;
                MessageDialog msgbox = new MessageDialog("Usuario Cadastrado Com Sucesso");
                await msgbox.ShowAsync();
                Frame.Navigate(typeof(Feed));
            }
                catch(Exception ex)
            {
                MessageDialog errorBox = new MessageDialog(ex.Message);
                await errorBox.ShowAsync();
            }
        }
    }
}
