using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VideoKlub2019.Models
{
    public partial class VideoKlubContext : DbContext
    {
        public VideoKlubContext()
        {
        }

        public VideoKlubContext(DbContextOptions<VideoKlubContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Film> Filmovi { get; set; }
        public virtual DbSet<Iznajmljivanje> Iznajmljivanja { get; set; }
        public virtual DbSet<Komentar> Komentari { get; set; }
        public virtual DbSet<Zanr> Zanrovi { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Film>(entity =>
            {
                entity.HasOne(d => d.Zanr)
                    .WithMany(p => p.Filmovi)
                    .HasForeignKey(d => d.ZanrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Film__ZanrId__4BAC3F29");
            });

            modelBuilder.Entity<Iznajmljivanje>(entity =>
            {
                entity.Property(e => e.DatumIznajmljivanja).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.Iznajmljivanja)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Iznajmlji__FilmI__4E88ABD4");
            });

            modelBuilder.Entity<Komentar>(entity =>
            {
                entity.HasOne(d => d.Film)
                    .WithMany(p => p.Komentari)
                    .HasForeignKey(d => d.FilmId)
                    .HasConstraintName("FK__Komentar__FilmId__03F0984C");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}