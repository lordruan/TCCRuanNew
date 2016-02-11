using System;
using System.Collections.Generic;
using System.Net.Http;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Newtonsoft.Json;
using System.Globalization;
using System.Threading.Tasks;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TCC
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }
        
        private void Border_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Logar();
        }

        private async void Logar()
        {
            var users = await GetUsers();
            var flag = false;
            foreach (var user in users)
            {
                if (string.Compare(txtLogin.Text.ToString(), user.login) == 0)
                {
                    if (user.password.Equals(txtPasswd.Password.ToString()))
                    {
                        App a = Application.Current as App;
                        a.usuarioLogado = user;
                        Frame.Navigate(typeof(Feed));
                        flag = true;
                    }
                }
            }
            if (!flag)
            {
                MessageDialog msgbox = new MessageDialog("Tente novamente");
                await msgbox.ShowAsync();
            }
        }

        private void forgOt_password__Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Feed));
        }

        private void Still_don_t_have_an_account__Register_now__Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Registrar));
        }

        public async Task<List<UserResponse>> GetUsers()
        {
            var client = new HttpClient();
            CultureInfo ci = new CultureInfo(Windows.System.UserProfile.GlobalizationPreferences.Languages[0]);
            client.DefaultRequestHeaders.Add("Accept-Language", ci.TwoLetterISOLanguageName);
            var uri = new Uri(string.Format("http://192.168.173.1/api/users", "action", "get", DateTime.Now.Ticks));

            var response = client.GetAsync(uri);

            HttpResponseMessage x = await response;
            Debug.WriteLine(x.StatusCode);
            if (x.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //throw new ConnectionOutException("While posting: " + url + " we got the following status code: " + x.StatusCode);
                MessageDialog errorbox = new MessageDialog("While posting: "+ uri+" we got the following status code: " + x.StatusCode);
                await errorbox.ShowAsync();

            }
            HttpContent requestContent = x.Content;
            string jsonContent = requestContent.ReadAsStringAsync().Result;
            //MessageDialog msgbox = new MessageDialog(jsonContent);
            //await msgbox.ShowAsync();
            return JsonConvert.DeserializeObject<List<UserResponse>>(jsonContent);
        }

        private async void PsdLayer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var users = await GetUsers();
        }

        private void txtLogin_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Focus(FocusState.Keyboard);
            }
        }

        private void txtPasswd_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                LoadingIndicator.Visibility = Visibility.Visible;
                Logar();
                LoadingIndicator.Visibility = Visibility.Collapsed;
            }
        }
    }
}
