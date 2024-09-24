using System;

namespace ApiApplication.Domain.Exceptions
{
    public class AuditoriumNotFoundException : Exception
    {
        public AuditoriumNotFoundException(string message) : base(message)
        {
        }
    }
}