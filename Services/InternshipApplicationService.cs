
using InternshipApplicationBackend.Data;
using InternshipApplicationBackend.Exceptions;
using InternshipApplicationBackend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipApplicationBackend.Services
{
    // Service class to manage internship applications, handling CRUD operations.
    // This class interacts with the database context to perform all CRUD operations
    // and applies business logic to ensure data integrity.
    public class InternshipApplicationService
    {
        // Dependency injection for accessing the database.
        private readonly ApplicationDbContext _context;

        // Constructor to initialize the service with a database context.
        public InternshipApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieves all internship applications from the database.
        // This method is used to fetch every internship application without any filters.
        public async Task<IEnumerable<InternshipApplication>> GetAllInternshipApplications()
        {
            // Querying the database to get all internship applications.
            return await _context.InternshipApplications
            .Include(a => a.User)
            .Include(a => a.Internship)
            .ToListAsync();
        }

        // Retrieves internship applications associated with a specific user ID.
        // This method filters internship applications based on the userId parameter.
        public async Task<IEnumerable<InternshipApplication>> GetInternshipApplicationsByUserId(int userId)
        {
            // Querying the database for internship applications where the UserId matches.
            return await _context.InternshipApplications
                .Where(application => application.UserId == userId)
                .Include(a => a.User)
                .Include(a => a.Internship)
                .ToListAsync();
        }

        // Adds a new internship application to the database after validating for duplicates.
        // Throws an exception if a duplicate application is found.
        public async Task<bool> AddInternshipApplication(InternshipApplication internshipApplication)
        {
            // Check if the user has already applied for this internship by matching UserId and InternshipId.
            var existingApplication = await _context.InternshipApplications
                .FirstOrDefaultAsync(application =>
                    application.UserId == internshipApplication.UserId &&
                    application.InternshipId == internshipApplication.InternshipId);

            if (existingApplication != null)
            {
                // Throw a custom exception if a duplicate application is found.
                throw new InternshipException("User already applied for this internship");
            }

            // Add the new internship application to the database.
            _context.InternshipApplications.Add(internshipApplication);

            // Save changes asynchronously to the database.
            await _context.SaveChangesAsync();
            return true; // Return true to indicate successful addition.
        }

        // Updates an existing internship application in the database.
        // If no application is found for the provided ID, the method returns false.
        public async Task<bool> UpdateInternshipApplication(int internshipApplicationId, InternshipApplication internshipApplication)
        {
            // Find the existing application by its ID.
            var existingApplication = await _context.InternshipApplications
                .FirstOrDefaultAsync(application => application.InternshipApplicationId == internshipApplicationId);

            if (existingApplication == null)
            {
                // Return false if the application is not found in the database.
                return false;
            }

            // Update the fields of the existing application with new values.
            existingApplication.UniversityName = internshipApplication.UniversityName;
            existingApplication.DegreeProgram = internshipApplication.DegreeProgram;
            existingApplication.Resume = internshipApplication.Resume;
            existingApplication.LinkedInProfile = internshipApplication.LinkedInProfile;
            existingApplication.ApplicationStatus = internshipApplication.ApplicationStatus;

            // Save the updated application back to the database.
            await _context.SaveChangesAsync();
            return true; // Return true to indicate a successful update.
        }

        // Deletes an internship application from the database using its unique ID.
        // Returns false if the application is not found, otherwise deletes it and returns true.
        public async Task<bool> DeleteInternshipApplication(int internshipApplicationId)
        {
            // Find the application to delete by its ID.
            var existingApplication = await _context.InternshipApplications
                .FirstOrDefaultAsync(application => application.InternshipApplicationId == internshipApplicationId);

            if (existingApplication == null)
            {
                // Return false if no application with the given ID is found.
                return false;
            }

            // Remove the application from the database.
            _context.InternshipApplications.Remove(existingApplication);

            // Save changes asynchronously to the database.
            await _context.SaveChangesAsync();
            return true; // Return true to indicate successful deletion.
        }
    }
}