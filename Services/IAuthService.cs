using InternshipApplicationBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipApplicationBackend.Services
{
    /// <summary>
    /// Defines authentication-related operations, including user registration and login.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="model">User registration details.</param>
        /// <param name="role">Role assigned to the user.</param>
        /// <returns>A tuple containing a status code and a message.</returns>
        Task<(int, string)> Registration(User model, string role);

        /// <summary>
        /// Authenticates a user based on login credentials.
        /// </summary>
        /// <param name="model">User login credentials.</param>
        /// <returns>A tuple containing a status code and either a token or an error message.</returns>
        Task<(int, string)> Login(LoginModel model);
    }
}
