using Administrator1._0.DBEntities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator1._0
{
    public class Db : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<RoomStatus> RoomStatuses { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // 
        {
            var sb = new SqlConnectionStringBuilder();
            sb.DataSource = File.ReadAllText("sqlserverMax.txt");
            //@"COMP1-CAB1\SQLEXPRESS";
            //DESKTOP-USEKBLE
            //COMP1-CAB1\SQLEXPRESS
            sb.InitialCatalog = "administrator18";
            sb.IntegratedSecurity = true;
            optionsBuilder.UseSqlServer(sb.ToString());
            base.OnConfiguring(optionsBuilder);
        }

        private Db()
        {
            Database.EnsureCreated();
        }

        static Db db;
        public static Db GetDb()
        {
            if (db == null)
                db = new Db();
            return db;
        }

    }
}
