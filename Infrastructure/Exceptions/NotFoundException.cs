using System;

namespace Infrastructure.Exceptions
{
    /// <summary>
    /// This exception will thrown if some resource not found
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Constructor for specific message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public NotFoundException(string message) : base(message)
        {

        }
    }
}