using Microsoft.EntityFrameworkCore;
using Projekt.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models
{
    public class DBcontext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<WatchedElement> watchedElements { get; set; }
        public DbSet<Watchlist> watchlists { get; set; }
        public DbSet<OHLC> oHLCs { get; set; }

        public DBcontext(DbContextOptions<DBcontext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new WatchedElementConfiguration());
            modelBuilder.ApplyConfiguration(new WatchedListConfiguration());
            modelBuilder.ApplyConfiguration(new OHLCConfiguration());
        }
    }
}
