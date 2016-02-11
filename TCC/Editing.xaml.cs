using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TCC
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Editing : Page
    {
        App app = Application.Current as App;
        private WriteableBitmap originalBitmap;
        private Posts post;
        public Editing()
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
            try
            {
                if (!e.Parameter.Equals(null))   // I've also used if(data != null) which hasn't worked either
                {
                        post = (Posts)e.Parameter;
                        imgPreview.Source = app.imgTemp;
                }
            }
            catch { }
        }
        private void okIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Postar();
        }

        public async void Postar()
        {
            post.description = txtDescricao.Text;
            post.active = true;
            //post.image = Convert.ToBase64String(ConvertBitmapToByteArray(app.imgTemp));
            post.image = Convert.ToBase64String(ConvertBitmapToByteArray(app.imgTemp));
            try
            {
                var client = new HttpClient();
                CultureInfo ci = new CultureInfo(Windows.System.UserProfile.GlobalizationPreferences.Languages[0]);
                client.DefaultRequestHeaders.Add("Accept-Language", ci.TwoLetterISOLanguageName);
                var uri = new Uri(string.Format("http://192.168.173.1/api/posts/", "action", "post", DateTime.Now.Ticks));
                string serialized = JsonConvert.SerializeObject(post);

                StringContent stringContent = new StringContent(
                    serialized,
                    Encoding.UTF8,
                    "application/json");

                var response = client.PostAsync(uri, stringContent);
                HttpResponseMessage x = await response;
                if (x.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    MessageDialog errorbox = new MessageDialog("While puting: http://192.168.173.1/api/posts/ we got the following status code: " + x.StatusCode);
                    await errorbox.ShowAsync();

                }
                HttpContent requestContent = x.Content;
                string jsonContent = requestContent.ReadAsStringAsync().Result;
                var retorno = JsonConvert.DeserializeObject<PostsResponse>(jsonContent);
                MessageDialog msgbox = new MessageDialog("Post Completo");
                await msgbox.ShowAsync();
                Frame.Navigate(typeof(Feed), retorno);
            }
            catch (Exception ex)
            {
                MessageDialog errorBox = new MessageDialog(ex.Message);
                await errorBox.ShowAsync();
            }
        }
        byte[] ConvertBitmapToByteArray(WriteableBitmap bitmap)
        {
            using (Stream stream = bitmap.PixelBuffer.AsStream())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
        }

        private void txtDescricao_KeyUp(object sender, KeyRoutedEventArgs e)
        {

        }

        private void Objeto_Inteligente_de_Vetor_Copy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Feed));
        }
    }
}
