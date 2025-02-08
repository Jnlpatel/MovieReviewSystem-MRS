using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieReviewSystem.Models;

namespace MovieReviewSystem.Data
{
    public class MovieReviewSystemContext : DbContext
    {
        public MovieReviewSystemContext (DbContextOptions<MovieReviewSystemContext> options)
            : base(options)
        {
        }

        public DbSet<MovieReviewSystem.Models.User> Users { get; set; } = default!;
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Many-to-Many Relationship: Movie <-> Genre
            modelBuilder.Entity<MovieGenre>().HasKey(mg => new { mg.MovieID, mg.GenreID });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieID);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreID);
        }
    }
}
