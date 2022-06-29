using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Configurations
{
    public class OHLCConfiguration : IEntityTypeConfiguration<OHLC>
    {
        public void Configure(EntityTypeBuilder<OHLC> builder) 
        {
            builder.HasKey(e => new { e.WatchedElementTicker, e.t });

            builder.Property(e => e.h).IsRequired();
            builder.Property(e => e.l).IsRequired();
            builder.Property(e => e.n).IsRequired();
            builder.Property(e => e.o).IsRequired();
            builder.Property(e => e.c).IsRequired();
            builder.Property(e => e.vw).IsRequired();
            builder.Property(e => e.v).IsRequired();
            builder.HasOne(e => e.Watched)
                .WithMany(e => e.OHLCs)
                .HasForeignKey(e => e.WatchedElementTicker)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
