using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Configurations
{
    public class WatchedElementConfiguration : IEntityTypeConfiguration<WatchedElement>
    {
        public void Configure(EntityTypeBuilder<WatchedElement> builder)
        {
            builder.HasKey(e => e.ticker);
            builder.Property(e => e.logo);
            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
            builder.Property(e => e.description);
            builder.Property(e => e.ticker).HasMaxLength(100).IsRequired();
            builder.Property(e => e.country).HasMaxLength(100).IsRequired();

            builder.ToTable("WatchedElement");
        }
    }
}
