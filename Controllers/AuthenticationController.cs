
using InternshipApplicationBackend.Models;
using InternshipApplicationBackend.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace InternshipApplicationBackend.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        // Initializing Log4Net logger for logging purposes.
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Constructor initializes the authentication service.
        /// </summary>
        /// <param name="authService">Service handling authentication.</param>
        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="model">User registration details.</param>
        /// <returns>Success message or error response.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            log.Info("Register request received.");

            if (!ModelState.IsValid)
            {
                log.Warn("Invalid registration request payload.");
                return BadRequest("Invalid request payload");
            }

            try
            {
                var (status, result) = await _authService.Registration(model, model.UserRole);

                if (status == 0)
                {
                    log.Warn($"Registration failed: {result}");
                    return BadRequest(result);
                }

                log.Info("User registered successfully.");
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                log.Error($"Error in registration: {ex.Message}", ex);
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="model">User login credentials.</param>
        /// <returns>Authentication token or error response.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            log.Info("Login request received.");

            if (!ModelState.IsValid)
            {
                log.Warn("Invalid login request payload.");
                return BadRequest("Invalid request payload");
            }

            try
            {
                var (status, result) = await _authService.Login(model);

                if (status == 0)
                {
                    log.Warn($"Login failed: {result}");
                    return BadRequest(result);
                }

                log.Info("User logged in successfully.");
                return Ok(new { Token = result });
            }
            catch (Exception ex)
            {
                log.Error($"Error in login process: {ex.Message}", ex);
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
