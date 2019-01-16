using New_You.Common;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using New_You.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace New_You
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PrefArticles : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public PrefArticles()
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
            DbHelperArticle dbdb = new DbHelperArticle();
          ObservableCollection<Article> articlesList = dbdb.GetArticles();
          Titles.DataContext = articlesList;
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

        private void GoPost(object sender, ItemClickEventArgs e)
        {
            Article ar = e.ClickedItem as Article;
            this.Frame.Navigate(typeof(PreArticleText), ar);
        }
    }
}
