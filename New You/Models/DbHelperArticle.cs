using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using New_You.Models;
using System.Collections.ObjectModel;
namespace New_You.Models
{
    class DbHelperArticle
    {
        SQLiteConnection dbConn;

        //Create Tabble 
        public async Task<bool> onCreate(string DB_PATH)
        {
            try
            {
                if (!CheckFileExists(DB_PATH).Result)
                {
                    using (dbConn = new SQLiteConnection(DB_PATH))
                    {
                        dbConn.CreateTable<Article>();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
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
                return false;
            }
        }

        //// Retrieve the specific contact from the database. 
        //public Contacts ReadContact(int contactid)
        //{
        //    using (var dbConn = new SQLiteConnection(App.DB_PATH))
        //    {
        //        var existingconact = dbConn.Query<Contacts>("select * from Contacts where Id =" + contactid).FirstOrDefault();
        //        return existingconact;
        //    }
        //}
        // Retrieve the all contact list from the database. 
        public ObservableCollection<Article> GetArticles()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Article> myCollection = dbConn.Table<Article>().ToList<Article>();
                ObservableCollection<Article> ArticlesList = new ObservableCollection<Article>(myCollection);
                return ArticlesList;
            }
        }

        ////Update existing conatct 
        //public void UpdateContact(Contacts contact)
        //{
        //    using (var dbConn = new SQLiteConnection(App.DB_PATH))
        //    {
        //        var existingconact = dbConn.Query<Contacts>("select * from Contacts where Id =" + contact.Id).FirstOrDefault();
        //        if (existingconact != null)
        //        {
        //            existingconact.Name = contact.Name;
        //            existingconact.PhoneNumber = contact.PhoneNumber;
        //            existingconact.CreationDate = contact.CreationDate;
        //            dbConn.RunInTransaction(() =>
        //            {
        //                dbConn.Update(existingconact);
        //            });
        //        }
        //    }
        //}
        // Insert the new contact in the Contacts table. 
        public void Insert(Article newArticle)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newArticle);
                });
            }
        }

        //Delete specific contact 
        public void DeleteContact(int Id)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingArticle = dbConn.Query<ImageModel>("select * from FavArticles where Id =" + Id).FirstOrDefault();
                if (existingArticle != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingArticle);
                    });
                }
            }
        }
        //Delete all contactlist or delete Contacts table 
        public void DeleteAllContact()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() => 
                //   { 
                dbConn.DropTable<Article>();
                dbConn.CreateTable<Article>();
                dbConn.Dispose();
                dbConn.Close();
                //}); 
            }
        } 

    }
}
