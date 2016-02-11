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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TCC
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Feed : Page
    {

        App app = Application.Current as App;
        bool log = false;
        public Feed()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            if (app != null)
            {
                app.FilesOpenPicked += OnFilesOpenPicked;
                app.FileSavePicked += OnFileSavePicked;
            }            
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
                    //var post = (PostsResponse)e.Parameter;
                    //postControl newPost = new postControl();
                    //newPost.imgPost.Source = app.imgTemp;
                    //ImageBrush userBrush = new ImageBrush();
                    //userBrush = userBImg;
                    //newPost.imgUserPost.Fill = userBrush;
                    //newPost.description.Text = post.description;
                    //content.Children.Add(newPost);
                    //Debug.WriteLine("funcionou até aqui");
                    LoadPosts();
                }
            }
            catch { }
        }
        private static readonly IEnumerable<string> SupportedImageFileTypes = new List<string> { ".jpeg", ".jpg", ".png" };
        private WriteableBitmap originalBitmap;
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        private bool _isSettingsOpen = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_isSettingsOpen)
            {
                VisualStateManager.GoToState(this, "ClosingSettings", true);
                _isSettingsOpen = false;
            }
            else
            {
                VisualStateManager.GoToState(this, "OpeningSettings", true);
                _isSettingsOpen = true;
            }
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            if (_isSettingsOpen)
            {
                VisualStateManager.GoToState(this, "ClosingSettings", true);
                _isSettingsOpen = false;
            }
        }
        void gestureRecognizer_CrossSliding(Windows.UI.Input.GestureRecognizer sender, Windows.UI.Input.CrossSlidingEventArgs args)
        {

            Debug.WriteLine("ok");
            if (args.ToString() == "Horizontal") //Left or right
            {
                Debug.WriteLine("Left");
                if (_isSettingsOpen)
                {
                    VisualStateManager.GoToState(this, "OpeningSettings", true);
                    _isSettingsOpen = false;
                }
            }
            else //Left
            {
                // Add code for Left swipe handling
                Debug.WriteLine("Right");
                if (!_isSettingsOpen)
                {
                    VisualStateManager.GoToState(this, "ClosingSettings", true);
                    _isSettingsOpen = true;
                }
            }
        }

        private void onClickPost(object sender, TappedRoutedEventArgs e)
        {
            var fop = new FileOpenPicker();
            foreach (var fileType in SupportedImageFileTypes)
            {
                fop.FileTypeFilter.Add(fileType);
            }
            fop.PickSingleFileAndContinue();
        }
        private async void OnFilesOpenPicked(IReadOnlyList<StorageFile> files)
        {
            // Load picked file
            if (files.Count > 0)
            {
                // Check if image and pick first file to show
                var imageFile = files.FirstOrDefault(f => SupportedImageFileTypes.Contains(f.FileType.ToLower()));
                var imageExt = imageFile.FileType.ToLower();
                if (imageFile != null)
                {
                    // Use WriteableBitmapEx to easily load from a stream
                    using (var stream = await imageFile.OpenReadAsync())
                    {
                        originalBitmap = await new WriteableBitmap(1, 1).FromStream(stream);
                        originalBitmap = originalBitmap.Resize(400,360, WriteableBitmapExtensions.Interpolation.Bilinear);
                        Posts post = new Posts();
                        post.users_id = app.usuarioLogado.id;
                        app.imgTemp = originalBitmap;
                        Frame.Navigate(typeof(Editing),post);
                        //doUpload(pixels);
                    }
                    //postControl newPost = new postControl();
                    //newPost.imgPost.Source = originalBitmap;
                    //ImageBrush userBrush = new ImageBrush();
                    //userBrush = userBImg;
                    //newPost.imgUserPost.Fill = userBrush;
                    //content.Children.Add(newPost);
                    //Debug.WriteLine("funcionou até aqui");

                }
            }
        }
        private async void OnFileSavePicked(StorageFile storageFile)
        {
            using (var stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                await originalBitmap.ToStreamAsJpeg(stream);
            }
        }
        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                frame.GoBack();
                //Indicate the back button press is handled so the app does not exit  
                e.Handled = true;
            }
        }

        /*
        if (_isSettingsOpen)
            {
                VisualStateManager.GoToState(this, "ClosingSettings", true);
                _isSettingsOpen = false;
            }
            var fop = new FileOpenPicker();
            foreach (var fileType in SupportedImageFileTypes)
            {
                fop.FileTypeFilter.Add(fileType);
            }
            fop.PickSingleFileAndContinue();
            */
            

        private void icon_open_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!_isSettingsOpen)
            {
                VisualStateManager.GoToState(this, "OpeningSettings", true);
                _isSettingsOpen = true;
            }
        }

        private void icon_closed_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_isSettingsOpen)
            {
                VisualStateManager.GoToState(this, "ClosingSettings", true);
                _isSettingsOpen = false;
            }
        }

        private void postIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var fop = new FileOpenPicker();
            foreach (var fileType in SupportedImageFileTypes)
            {
                fop.FileTypeFilter.Add(fileType);
            }
            fop.PickSingleFileAndContinue();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App a = Application.Current as App;
            txtUserName.Text = a.usuarioLogado.login;
            if(log == false)
                LoadPosts();
        }
        private void refreshIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            LoadPosts();
        }
        private async void LoadPosts()
        {
            content.Children.Clear();
            var posts = await GetPosts();
            foreach (var post in posts)
            {
                postControl newPost = new postControl();
                byte[] img = Convert.FromBase64String(post.image);
                originalBitmap = new WriteableBitmap(400,360).FromByteArray(img,img.Length);
                newPost.imgPost.Source = originalBitmap;
                ImageBrush userBrush = new ImageBrush();
                userBrush = userBImg;
                newPost.imgUserPost.Fill = userBrush;
                newPost.description.Text = post.description;
                content.Children.Add(newPost);
                Debug.WriteLine("funcionou até aqui");
            }
            log = true;
        }
        public async Task<List<PostsResponse>> GetPosts()
        {
            var client = new HttpClient();
            CultureInfo ci = new CultureInfo(Windows.System.UserProfile.GlobalizationPreferences.Languages[0]);
            client.DefaultRequestHeaders.Add("Accept-Language", ci.TwoLetterISOLanguageName);
            var uri = new Uri(string.Format("http://192.168.173.1/api/posts", "action", "get", DateTime.Now.Ticks));
            var response = client.GetAsync(uri);
            HttpResponseMessage x = await response;
            Debug.WriteLine(x.StatusCode);
            if (x.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //throw new ConnectionOutException("While posting: " + url + " we got the following status code: " + x.StatusCode);
                MessageDialog errorbox = new MessageDialog("While posting: " + uri + " we got the following status code: " + x.StatusCode);
                await errorbox.ShowAsync();

            }
            HttpContent requestContent = x.Content;
            string jsonContent = requestContent.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<PostsResponse>>(jsonContent);
        }

        
    }
}
