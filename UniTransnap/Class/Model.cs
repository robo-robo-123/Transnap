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

}
