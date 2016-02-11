using System;
using System.Windows;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Net;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Input;
using System.Threading.Tasks;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace TCC
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            ExtendeSplashScreen();
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }  
        public bool CheckUser()
        {
            return false;
        }
        async void ExtendeSplashScreen()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            if (CheckUser())
            {
                Frame.Navigate(typeof(Feed));
            }
            else
            {
                Frame.Navigate(typeof(Login));
            }
        }
    }
}
