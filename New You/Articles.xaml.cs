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

namespace New_You
{
   
    public sealed partial class Articles : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        List<Article> ArticleList = new List<Article>();
        string ch = "https://greatday.com/nmot/previous.html";
        public Articles()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

           
        }
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        private  void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            //try
            //{
                string ch1 = e.NavigationParameter as string;
                if (ch1 != null) ch = ch1;

                parsing(ch);
            //}
            //catch
            //{
            //    MessageDialog mesg = new MessageDialog("Please verify your Internet connection");
            //    mesg.ShowAsync();
            //}
            
        }


        string str;







       

        private async void parsing(string Url)
        {
          
            try
            {
                chargement.Visibility = Visibility.Visible;
                HttpClient http = new System.Net.Http.HttpClient();
                var reponse = await http.GetByteArrayAsync(new Uri(Url));
                str = System.Text.Encoding.GetEncoding("UTF-8").GetString(reponse, 0, reponse.Length - 1).ToString() ;
                var strdecode = System.Net.WebUtility.HtmlDecode(str);
                HtmlDocument htmldoc = new HtmlDocument();
                htmldoc.LoadHtml(strdecode);
                var kk = htmldoc.DocumentNode.Descendants("a").ToList();
                for (int i = 84; i < 297; i++)
                {
                    ArticleList.Add(new Article
                    {
                        Title = kk[i].InnerText,
                        Url = kk[i].GetAttributeValue("href", null)
                    });

                }
                Titles.DataContext = ArticleList;
                chargement.Visibility = Visibility.Collapsed;
            }
            catch
            {
                MessageDialog mesg = new MessageDialog("Please verify your Internet connection");
                mesg.ShowAsync();
            }
            
            int j= 0;
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

        private void GoPost(object sender, ItemClickEventArgs e)
        {
            Article ar = e.ClickedItem as Article;
            this.Frame.Navigate(typeof(ArticleText), ar);
        }
        

        //private async  void Go2015(object sender, RoutedEventArgs e)
        //{
        //    ArticleList.Clear();
        //   this.Frame.Navigate(typeof(Articles), "http://greatday.com/nmot/previous.html?y=2015");
            
        //}
    }
}
