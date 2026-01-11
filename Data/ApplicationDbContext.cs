using Microsoft.EntityFrameworkCore;
using InternshipApplicationBackend.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using InternshipApplicationBackend.Models;


namespace InternshipApplicationBackend.Data
{
    // ApplicationDbContext is responsible for interacting with the database 
    // using Entity Framework Core. It manages entity mappings and database operations.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Constructor to initialize the DbContext with provided options (e.g., connection string).
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet properties define tables within the database.
        // Each DbSet represents a table containing records of the corresponding model.

        // Users table to store user-related data.
        public DbSet<User> Users { get; set; }

        // Internships table to store information about internship opportunities.
        public DbSet<Internship> Internships { get; set; }

        // InternshipApplications table to store applications submitted by users for internships.
        public DbSet<InternshipApplication> InternshipApplications { get; set; }

        // Feedbacks table to store feedback provided by users regarding internships or services.
        public DbSet<Feedback> Feedbacks { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder); // Ensures Identity tables are configured correctly
        // }

        // // Override OnModelCreating to configure database schemas, relationships, and indexes.
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     // Apply an index on the CompanyName column in the Internship table.
        //     // This improves query performance when searching for internships by company name.
        //     modelBuilder.Entity<Internship>()
        //         .HasIndex(i => i.CompanyName)
        //         .HasDatabaseName("Idx_Internship_CompanyName"); // Optional: Assign a custom index name

        //     // Apply an index on the Location column in the Internship table.
        //     // This enhances search efficiency when filtering internships by location.
        //     modelBuilder.Entity<Internship>()
        //         .HasIndex(i => i.Location)
        //         .HasDatabaseName("Idx_Internship_Location"); // Optional: Assign a custom index name

        //     // Applying an index on the DegreeProgram column in InternshipApplication table
        //     // This helps optimize search performance when filtering internship applications based on Degree Program.

    }
}


