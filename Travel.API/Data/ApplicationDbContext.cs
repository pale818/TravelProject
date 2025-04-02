/*
 * DbContext and Trip DbSet
 * 	Maps your C# models (like Trip) to database tables
 •	Tracks changes to objects
 •	Lets you query, add, update, and delete rows in the database using LINQ or EF Core methods
 */

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Travel.API.Models;

// That file (ApplicationDbContext.cs) defines your EF Core database context, which is the central bridge between:
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

        // Makes sure EF Core uses the actual table name "Trip" and not "Trips"
        // "Trips" is used in the code for better meaning, plurar
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Trip>().ToTable("Trip");

            modelBuilder.Entity<Destination>().ToTable("Destination");

        }

    }
}
