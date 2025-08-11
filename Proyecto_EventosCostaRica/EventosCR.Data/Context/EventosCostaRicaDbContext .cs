using System;
using System.Collections.Generic;
using EventosCR.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace EventosCR.Data.Context;

public partial class EventosCostaRicaDbContext : DbContext
{
    public EventosCostaRicaDbContext()
    {
    }

    public EventosCostaRicaDbContext(DbContextOptions<EventosCostaRicaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asiento> Asientos { get; set; }

    public virtual DbSet<Boleto> Boletos { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Name=ConnectionStrings:EventosCostaRica");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Asientos__3214EC07E001FA3A");

            entity.HasIndex(e => new { e.EventoId, e.Fila, e.Numero }, "UQ__Asientos__FBCA51CE3560C7F6").IsUnique();

            entity.Property(e => e.Disponible).HasDefaultValue(true);

            entity.HasOne(d => d.Evento).WithMany(p => p.Asientos)
                .HasForeignKey(d => d.EventoId)
                .HasConstraintName("FK__Asientos__Evento__44FF419A");
        });

        modelBuilder.Entity<Boleto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Boletos__3214EC0708113EED");


            entity.HasIndex(e => e.AsientoId, "UQ__Boletos__04904D11B5760BB8").IsUnique();

            entity.Property(e => e.FechaCompra).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Asiento).WithOne(p => p.Boleto)
                .HasForeignKey<Boleto>(d => d.AsientoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Boletos__Asiento__4BAC3F29");

            entity.HasOne(d => d.Evento).WithMany(p => p.Boletos)
                .HasForeignKey(d => d.EventoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Boletos__EventoI__4AB81AF0");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Boletos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Boletos__Usuario__49C3F6B7");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Eventos__3214EC074E3FEC8A");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Banner).HasMaxLength(500);
            entity.Property(e => e.CapacidadTotal).HasDefaultValue(100);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Lugar).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.PrecioEntrada).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Vendido).HasDefaultValue(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC07C7EAAD18");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534A6E9EEB4").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Rol).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
