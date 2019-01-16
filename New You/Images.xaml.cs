using HtmlAgilityPack;
using New_You.Common;
using New_You.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Net;
using Windows.Storage;
using Windows.Storage.Streams;
using Tweetinvi;

namespace New_You
{
    public sealed partial class Images : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        List<ImageModel> LList = new List<ImageModel>();
        ImageModel mal;
        string ch ;
        const string ConsumerKey = "sMvLHuQkPmmImHVVedBuzqYhB";
        const string ConsumerSecret = "HOcoD4QjEK7391eGBcGuKd0YC6o8ZPlG4OkyypBczOKRLupjTb";
        const string AccessToken = "1479537228-B9FbP6ex5RtZLcJnVW2ltb9zWvuwPkoOHbeOT2s";
        const string AccessTokenSecret = "l6bLuv4N94MK9O6YkZr74uaMLVdX86f02WTbKeULi66NL";
        public Images()
        {
            this.InitializeComponent();
            InitTwitterCredentials();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }
        private static void InitTwitterCredentials()
        {
            var creds = new Tweetinvi.Models.TwitterCredentials(AccessToken, AccessTokenSecret, ConsumerKey, ConsumerSecret);
            Auth.SetUserCredentials(ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret);
            Auth.ApplicationCredentials = creds;
        }
        public async void PublishTweet(string text, string imageUrl)
        {
            var response = await WebRequest.Create(imageUrl).GetResponseAsync();
            var allBytes = new List<byte>();
            using (var imageStream = response.GetResponseStream())
            {
                byte[] buffer = new byte[4000];
                int bytesRead;
                while ((bytesRead = await imageStream.ReadAsync(buffer, 0, 4000)) > 0)
                {
                    allBytes.AddRange(buffer.Take(bytesRead));
                }
            }
            var media = Upload.UploadBinary(allBytes.ToArray());
            Tweet.PublishTweet(text, new Tweetinvi.Parameters.PublishTweetOptionalParameters
            {
                Medias = new List<Tweetinvi.Models.IMedia> { media }
            });
        }
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            try {
            string ctr = e.NavigationParameter as string;
            if (ctr == null) ch = "https://simplereminders.com/all/";
            else ch = ctr;
            parsing(ch);
            
            }
            catch
            {
                MessageDialog mesg = new MessageDialog("Please verify your Internet connection");
                mesg.ShowAsync();
            }
           
        }
        string str;
        private async void parsing(string Url)
        {
            try
            {
           chargement.Visibility = Visibility.Visible;
            HttpClient http = new System.Net.Http.HttpClient();
            var reponse = await http.GetByteArrayAsync(Url);
            String str = System.Text.Encoding.GetEncoding("UTF-8").GetString(reponse, 0, reponse.Length-1 );
            var strdecode = System.Net.WebUtility.HtmlDecode(str);
            HtmlDocument htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(strdecode);
            var l0 = htmldoc.DocumentNode.Descendants("img").Where(x => (x.Attributes.Contains("class") && String.Equals((String)x.GetAttributeValue("class", "null"), "uk-border-rounded"))).ToList();
            var l1 = htmldoc.DocumentNode.Descendants("p").ToList();
            var l2 = htmldoc.DocumentNode.Descendants("h2").ToList();
            int i = 0;
            while (i < 10)
            {
                LList.Add(new ImageModel
                {
                    lien = "http://simplereminders.com/domains/simplereminders.com" + l0[i].GetAttributeValue("src", null).ToString(),
                    //Quote = l1[i].InnerText.ToString(),
                    //Post = l2[i-1].InnerText.ToString()

                });
               
                i++;
            }

            ImageGrid.DataContext = LList;
            chargement.Visibility = Visibility.Collapsed;



             }
            
           catch
            {
                MessageDialog mesg = new MessageDialog("Please verify your Internet connection");
                mesg.ShowAsync();
            }     
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

        private void GoNew(object sender, RoutedEventArgs e)
        {

            Random rand = new Random();
            int a = rand.Next(1, 97);
            string g = a.ToString();
            ch = "https://simplereminders.com/all/p" + g;

            this.Frame.Navigate(typeof(Images), ch);
        }

        private async void ShowBar(object sender, ItemClickEventArgs e)
        {
            BottomBar.Visibility = Visibility.Visible;
             mal = e.ClickedItem as ImageModel;
        }

        private async  void twit(object sender, RoutedEventArgs e)
        {
            String cc = mal.lien;
            PublishTweet("Hello world from Happy New You App", cc);
            MessageDialog SuccessMsg = new MessageDialog("Image posted sucessfully on twitter wall");
            await SuccessMsg.ShowAsync();
        }

        private void Like(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(BasicPage1),mal);
        }

      

       

        private async void Download(object sender, RoutedEventArgs e)
        {
            String urr = mal.lien;


            StorageFolder picsFolder = KnownFolders.PicturesLibrary;
            StorageFile file = await picsFolder.CreateFileAsync("myImage.jpg", CreationCollisionOption.GenerateUniqueName);
            HttpClient client = new HttpClient();

            byte[] responseBytes = await client.GetByteArrayAsync(urr);

            var stream = await file.OpenAsync(FileAccessMode.ReadWrite);

            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                DataWriter writer = new DataWriter(outputStream);
                writer.WriteBytes(responseBytes);
              await   writer.StoreAsync();
              await  outputStream.FlushAsync();
            }
            MessageDialog SuccessMsg = new MessageDialog("Message posted sucessfully on facebook wall");
            await SuccessMsg.ShowAsync();

        }

        //private void Fbook(object sender, RoutedEventArgs e)
        //{

        //}

        private void share(object sender, RoutedEventArgs e)
        {

        }

        private async void Fbook(object sender, RoutedEventArgs e)
        {
            DatabaseHelperClass dbhelper = new DatabaseHelperClass();
            ImageModel mm = mal;
            dbhelper.Insert(mm);
            MessageDialog SuccessMsg = new MessageDialog("Image added sucessfully to your favorites");
            await SuccessMsg.ShowAsync();
        }

       
    }
}
