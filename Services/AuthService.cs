using BCrypt.Net; // Importing BCrypt for password hashing
using InternshipApplicationBackend.Data;
using InternshipApplicationBackend.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApplicationBackend.Services
{
    /// <summary>
    /// Service responsible for user authentication and registration.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes the authentication service with database context and configuration settings.
        /// </summary>
        /// <param name="configuration">Application configuration settings.</param>
        /// <param name="dbContext">Database context for user management.</param>
        public AuthService(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Registers a new user in the system with encrypted password storage.
        /// </summary>
        /// <param name="model">User registration details.</param>
        /// <param name="role">Role assigned to the user.</param>
        /// <returns>A tuple containing a status code and a message.</returns>
        public async Task<(int, string)> Registration(User model, string role)
        {
            // Check if user already exists
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                return (0, "User already exists");
            }

            // Encrypt the password before storing it
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            model.UserRole = role;
            _dbContext.Users.Add(model);
            await _dbContext.SaveChangesAsync();

            return (1, "User created successfully!");
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="model">User login credentials.</param>
        /// <returns>A tuple containing a status code and either a token or an error message.</returns>
        public async Task<(int, string)> Login(LoginModel model)
        {
            // Validate user email
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                return (0, "Invalid email or password");
            }

            // Validate password using BCrypt hash verification
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return (0, "Invalid email or password");
            }

            // Generate authentication token
            var token = GenerateToken(user, user.UserRole);
            return (1, token);
        }

        /// <summary>
        /// Generates a JWT token for the authenticated user.
        /// </summary>
        /// <param name="user">User data.</param>
        /// <param name="role">Role assigned to the user.</param>
        /// <returns>JWT token string.</returns>
        private string GenerateToken(User user, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role),
                new Claim("username", user.Username) // ✅ Add username to JWT
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = creds,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
