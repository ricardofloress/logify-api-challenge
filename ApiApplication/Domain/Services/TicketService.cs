using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Domain.Dtos;
using ApiApplication.Domain.Dtos.Response;
using ApiApplication.Domain.Exceptions;
using ApiApplication.Domain.Extensions;
using ApiApplication.Domain.Services.Interfaces;

namespace ApiApplication.Domain.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IShowtimesRepository _showtimesRepository;
        private readonly IAuditoriumService _auditoriumService;

        public TicketService(ITicketsRepository ticketsRepository, IShowtimesRepository showtimesRepository,
            IAuditoriumService auditoriumService)
        {
            _ticketsRepository = ticketsRepository;
            _showtimesRepository = showtimesRepository;
            _auditoriumService = auditoriumService;
        }

        public async Task<TicketResponseDto> TicketReservation(TicketReservationRequestDto request)
        {
            try
            {
                if (request.Seats == null || request.Seats.Count() == 0)
                    throw new TicketReservationErrorException("Please provide at least one seat.");

                var showtime =
                    await _showtimesRepository.GetWithMoviesByIdAsync(request.ShowtimeId, new CancellationToken());

                if (showtime is null)
                    throw new TicketReservationErrorException("Showtime Not Found.");

                if (!AreSeatsContiguous(request.Seats))
                    throw new TicketReservationErrorException("Seats are not contiguous.");

                var reservedSeats = await CheckIfSeatsAreAlreadyReservedOrSold(request, showtime);

                if (reservedSeats)
                    throw new TicketReservationErrorException("Some seats are already reserved or sold.");

                var seatsForTicket = await _auditoriumService.GetSeatsForTicket(showtime.AuditoriumId, request.Seats);

                var ticketReservation =
                    await _ticketsRepository.CreateAsync(showtime, seatsForTicket, new CancellationToken());

                if (ticketReservation is null)
                    throw new TicketReservationErrorException("Error on saving reservation in database.");

                return ticketReservation.ToTicketResponseDto();
            }
            catch (Exception e)
            {
                throw new TicketReservationErrorException($"Error creating reservation: {e.Message}");
            }
        }

        public async Task<TicketResponseDto> TicketConfirmation(Guid id)
        {
            try
            {
                var ticket = await _ticketsRepository.GetAsync(id, new CancellationToken());

                if (ticket is null)
                    throw new TicketConfirmationErrorException("Ticket Not Found.");
                
                if (ticket.CreatedTime.AddMinutes(10) < DateTime.Now)
                    throw new TicketConfirmationErrorException("Ticket Expired.");

                if (ticket.Paid)
                    throw new TicketConfirmationErrorException("Ticket already Paid.");

                var confirmatedTicket = await _ticketsRepository.ConfirmPaymentAsync(ticket, new CancellationToken());
                
                if (confirmatedTicket is null)
                    throw new TicketConfirmationErrorException("Error o payment confirmation.");

                return confirmatedTicket.ToTicketResponseDto();
            }
            catch (Exception e)
            {
                throw new TicketReservationErrorException($"Error on ticket confirmation: {e.Message}");
            }
        }

        private static bool AreSeatsContiguous(List<SeatEntity> seats)
        {
            seats = seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber).ToList();

            for (int i = 1; i < seats.Count; i++)
                if (seats[i].Row == seats[i - 1].Row)
                    if (seats[i].SeatNumber != seats[i - 1].SeatNumber + 1)
                        return false;

            return true;
        }

        private async Task<bool> CheckIfSeatsAreAlreadyReservedOrSold(TicketReservationRequestDto request,
            ShowtimeEntity showtime)
        {
            var showtimeTickets = await _ticketsRepository.GetEnrichedAsync(showtime.Id, new CancellationToken());

            var seatsInRangeOfAuditorium =
                await _auditoriumService.AreSeatsInRangeOfAuditorium(showtime.AuditoriumId, request.Seats);

            if (!seatsInRangeOfAuditorium)
                return true;

            var reservedOrPurchasedSeats = showtimeTickets?
                .Where(ticket => ticket.Seats.Any(seat => request.Seats
                    .Any(requestSeat => requestSeat.Row == seat.Row && requestSeat.SeatNumber == seat.SeatNumber)))
                .ToList();

            if (reservedOrPurchasedSeats is null)
                return false;

            return reservedOrPurchasedSeats.Any(ticket =>
                ticket.Paid || ticket.CreatedTime.AddMinutes(10) > DateTime.Now);
        }
    }
}