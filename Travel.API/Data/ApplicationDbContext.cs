/*
 * DbContext and Trip DbSet
 * 	Maps your C# models (like Trip) to database tables
 •	Tracks changes to objects
 •	Lets you query, add, update, and delete rows in the database using LINQ or EF Core methods
 */

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Travel.API.Models;

// This file (ApplicationDbContext.cs) defines your EF Core database context, which is the central bridge between:
// C# models (Trip, Destination, etc.)
// actual database tables
namespace Travel.API.Data
{
    // inherits from DbContext, the base class in Entity Framework Core that handles all interaction with the database.
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // *****************************************************************
        public DbSet<Trip> Trips { get; set; }

        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Guide> Guides { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Log> Logs { get; set; }

        // Makes sure EF Core uses the actual table name "Trip" and not "Trips"
        // "Trips" is used in the code for better meaning, plurar
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Trip>().ToTable("Trip");

            modelBuilder.Entity<Destination>().ToTable("Destination");

            modelBuilder.Entity<Guide>().ToTable("Guide");

            // bridge table between Trip and Guide
            /*
             * This tells EF Core: “I already have a bridge table called TripGuide with columns TripId and GuideId, and it’s the join between Trip and Guide.”
             */
            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Guides)
                .WithMany(g => g.Trips)
                .UsingEntity<Dictionary<string, object>>(
                    "TripGuide",
                    j => j
                        .HasOne<Guide>()
                        .WithMany()
                        .HasForeignKey("GuideId"),
                    j => j
                        .HasOne<Trip>()
                        .WithMany()
                        .HasForeignKey("TripId"),
                    j =>
                    {
                        j.HasKey("TripId", "GuideId");
                        j.ToTable("TripGuide"); // exactly matches your SQL table
                    });

            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Destinations)
                .WithMany(d => d.Trips)
                .UsingEntity<Dictionary<string, object>>(
                    "TripDestination",
                    j => j
                        .HasOne<Destination>()
                        .WithMany()
                        .HasForeignKey("DestinationId"),
                    j => j
                        .HasOne<Trip>()
                        .WithMany()
                        .HasForeignKey("TripId"),
                    j =>
                    {
                        j.HasKey("TripId", "DestinationId");
                        j.ToTable("TripDestination"); // table name matches your style
                    });


            modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUser");

            modelBuilder.Entity<Wishlist>().ToTable("Wishlist");

            modelBuilder.Entity<Log>().ToTable("Log");

        }
    }
}
