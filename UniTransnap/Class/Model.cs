using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UniTransnap.Class
{


        public class HistoryContext : DbContext
        {
        public DbSet<History> Historys { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Data Source=historys.db");
            }
        }

    public class History
    {
        public int Id { get; set; }

        public string before_langage { get; set; }
        public string after_langage { get; set; }
        public string before_word { get; set; }
        public string after_word { get; set; }
    }





    /// <summary>
    /// 定義したクラス
    /// </summary>
    /*
        [DataContract]
        public class History_old
        {
          [PrimaryKey, AutoIncrement]
          public int Id { get; set; }

          [DataMember]
          public string before_langage { get; set; }
          [DataMember]
          public string after_langage { get; set; }
          [DataMember]
          public string before_word { get; set; }
          [DataMember]
          public string after_word { get; set; }
        }
        */


}
