using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(e => e.IdUser);
            builder.Property(e => e.Login).HasMaxLength(32).IsRequired();
            builder.HasIndex(e => e.Login).IsUnique();

            builder.Property(e => e.Password).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Salt).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Email).HasMaxLength(100).IsRequired();
            builder.Property(e => e.RefreshToken).IsRequired();
            builder.ToTable("Client");
        }
    }
}
