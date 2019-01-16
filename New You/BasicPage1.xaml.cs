using Facebook;
using New_You.Common;
using New_You.Helpers;
using New_You.Models;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
namespace New_You
{
    public sealed partial class BasicPage1 : Page,IWebAuthenticationBrokerContinuable 
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        ImageModel im;
        public BasicPage1()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        FacebookHelper ObjFBHelper = new FacebookHelper();
        FacebookClient fbclient = new FacebookClient();

        private void BtnFaceBookLogin_Click(object sender, RoutedEventArgs e)
        {
            ObjFBHelper.LoginAndContinue();
        }



        private async void BtnFaceBookPost_Click(object sender, RoutedEventArgs e)
        {
            var postParams = new
            {
                name = "Facebook Post from Happy New You wp App",
                caption = "Posted from Happy New You App",
                link = im.lien,
             //   link = "https://simplereminders.com/domains/simplereminders.com/uploads/images/blog/robert-brault-friends-adjust.jpg",
                description = TxtStatusMsg.Text,
                picture = im.lien,
              //  picture = "https://simplereminders.com/domains/simplereminders.com/uploads/images/blog/robert-brault-friends-adjust.jpg"
            };
            try
            {
                dynamic fbPostTaskResult = await fbclient.PostTaskAsync("/me/feed", postParams);
                var responseresult = (IDictionary<string, object>)fbPostTaskResult;
                MessageDialog SuccessMsg = new MessageDialog("Message posted sucessfully on facebook wall");
                await SuccessMsg.ShowAsync();
            }
            catch (Exception ex)
            {
                //MessageDialog ErrMsg = new MessageDialog("Error Ocuured!");  

            }
        }


        private  void BtnFaceBookLogout_Click(object sender, RoutedEventArgs e)
        {
            _logoutUrl = fbclient.GetLogoutUrl(new
            {
                next = "https://www.facebook.com/connect/login_success.html",
                access_token = ObjFBHelper.AccessToken
            });
            WebAuthenticationBroker.AuthenticateAndContinue(_logoutUrl);
            BtnLogin.Visibility = Visibility.Visible;
            BtnLogout.Visibility = Visibility.Collapsed;
        }

        public async void ContinueWithWebAuthenticationBroker(WebAuthenticationBrokerContinuationEventArgs args)
        {
            ObjFBHelper.ContinueAuthentication(args);
            if (ObjFBHelper.AccessToken != null)
            {
                fbclient = new Facebook.FacebookClient(ObjFBHelper.AccessToken);

                //Fetch facebook UserProfile:  
                dynamic result = await fbclient.GetTaskAsync("me");
                string id = result.id;
                string email = result.email;
                string FBName = result.name;

                //Format UserProfile:  
                GetUserProfilePicture(id);
                TxtUserProfile.Text = FBName;
                StckPnlProfile_Layout.Visibility = Visibility.Visible;
                BtnLogin.Visibility = Visibility.Collapsed;
                BtnLogout.Visibility = Visibility.Visible;
            }
            else
            {
                StckPnlProfile_Layout.Visibility = Visibility.Collapsed;
            }

        }

        private void GetUserProfilePicture(string UserID)
        {
            string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", UserID, "square", ObjFBHelper.AccessToken);
            picProfile.Source = new BitmapImage(new Uri(profilePictureUrl));
        }






        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
            
        }

       
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            im = e.NavigationParameter as ImageModel ;
        }

      
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

     
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        public Uri _logoutUrl { get; set; }
    }
}
