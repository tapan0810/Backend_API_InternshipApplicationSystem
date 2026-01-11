
using InternshipApplicationBackend.Models;
using InternshipApplicationBackend.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace InternshipApplicationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;

        // Initializing Log4Net logger for logging purposes.
        // This logger helps record application events such as successful operations, warnings, errors, and debugging information.
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        // Constructor: Initializes the controller with an instance of FeedbackService.
        // Logging is enabled to track controller requests and operations.
        public FeedbackController(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // Retrieves all feedback records from the database.
        // This endpoint allows users or admins to view all feedback provided by different users.
        // If an error occurs during retrieval, it returns a 500 Internal Server Error.
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetAllFeedbacks()
        {
            // Logging request initiation: Captures the start of fetching feedback data
            log.Info("Fetching all feedback records.");

            try
            {
                var feedbacks = await _feedbackService.GetAllFeedbacks(); // Fetch all feedback entries

                // Logging successful response: Records number of feedbacks retrieved
                log.Info($"Successfully retrieved {feedbacks.Count()} feedback records.");
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                // Logging error: Captures the error message and stack trace for debugging
                log.Error($"Error fetching feedback records: {ex.Message}", ex);
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // Retrieves feedback entries based on a specific user ID.
        // This endpoint is useful when displaying feedbacks provided by a particular user.
        // If an error occurs during retrieval, it returns a 500 Internal Server Error.
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacksByUserId(int userId)
        {
            // Logging request initiation: Captures request with specific User ID
            log.Info($"Fetching feedback records for User ID: {userId}");

            try
            {
                var feedbacks = await _feedbackService.GetFeedbacksByUserId(userId); // Fetch feedbacks specific to a user

                // Logging successful response: Confirms feedbacks were successfully retrieved
                log.Info($"Successfully retrieved feedback for User ID: {userId}");
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                // Logging error: Captures failure in fetching user-specific feedback
                log.Error($"Error fetching feedback for User ID {userId}: {ex.Message}", ex);
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // Allows a user to submit feedback about an internship, application experience, or platform usage.
        // Receives feedback data from the request body and validates it before adding it to the database.
        // If successful, returns 200 OK with a success message.
        // If an error occurs during submission, it returns a 500 Internal Server Error.
        [HttpPost()]
        public async Task<ActionResult> AddFeedback([FromBody] Feedback feedback)
        {
            // Logging request initiation: Captures the feedback submission attempt
            log.Info("Attempting to add new feedback.");

            try
            {
                var success = await _feedbackService.AddFeedback(feedback); // Attempt to add feedback entry
                if (success)
                {
                    // Logging successful response: Confirms feedback addition was successful
                    log.Info("Feedback added successfully.");
                    return Ok();
                }

                // Logging warning: Indicates invalid feedback data was provided
                log.Warn("Invalid feedback data provided.");
                return BadRequest("Invalid feedback data provided.");
            }
            catch (Exception ex)
            {
                // Logging error: Captures any issues encountered during feedback submission
                log.Error($" addingError feedback: {ex.Message}", ex);
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // Deletes a feedback entry based on its unique ID.
        // This endpoint allows users or admins to remove inappropriate or outdated feedback.
        // If the feedback entry exists, it is deleted, and a 200 OK response is returned.
        // If no matching feedback is found, it returns a 404 Not Found.
        // If an error occurs, it returns a 500 Internal Server Error.
        [HttpDelete("{feedbackId}")]
        public async Task<ActionResult> DeleteFeedback(int feedbackId)
        {
            // Logging request initiation: Captures deletion request with specific feedback ID
            log.Info($"Attempting to delete feedback ID: {feedbackId}");

            try
            {
                var success = await _feedbackService.DeleteFeedback(feedbackId); // Attempt to delete feedback entry
                if (success)
                {
                    // Logging successful response: Confirms feedback deletion
                    log.Info($"Feedback ID {feedbackId} deleted successfully.");
                    return Ok();
                }

                // Logging warning: Indicates feedback not found in the database
                log.Warn($"Feedback ID {feedbackId} not found.");
                return NotFound("Cannot find any feedback.");
            }
            catch (Exception ex)
            {
                // Logging error: Captures issues encountered during deletion
                log.Error($"Error deleting feedback ID {feedbackId}: {ex.Message}", ex);
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}