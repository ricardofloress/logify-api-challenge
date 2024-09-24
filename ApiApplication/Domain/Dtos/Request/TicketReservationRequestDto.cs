using System.Collections.Generic;
using System.Runtime.Serialization;
using ApiApplication.Database.Entities;

namespace ApiApplication.Domain.Dtos
{
    public class TicketReservationRequestDto
    {
        public int ShowtimeId { get; set; }
        public List<SeatEntity> Seats { get; set; }
    }
}