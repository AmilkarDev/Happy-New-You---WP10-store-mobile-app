using New_You.Common;
using New_You.Helpers;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using SQLite;
using New_You.Models;
using System.IO;
using Windows.Storage;
// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace New_You
{
 
    public sealed partial class App : Application
    {
       //public static MobileServiceClient MobileService = new MobileServiceClient( "https://happy-new-you.azurewebsites.net");

        public static string DB_PATH = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "Favorites.sqlite"));
        //public static string DB_PATH1 = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "FavQuotes.sqlite"));
        //public static string DB_PATH2 = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "FavArticles.sqlite"));
        private TransitionCollection transitions;

       
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            if (!CheckFileExists("Favorites.sqlite").Result)
            {
                using (var db = new SQLiteConnection(DB_PATH))
                {
                    db.CreateTable<ImageModel>();
                    db.CreateTable<Template>();
                    db.CreateTable<Article>();
                }
            }
            //if (!CheckFileExists("FavQuotes.sqlite").Result)
            //{
            //    using (var db = new SQLiteConnection(DB_PATH))
            //    {
            //        db.CreateTable<Template>();
            //    }
            //}
            //if (!CheckFileExists("FavArticles.sqlite").Result)
            //{
            //    using (var db = new SQLiteConnection(DB_PATH))
            //    {
            //        db.CreateTable<Article>();
            //    }
            //}  
        }
        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
            }
            return false;
        }
        ContinuationManager _continuator = new ContinuationManager();
        protected async override void OnActivated(IActivatedEventArgs args)
        {
            CreateRootFrame();

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                try
                {
                    await SuspensionManager.RestoreAsync();
                }
                catch { }
            }

            if (args is IContinuationActivatedEventArgs)
                _continuator.ContinueWith(args);

            Window.Current.Activate();
        }  
        private void CreateRootFrame()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
                Window.Current.Content = rootFrame;
            }
        }  
        
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

      
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async   void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
         await SuspensionManager.SaveAsync();  
            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}