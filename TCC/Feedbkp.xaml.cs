using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
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
        public Feed()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            var app = Application.Current as App;
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
                        BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                        Stream pixelStream = originalBitmap.PixelBuffer.AsStream();
                        byte[] pixels = new byte[pixelStream.Length];
                        //doUpload(pixels);
                    }
                    postControl newPost = new postControl();
                    newPost.imgPost.Source = originalBitmap;
                    ImageBrush userBrush = new ImageBrush();
                    userBrush = userBImg;
                    newPost.imgUserPost.Fill = userBrush;
                    content.Children.Add(newPost);
                    Debug.WriteLine("funcionou até aqui");

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

        private void Objeto_Inteligente_de_Vetor_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_isSettingsOpen)
            {
                VisualStateManager.GoToState(this, "ClosingSettings", true);
                _isSettingsOpen = false;
            }
        }

        private void Border_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var fop = new FileOpenPicker();
            foreach (var fileType in SupportedImageFileTypes)
            {
                fop.FileTypeFilter.Add(fileType);
            }
            fop.PickSingleFileAndContinue();
        }
    }
}
