using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EFProject.EF;

public partial class HotelContext : DbContext
{
    public HotelContext()
    {
        Database.EnsureCreated();
    }

    public HotelContext(DbContextOptions<HotelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomGuest> RoomGuests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["HotelPath"].ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.IdGuest).HasName("PK__tmp_ms_x__69404E79F5509C99");

            entity.ToTable("Guest");

            entity.Property(e => e.FullName)
                .HasMaxLength(30)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.IdRoom).HasName("PK__tmp_ms_x__B436880FCDDA85F7");

            entity.ToTable("Room");

            entity.HasIndex(e => e.Number, "UQ__tmp_ms_x__78A1A19D028D3130").IsUnique();

            entity.HasOne(d => d.IdGuestNavigation).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.IdGuest)
                .HasConstraintName("FK_Room_Guest");
        });

        modelBuilder.Entity<RoomGuest>(entity =>
        {
            entity.HasKey(e => e.IdRoomGuest).HasName("PK__tmp_ms_x__5D76AB34D74DE054");

            entity.ToTable("RoomGuest");

            entity.HasOne(d => d.IdGuestNavigation).WithMany(p => p.RoomGuests)
                .HasForeignKey(d => d.IdGuest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomGuest_Guest");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
