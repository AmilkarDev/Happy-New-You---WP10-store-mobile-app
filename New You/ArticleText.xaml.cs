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

    public sealed partial class ArticleText : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        string mar;
        Article article;
        List<TextArticle> Listt = new List<TextArticle>();
        public ArticleText()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }






        string str;
          private async void parsing(string Url)
        {
              try { 
                  chargement.Visibility = Visibility.Visible;
                    HttpClient http = new System.Net.Http.HttpClient();
                    var reponse = await http.GetByteArrayAsync(Url);
                    str = System.Text.Encoding.GetEncoding("UTF-8").GetString(reponse, 0, reponse.Length - 1);
                    var strdecode = System.Net.WebUtility.HtmlDecode(str);
                    HtmlDocument htmldoc = new HtmlDocument();
                    htmldoc.LoadHtml(strdecode);

                    var l0 = htmldoc.DocumentNode.Descendants("div").Where(x => (x.Attributes.Contains("id") && String.Equals((String)x.GetAttributeValue("id", "null"), "print_dmname"))).ToList();
                    var mal = htmldoc.DocumentNode.Descendants("p").ToList();
            
                      for (int i=4 ; i<9;i++)
                      {
                          mar = mar + mal[i].InnerText ;
                              //+ mal[i].InnerText;
                      }
                    Listt.Add(new TextArticle
                      {  
                          Titre= article.Title ,
                          Text=mar,
                          Writer = mal[9].InnerText 
                      });
                      Post.DataContext = Listt;
                      chargement.Visibility = Visibility.Collapsed;
              }
              catch
              {
                  MessageDialog mesg = new MessageDialog("Please verify your Internet connection");
                  mesg.ShowAsync();
              }
            
            
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
            
            article = e.NavigationParameter as Article;
            string cc = article.Url;
            cc = cc.Substring(0, 4) + "s" + cc.Substring(4);
            parsing(cc);
           

        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void Fbook(object sender, RoutedEventArgs e)
        {
            Article art = article;
            DbHelperArticle dbhelper = new DbHelperArticle();
            dbhelper.Insert(art);
            MessageDialog SuccessMsg = new MessageDialog("Article added sucessfully to your favorites");
            await SuccessMsg.ShowAsync();

        }

        private void ShowBar(object sender, ItemClickEventArgs e)
        {
            BottomBar.Visibility = Visibility.Visible;
        }

        private void Like(object sender, RoutedEventArgs e)
        {

        }

        private void twit(object sender, RoutedEventArgs e)
        {

        }
    }
}
