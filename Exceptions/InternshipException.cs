using System;

namespace InternshipApplicationBackend.Exceptions
{
    /// Custom exception class for handling errors related to internship operations.
    /// This exception is thrown in scenarios such as:
    /// - Deleting an internship that is referenced by an internship application.
    /// - Adding an internship with a duplicate company name.
    /// - Applying for an internship that has already been applied for by the same user.
    public class InternshipException : Exception
    {
        /// Constructor that accepts a custom error message.
        /// Allows specifying detailed error information when throwing an exception.
        public InternshipException(string message) : base(message)
        {
        }
    }
}