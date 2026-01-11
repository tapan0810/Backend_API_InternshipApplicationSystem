
using InternshipApplicationBackend.Exceptions;
using InternshipApplicationBackend.Models;
using InternshipApplicationBackend.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InternshipApplicationBackend  .Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternshipApplicationController : ControllerBase
    {
        private readonly InternshipApplicationService _internshipApplicationService;

        // Initializing Log4Net logger for logging purposes.
        // This logger tracks controller requests, successful operations, warnings, and errors for debugging purposes.
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        // Constructor: Injects the InternshipApplicationService instance.
        public InternshipApplicationController(InternshipApplicationService internshipApplicationService)
        {
            _internshipApplicationService = internshipApplicationService;
        }

        // 1. Retrieves all internship applications.
        // This endpoint allows users or admins to view all internship applications.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InternshipApplication>>> GetAllInternshipApplications()
        {
            // Logging the initiation of the fetch request.
            log.Info("Fetching all internship applications.");

            try
            {
                var applications = await _internshipApplicationService.GetAllInternshipApplications();

                // Logging successful retrieval of applications.
                log.Info($"Successfully retrieved {applications.Count()} internship applications.");
                return Ok(applications); // 200 OK response
            }
            catch (Exception ex)
            {
                // Logging error details for debugging.
                log.Error($"Error fetching internship applications: {ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}"); // 500 Internal Server Error
            }
        }

        // 2. Retrieves internship applications by user ID.
        // This endpoint fetches applications submitted by a specific user.
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<InternshipApplication>>> GetInternshipApplicationByUserId(int userId)
        {
            // Logging the initiation of the fetch request for a specific user ID.
            log.Info($"Fetching internship applications for User ID: {userId}");

            try
            {
                var applications = await _internshipApplicationService.GetInternshipApplicationsByUserId(userId);

                if (applications == null || !applications.Any())
                {
                    // Logging that no applications were found for the user.
                    log.Warn($"No internship applications found for User ID: {userId}");
                    return NotFound("Cannot find any internship application"); // 404 Not Found
                }

                // Logging successful retrieval of user-specific applications.
                log.Info($"Successfully retrieved {applications.Count()} applications for User ID: {userId}");
                return Ok(applications); // 200 OK response
            }
            catch (Exception ex)
            {
                // Logging error details for debugging.
                log.Error($"Error fetching internship applications for User ID {userId}: {ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}"); // 500 Internal Server Error
            }
        }

        // 3. Adds a new internship application.
        // This endpoint receives internship application data and validates it before adding it to the database.
        [HttpPost]
        public async Task<ActionResult> AddInternshipApplication([FromBody] InternshipApplication internshipApplication)
        {
            // Logging the initiation of adding an internship application.
            log.Info("Attempting to add a new internship application.");

            try
            {
                var success = await _internshipApplicationService.AddInternshipApplication(internshipApplication);

                if (success)
                {
                    // Logging successful addition of the internship application.
                    log.Info("Internship application added successfully.");
                    return Ok(); // 200 OK response
                }

                // Logging that invalid data was provided.
                log.Warn("Invalid internship application data provided.");
                return BadRequest("Failed to add internship application"); // 400 Bad Request
            }
            catch (InternshipException iex)
            {
                // Logging the custom exception related to internship applications.
                log.Warn($"Business rule violation: {iex.Message}", iex);
                return BadRequest(iex.Message); // 400 Bad Request
            }
            catch (Exception ex)
            {
                // Logging error details for debugging.
                log.Error($"Error adding internship application: {ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}"); // 500 Internal Server Error
            }
        }

        // 4. Updates an existing internship application.
        // This endpoint receives the application ID and updated data, then updates the application in the database.
        [HttpPut("{internshipApplicationId}")]
        public async Task<ActionResult> UpdateInternshipApplication(int internshipApplicationId, [FromBody] InternshipApplication internshipApplication)
        {
            // Logging the initiation of the update request.
            log.Info($"Attempting to update internship application with ID: {internshipApplicationId}");

            try
            {
                var success = await _internshipApplicationService.UpdateInternshipApplication(internshipApplicationId, internshipApplication);

                if (success)
                {
                    // Logging successful update.
                    log.Info($"Internship application with ID {internshipApplicationId} updated successfully.");
                    return Ok(internshipApplication); // 200 OK response
                }

                // Logging that the application was not found.
                log.Warn($"Cannot find internship application with ID: {internshipApplicationId}");
                return NotFound("Cannot find any internship application"); // 404 Not Found
            }
            catch (Exception ex)
            {
                // Logging error details for debugging.
                log.Error($"Error updating internship application with ID {internshipApplicationId}: {ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}"); // 500 Internal Server Error
            }
        }

        // 5. Deletes an internship application by ID.
        // This endpoint allows the user to delete an internship application by its unique ID.
        [HttpDelete("{internshipApplicationId}")]
        public async Task<ActionResult> DeleteInternshipApplication(int internshipApplicationId)
        {
            // Logging the initiation of the delete request.
            log.Info($"Attempting to delete internship application with ID: {internshipApplicationId}");

            try
            {
                var success = await _internshipApplicationService.DeleteInternshipApplication(internshipApplicationId);

                if (success)
                {
                    // Logging successful deletion.
                    log.Info($"Internship application with ID {internshipApplicationId} deleted successfully.");
                    return Ok(); // 200 OK response
                }

                // Logging that the application was not found.
                log.Warn($"Cannot find internship application with ID: {internshipApplicationId}");
                return NotFound("Cannot find any internship application"); // 404 Not Found
            }
            catch (Exception ex)
            {
                // Logging error details for debugging.
                log.Error($"Error deleting internship application with ID {internshipApplicationId}: {ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}"); // 500 Internal Server Error
            }
        }
    }
}



