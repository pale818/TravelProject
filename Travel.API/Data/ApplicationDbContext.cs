/*
 * DbContext and Trip DbSet
 */

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Travel.API.Models;

namespace Travel.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Trip> Trips { get; set; }
        // Add other DbSets later
    }
}
