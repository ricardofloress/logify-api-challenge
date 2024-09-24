using System;

namespace ApiApplication.Domain.Exceptions
{
    public class MovieErrorException : Exception
    {
        public MovieErrorException(string message) : base(message)
        {
        }
    }
}