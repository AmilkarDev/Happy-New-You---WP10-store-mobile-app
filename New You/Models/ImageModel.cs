using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace New_You.Models
{
   public class ImageModel
    {
       [SQLite.PrimaryKey, SQLite.AutoIncrement]
       public int Id { get; set; }
       public string lien { get; set; }
       public string Post { get; set; }
       public string Quote { get; set; } 
    }
}
