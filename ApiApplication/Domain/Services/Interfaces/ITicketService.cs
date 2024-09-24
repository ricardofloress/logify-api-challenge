using System;
using System.Threading.Tasks;
using ApiApplication.Domain.Dtos;
using ApiApplication.Domain.Dtos.Response;

namespace ApiApplication.Domain.Services.Interfaces
{
    public interface ITicketService
    {
        Task<TicketResponseDto> TicketReservation(TicketReservationRequestDto request);

        Task<TicketResponseDto> TicketConfirmation(Guid id);
    }
}