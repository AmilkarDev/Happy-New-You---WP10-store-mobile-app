using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_You.Models
{
    class DatabaseHelperClass
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
                        dbConn.CreateTable<ImageModel>();
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
        public ObservableCollection<ImageModel> GetImages()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<ImageModel> myCollection = dbConn.Table<ImageModel>().ToList<ImageModel>();
                ObservableCollection<ImageModel> ImagesList = new ObservableCollection<ImageModel>(myCollection);
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
        public void Insert(ImageModel newImage)
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
                var existingImage = dbConn.Query<ImageModel>("select * from Favorites where Id =" + Id).FirstOrDefault();
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
                dbConn.DropTable<ImageModel>();
                dbConn.CreateTable<ImageModel>();
                dbConn.Dispose();
                dbConn.Close();
                //}); 
            }
        } 
    }
}
