using New_You.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace New_You
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
       
        private async void GoQuote(object sender, RoutedEventArgs e)
        {
           // TodoItem item = new TodoItem
           // {
           //     Text = "Awesome item",
           //     Complete = false
           // };
           // await App.MobileService.GetTable<TodoItem>().InsertAsync(item);
           //// List<TodoItem> items; 
           // List<TodoItem> itemss = new List<TodoItem>();
           //itemss = await App.MobileService.GetTable<TodoItem>().ToListAsync();
            this.Frame.Navigate(typeof(Quotes));
        }

        private void GoImage(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Images), null);
        }

        private void GoArticle(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Articles), null);
        }

        private void GoFavorites(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Favorites));
        }
    }
}
