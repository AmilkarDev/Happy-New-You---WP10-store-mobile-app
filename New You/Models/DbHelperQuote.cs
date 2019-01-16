using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_You.Models
{
    class DbHelperQuote
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
                        dbConn.CreateTable<Template>();
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
        public ObservableCollection<Template> GetQuotes()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Template> myCollection = dbConn.Table<Template>().ToList<Template>();
                ObservableCollection<Template> ImagesList = new ObservableCollection<Template>(myCollection);
                return ImagesList;
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
        public void Insert(Template newImage)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newImage);
                });
            }
        }

        //Delete specific contact 
        public void DeleteContact(int Id)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingImage = dbConn.Query<Template>("select * from FavQuotes where Id =" + Id).FirstOrDefault();
                if (existingImage != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingImage);
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
                dbConn.DropTable<Template>();
                dbConn.CreateTable<Template>();
                dbConn.Dispose();
                dbConn.Close();
                //}); 
            }
        } 
    }
}
