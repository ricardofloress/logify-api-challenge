using System;

namespace ApiApplication.Domain.Exceptions
{
    public class TicketReservationErrorException : Exception
    {
        public TicketReservationErrorException(string message) : base(message)
        {
        }
    }
}