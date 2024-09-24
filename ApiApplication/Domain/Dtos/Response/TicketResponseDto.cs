using System;
using System.Collections.Generic;
using ApiApplication.Database.Entities;

namespace ApiApplication.Domain.Dtos.Response
{
    public class TicketResponseDto
    {
        public string Id { get; set; }
        public bool Paid { get; set; }
        public ShowtimeResponseDto Showtime { get; set; }
        public List<SeatEntity> Seats { get; set; }
    }
}