
using InternshipApplicationBackend.Data;
using InternshipApplicationBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InternshipApplicationBackend.Services
{
    public class FeedbackService
    {
        private readonly ApplicationDbContext _context;

        // Constructor to initialize the database context, 
        // allowing interaction with the database through Entity Framework
        public FeedbackService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method to retrieve all feedback entries from the database
        // Returns an IEnumerable collection of Feedback objects
        public async Task<IEnumerable<Feedback>> GetAllFeedbacks()
        {
            // Fetch all feedback records from the Feedbacks table asynchronously
            var feedbackList = await _context.Feedbacks
                                    .Include(u => u.User)
                                    .ToListAsync();
            return feedbackList; // Return the list of feedback entries
        }

        // Method to retrieve feedback entries by user ID
        // Accepts a userId parameter and returns a list of feedback entries associated with that user
        public async Task<IEnumerable<Feedback>> GetFeedbacksByUserId(int userId)
        {
            // Query the Feedbacks table to filter feedbacks matching the given user ID
            var userFeedbacks = await _context.Feedbacks
                .Where(feedback => feedback.UserId == userId) // Filtering logic
                .Include(u => u.User)
                .ToListAsync(); // Convert result into a list asynchronously

            return userFeedbacks; // Return the filtered feedbacks
        }

        // Method to add a new feedback entry to the database
        // Accepts a Feedback object and returns a boolean indicating success or failure
        public async Task<bool> AddFeedback(Feedback feedback)
        {
            // Validate that the feedback object is not null before proceeding
            if (feedback == null)
            {
                return false; // Return false if the input is invalid
            }

            // Add the new feedback entry asynchronously to the Feedbacks table
            await _context.Feedbacks.AddAsync(feedback);

            // Commit the changes to the database
            await _context.SaveChangesAsync();

            return true; // Return true to indicate successful addition
        }

        // Method to delete a feedback entry from the database based on feedback ID
        // Accepts a feedbackId and returns a boolean indicating success or failure
        public async Task<bool> DeleteFeedback(int feedbackId)
        {
            // Attempt to find the feedback record corresponding to the given feedback ID
            var feedbackEntry = await _context.Feedbacks
                .FirstOrDefaultAsync(feedback => feedback.FeedbackId == feedbackId);

            // If no feedback is found, return false indicating deletion failure
            if (feedbackEntry == null)
            {
                return false;
            }

            // Remove the feedback entry from the Feedbacks table
            _context.Feedbacks.Remove(feedbackEntry);

            // Commit the deletion to the database
            await _context.SaveChangesAsync();

            return true; // Return true indicating successful deletion
        }
    }
}