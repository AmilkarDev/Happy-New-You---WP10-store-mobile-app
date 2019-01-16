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
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace New_You
{
    public sealed partial class Quotes : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
       private  Template quote;
        List<Template> Temp1 = new List<Template>();
        public Quotes()
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

    
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            parsing("https://simplereminders.com/all/quotes/");
          
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



        string str;
        private async void parsing(string Url)
        { 
            
            //try {
            chargement.Visibility = Visibility.Visible;
            HttpClient http = new System.Net.Http.HttpClient();
            var reponse = await http.GetByteArrayAsync(Url);
            str = System.Text.Encoding.GetEncoding("UTF-8").GetString(reponse, 0, reponse.Length-1 );
            var strdecode = System.Net.WebUtility.HtmlDecode(str);
            HtmlDocument htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(strdecode);
            var AutorList = htmldoc.DocumentNode.Descendants("p").Where(x => (x.Attributes.Contains("class") && String.Equals((String)x.GetAttributeValue("class", "null"), "uk-text-right"))).ToList();
            var QuoteList = htmldoc.DocumentNode.Descendants("h3").ToList();
            //int n = b.Count
            for (int i =0 ; i<AutorList.Count() ;  i++)
            {
                Temp1.Add(new Template
                {
                    Author=AutorList[i].InnerText.ToString(),
                    Text =QuoteList[i].InnerText.ToString()

                });
            }
            Qquote.DataContext = Temp1;
            chargement.Visibility = Visibility.Collapsed;
            if (Temp1.Count == 0)
            {
                MessageDialog mesg = new MessageDialog("Please verify your Internet connection");
                mesg.ShowAsync();
            }

            //}
            //catch
            //{
            //    MessageDialog mesg = new MessageDialog("Please verify your Internet connection");
            //    mesg.ShowAsync();
            //}
        }
        #endregion

        private async void Fbook(object sender, RoutedEventArgs e)
        {
            DbHelperQuote dbhelper = new DbHelperQuote();
            Template mm = quote;
            dbhelper.Insert(mm);
            MessageDialog SuccessMsg = new MessageDialog("Quote added sucessfully to your favorites");
            await SuccessMsg.ShowAsync();
        }

        private void Like(object sender, RoutedEventArgs e)
        {

        }

        private void twit(object sender, RoutedEventArgs e)
        {

        }

        private void ShowBar(object sender, ItemClickEventArgs e)
        {
            BottomBar.Visibility = Visibility.Visible;
            quote = e.ClickedItem as Template;
        }
    }
}
