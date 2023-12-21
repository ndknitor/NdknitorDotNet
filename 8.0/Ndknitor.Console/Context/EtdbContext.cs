using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
namespace Ndknitor.Console.Context;

public partial class EtdbContext : DbContext
{
    public EtdbContext(DbContextOptions<EtdbContext> options)
        : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
        .UseMySql("Server=127.0.0.1;Database=Etdb;User Id=root;Password=12345678aA#",
        ServerVersion.Create(new Version(8, 2, 0), ServerType.MySql));
        base.OnConfiguring(optionsBuilder);
    }

    public virtual DbSet<Bus> Bus { get; set; }

    public virtual DbSet<Route> Route { get; set; }

    public virtual DbSet<Seat> Seat { get; set; }

    public virtual DbSet<Ticket> Ticket { get; set; }

    public virtual DbSet<Trip> Trip { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Bus>(entity =>
        {
            entity.HasKey(e => e.BusId).HasName("PRIMARY");

            entity.Property(e => e.BusId).ValueGeneratedNever();
            entity.Property(e => e.Deleted)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)");
            entity.Property(e => e.LicensePlate)
                .IsRequired()
                .HasMaxLength(16);
            entity.Property(e => e.Name).HasMaxLength(128);
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.RouteId).HasName("PRIMARY");

            entity.Property(e => e.RouteId).ValueGeneratedNever();
            entity.Property(e => e.Deleted).HasColumnType("bit(1)");
            entity.Property(e => e.From)
                .IsRequired()
                .HasMaxLength(128)
                .HasDefaultValueSql("''");
            entity.Property(e => e.To)
                .IsRequired()
                .HasMaxLength(128)
                .HasDefaultValueSql("''");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId).HasName("PRIMARY");

            entity.HasIndex(e => e.BusId, "BusId");

            entity.Property(e => e.SeatId).ValueGeneratedNever();
            entity.Property(e => e.Deleted)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(128);

            entity.HasOne(d => d.Bus).WithMany(p => p.Seat)
                .HasForeignKey(d => d.BusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Seat_ibfk_1");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PRIMARY");

            entity.HasIndex(e => e.SeatId, "SeatId");

            entity.HasIndex(e => e.TripId, "TripId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.TicketId).ValueGeneratedNever();
            entity.Property(e => e.BookedDate).HasColumnType("datetime");
            entity.Property(e => e.From).HasMaxLength(128);
            entity.Property(e => e.To).HasMaxLength(128);

            entity.HasOne(d => d.Seat).WithMany(p => p.Ticket)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Ticket_ibfk_2");

            entity.HasOne(d => d.Trip).WithMany(p => p.Ticket)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Ticket_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.Ticket)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Ticket_ibfk_3");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.TripId).HasName("PRIMARY");

            entity.HasIndex(e => e.RouteId, "RouteId");

            entity.Property(e => e.TripId).ValueGeneratedNever();
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Route).WithMany(p => p.Trip)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Trip_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(128)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(128);
            entity.Property(e => e.Fullname)
                .IsRequired()
                .HasMaxLength(128)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(16)
                .HasDefaultValueSql("''");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
