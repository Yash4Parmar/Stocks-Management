using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Stocks_Management.Models;

public partial class StockManagementContext : DbContext
{
    public StockManagementContext()
    {
    }

    public StockManagementContext(DbContextOptions<StockManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<StockOrder> StockOrders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)=> optionsBuilder.UseSqlServer("Server=LAPTOP-6FBJ3NLA;Database=StockManagement:;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasIndex(e => e.Id, "IX_Stocks");
        });

        modelBuilder.Entity<StockOrder>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Oid).HasColumnName("OId");
            entity.Property(e => e.Sid).HasColumnName("SId");

            entity.HasOne(d => d.OidNavigation).WithMany(p => p.StockOrders)
                .HasForeignKey(d => d.Oid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StockOrders_Orders");

            entity.HasOne(d => d.SidNavigation).WithMany(p => p.StockOrders)
                .HasForeignKey(d => d.Sid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StockOrders_Stocks");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
