
using InternshipApplicationBackend.Services;
using InternshipApplicationBackend.Exceptions;
using InternshipApplicationBackend.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;


namespace InternshipApplicationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternshipController : ControllerBase
    {
        private readonly InternshipService _internshipService;

        // Initializing Log4Net logger for logging purposes.
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        // Constructor: Initializes the controller with an instance of InternshipService.
        public InternshipController(InternshipService internshipService)
        {
            _internshipService = internshipService;
        }

        // Retrieves all internship records from the database.
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Internship>>> GetAllInternships()
        {
            log.Info("Fetching all internship records.");

            try
            {
                var internships = await _internshipService.GetAllInternships();
                log.Info($"Successfully retrieved {internships.Count()} internship records.");
                return Ok(internships);
            }
            catch (InternshipException ex)
            {
                log.Error($"Error fetching all internship records: {ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Retrieves an internship entry by its unique ID.
        [HttpGet("{internshipId}")]
        // [Authorize(Roles = "user")]
        public async Task<ActionResult<Internship>> GetInternshipById(int internshipId)
        {
            log.Info($"Fetching internship record for ID: {internshipId}");

            try
            {
                var internship = await _internshipService.GetInternshipById(internshipId);
                if (internship == null)
                {
                    log.Warn($"No internship found with ID: {internshipId}");
                    return NotFound("Cannot find any internship");
                }

                log.Info($"Successfully retrieved internship record for ID: {internshipId}");
                return Ok(internship);
            }
            catch (InternshipException ex)
            {
                log.Error($"Error fetching internship record for ID {internshipId}: {ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Allows a user to add a new internship entry to the database.

        [HttpPost]
        // [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddInternship([FromBody] Internship internship)
        {
            // Logging the start of the AddInternship process
            // log.Info("Initiating the process to add a new internship record.");

            try
            {
                // Attempt to add the internship using the service
                var success = await _internshipService.AddInternship(internship);

                if (success)
                {
                    // Log successful addition of internship
                    log.Info("Internship record added successfully.");
                    return Ok();
                }

                // Log failure to add internship
                log.Error("Failed to add internship due to unexpected issues.");
                return StatusCode(500, "Failed to add internship");
            }
            catch (InternshipException ex)
            {
                // Log a warning when a duplicate internship is detected
                log.Warn($"Duplicate internship record detected. Exception: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Logging error details for debugging
                log.Error($"Error adding internship application: {ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}"); // 500 Internal Server Error
            }
        }



        // Updates an existing internship entry by its unique ID.
        [HttpPut("{internshipId}")]
        public async Task<ActionResult> UpdateInternship(int internshipId, [FromBody] Internship internship)
        {
            log.Info($"Attempting to update internship record with ID: {internshipId}");

            try
            {
                var success = await _internshipService.UpdateInternship(internshipId, internship);
                if (success)
                {
                    log.Info($"Internship record with ID {internshipId} updated successfully.");
                    return Ok(internship);
                }

                log.Warn($"No internship found with ID: {internshipId}");
                return NotFound("Cannot find any internship");
            }
            catch (InternshipException ex)
            {
                log.Error($"Error updating internship with ID {internshipId}: {ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Deletes an internship entry by its unique ID.
        [HttpDelete("{internshipId}")]
        public async Task<ActionResult> DeleteInternship(int internshipId)
        {
            log.Info($"Attempting to delete internship record with ID: {internshipId}");

            try
            {
                var success = await _internshipService.DeleteInternship(internshipId);
                if (success)
                {
                    log.Info($"Internship record with ID {internshipId} deleted successfully.");
                    return Ok();
                }

                log.Warn($"No internship found with ID: {internshipId}");
                return NotFound("Cannot find any internship");
            }
            catch (InternshipException ex)
            {
                log.Error($"Error deleting internship with ID {internshipId}: {ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
