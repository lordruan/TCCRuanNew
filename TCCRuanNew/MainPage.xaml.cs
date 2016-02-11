using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Diagnostics;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using Windows.Storage.Pickers;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using System.IO;
using Windows.UI.Xaml.Media.Imaging;

namespace TCCRuan
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        private static readonly IEnumerable<string> SupportedImageFileTypes = new List<string> { ".jpeg", ".jpg", ".png" };

        private WriteableBitmap originalBitmap;
        public MainPage()
        {
            InitializeComponent();

            // Attach events which will return the picked file for loading and saving
            var app = Application.Current as App;
            if (app != null)
            {
                app.FilesOpenPicked += OnFilesOpenPicked;
                //app.FileSavePicked += OnFileSavePicked;
            }
        }

        private void onClickPost(object sender, System.Windows.Input.GestureEventArgs e)
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
                if (imageFile != null)
                {
                    // Use WriteableBitmapEx to easily load from a stream
                    // Use WriteableBitmapEx to easily load from a stream
                    using (var stream = await imageFile.OpenReadAsync())
                    {
                        originalBitmap = await new WriteableBitmap(1, 1).FromStream(stream);
                    }
                    postControl newPost = new postControl();
                    Debug.WriteLine("funcionou até aqui2");
                    System.Windows.Media.ImageBrush postBrush = new System.Windows.Media.ImageBrush();
                    postBrush.ImageSource = originalBitmap;
                    newPost.imagePost.Background = postBrush;
                    System.Windows.Media.ImageBrush userBrush = new System.Windows.Media.ImageBrush();
                    userBrush.ImageSource = new BitmapImage(new Uri("/Assets/photo.jpg"));
                    newPost.imgUserPost.Fill = userBrush;
                    content.Children.Add(newPost);
                    Debug.WriteLine("funcionou até aqui");

                    /*var mediaCapture = new MediaCapture();
                    var jpgProperties = ImageEncodingProperties.CreateJpeg();
                    jpgProperties.Width = 380;
                    jpgProperties.Height = 480;
                    var stream = await imageFile.OpenReadAsync();
                    //await mediaCapture.CapturePhotoToStreamAsync(jpgProperties, stream);
                    using (var str = new InMemoryRandomAccessStream())
                    {
                        await mediaCapture.CapturePhotoToStreamAsync(jpgProperties, str);

                        str.Seek(0);
                        using (var ioStream = str.AsStream())
                        {
                            var originalBitmap = new WriteableBitmap(1, 1);
                            originalBitmap.FromStream(ioStream);
                        }
                    }
                    postControl newPost = new postControl();
                    Debug.WriteLine("funcionou até aqui2");
                    System.Windows.Media.ImageBrush postBrush = new System.Windows.Media.ImageBrush();
                    postBrush.ImageSource = originalBitmap;
                    newPost.imagePost.Background = postBrush;
                    System.Windows.Media.ImageBrush userBrush = new System.Windows.Media.ImageBrush();
                    userBrush.ImageSource = new BitmapImage(new Uri("/Assets/photo.jpg"));
                    newPost.imgUserPost.Fill = userBrush;
                    content.Children.Add(newPost);
                    Debug.WriteLine("funcionou até aqui");*/
                }
            }
        }
        /*void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {

            Debug.WriteLine("funcionou image");
            if (e.TaskResult == TaskResult.OK)
            {
                //Code to display the photo on the page in an image control named myImage.
                //System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                //bmp.SetSource(e.ChosenPhoto);
                //myImage.Source = bmp;
                postControl newPost = new postControl();
                Debug.WriteLine("funcionou até aqui2");
                System.Windows.Media.ImageBrush postBrush = new System.Windows.Media.ImageBrush();
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                postBrush.ImageSource = bmp;
                newPost.imagePost.Background = postBrush;
                System.Windows.Media.ImageBrush userBrush = new System.Windows.Media.ImageBrush();
                userBrush.ImageSource = new BitmapImage(new Uri("/Assets/photo.jpg"));
                newPost.imgUserPost.Fill = userBrush;
                content.Children.Add(newPost);
                Debug.WriteLine("funcionou até aqui");
            }
        }
        public async Task CapturePhoto()
        {
            var imgEncodingProprietes = ImageEncodingProperties.CreateJpeg();
            imgEncodingProprietes.Width = 380;
            imgEncodingProprietes.Height = 200;

        }*/
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
        private void GestureListener_Flick(object sender, FlickGestureEventArgs e)
        {

        if (e.Direction.ToString() == "Horizontal") //Left or right
        {
            if (e.HorizontalVelocity > 0) // Right
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
        }
    }
}