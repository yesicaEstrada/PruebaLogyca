using Logyca.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logyca.Data.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Code>()
            .HasOne(c => c.Owner)
            .WithMany(e => e.Codes)
            .HasForeignKey(c => c.OwnerId)
            .IsRequired();

        builder.Entity<Code>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd(); //para que se genere el id autoincrementable

        builder.Entity<Enterprise>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Entity<Enterprise>()
            .HasIndex(e => e.Nit)
            .IsUnique();
    }

    public DbSet<Enterprise> Enterprises { get; set; }

    public DbSet<Code> Codes { get; set; }

}
