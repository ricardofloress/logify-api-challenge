using System;

namespace ApiApplication.Domain.Exceptions
{
    public class TicketConfirmationErrorException : Exception
    {
        public TicketConfirmationErrorException(string message) : base(message)
        {
        }
    }
}