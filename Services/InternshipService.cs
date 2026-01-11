
using InternshipApplicationBackend.Exceptions;
using InternshipApplicationBackend.Data; // Namespace for database context
using InternshipApplicationBackend.Models;
using Microsoft.EntityFrameworkCore; // Entity Framework namespace for database operations
using System.Collections.Generic; // Namespace for collections

namespace InternshipApplicationBackend.Services // Defining the namespace for services
{
    public class InternshipService
    {
        private readonly ApplicationDbContext _context;

        // Constructor to initialize the database context
        public InternshipService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method to retrieve all internships from the database
        // Returns an IEnumerable collection of Internship objects
        public async Task<IEnumerable<Internship>> GetAllInternships()
        {
            // Fetch all internship records asynchronously from the Internships table
            return await _context.Internships.ToListAsync();
        }

        // Method to retrieve an internship by its ID
        // Accepts an internshipId parameter and returns the corresponding Internship object
        public async Task<Internship> GetInternshipById(int internshipId)
        {
            // Query the Internships table to find the internship matching the given ID
            return await _context.Internships.FirstOrDefaultAsync(i => i.InternshipId == internshipId);
        }

        // Method to add a new internship to the database
        // Accepts a new Internship object and returns a boolean indicating success
        public async Task<bool> AddInternship(Internship internship)
        {
            var matching = await _context.Internships.FirstOrDefaultAsync(i => i.CompanyName == internship.CompanyName);
            if (matching != null)
            {
                throw new InternshipException("Company with the same name already exists");
            }

            await _context.Internships.AddAsync(internship);
            await _context.SaveChangesAsync();
            return true; // Successfully added
        }

        // Method to update an existing internship in the database
        // Accepts an internshipId and an updated Internship object, and returns a boolean
        public async Task<bool> UpdateInternship(int internshipId, Internship internship)
        {
            // Retrieve the existing internship record using the provided ID
            var existingInternship = await _context.Internships.FirstOrDefaultAsync(i => i.InternshipId == internshipId);

            if (existingInternship == null)
            {
                return false; // Return false if no matching internship is found
            }

            // Update the existing internship record with new values
            existingInternship.Title = internship.Title;
            existingInternship.CompanyName = internship.CompanyName;
            existingInternship.Location = internship.Location;
            existingInternship.DurationInMonths = internship.DurationInMonths;
            existingInternship.Stipend = internship.Stipend;
            existingInternship.Description = internship.Description;
            existingInternship.SkillsRequired = internship.SkillsRequired;
            existingInternship.ApplicationDeadline = internship.ApplicationDeadline;
            // Save changes to the database
            await _context.SaveChangesAsync();

            return true; // Return true indicating successful update
        }

        // Method to delete an internship from the database
        // Accepts an internshipId and returns a boolean indicating success
        public async Task<bool> DeleteInternship(int internshipId)
        {
            // Retrieve the existing internship record using the provided ID
            var internshipEntry = await _context.Internships.FirstOrDefaultAsync(i => i.InternshipId == internshipId);

            if (internshipEntry == null)
            {
                return false; // Return false if no matching internship is found
            }

            // Remove the internship record from the database
            _context.Internships.Remove(internshipEntry);
            // Save changes to the database
            await _context.SaveChangesAsync();

            return true; // Return true indicating successful deletion
        }
    }
}
