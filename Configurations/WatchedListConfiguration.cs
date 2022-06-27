using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Configurations
{
    public class WatchedListConfiguration : IEntityTypeConfiguration<Watchlist>
    {
        public void Configure(EntityTypeBuilder<Watchlist> builder)
        {
            builder.HasKey(e => new { e.ticker, e.IdUser});

            builder.HasOne(e => e.Client)
                .WithMany(e => e.Watchlist)
                .HasForeignKey(e => e.IdUser)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.WatchedElement)
                .WithMany(e => e.Watchlist)
                .HasForeignKey(e => e.ticker)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("WatchedList");
        }
    }
}
