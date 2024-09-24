using System.Linq;
using ApiApplication.Database.Entities;
using ApiApplication.Domain.Dtos.Response;

namespace ApiApplication.Domain.Extensions
{
    public static class TicketExtension
    {
        public static TicketResponseDto ToTicketResponseDto(this TicketEntity ticket) =>
            new TicketResponseDto()
            {
                Id = ticket.Id.ToString(),
                Showtime = ticket.Showtime?.ToShowtimeResponseDto(),
                Paid = ticket.Paid,
                Seats = ticket.Seats?.ToList()
            };
    }
}