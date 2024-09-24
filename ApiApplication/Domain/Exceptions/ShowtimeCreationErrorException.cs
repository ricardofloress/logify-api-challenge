using System;

namespace ApiApplication.Domain.Exceptions
{
    public class ShowtimeCreationErrorException : Exception
    {
        public ShowtimeCreationErrorException(string message) : base(message)
        {
        }
    }
}