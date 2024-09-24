using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Domain.Exceptions;
using ApiApplication.Domain.Services.Interfaces;

namespace ApiApplication.Domain.Services
{
    public class AuditoriumService : IAuditoriumService
    {
        private readonly IAuditoriumsRepository _auditoriumsRepository;

        public AuditoriumService(IAuditoriumsRepository auditoriumsRepository)
        {
            _auditoriumsRepository = auditoriumsRepository;
        }

        public async Task<bool> IsAuditoriumAvailable(int id, DateTime date)
        {
            var auditorium = await _auditoriumsRepository.GetAsync(id, new CancellationToken());

            if (auditorium == null)
                throw new AuditoriumNotFoundException("Auditorium not found.");
            
            var checkIfDateIsAvailable = auditorium.Showtimes != null &&
                                         !auditorium.Showtimes.Select(s => s.SessionDate).Contains(date);

            return checkIfDateIsAvailable;
        }

        public async Task<bool> AreSeatsInRangeOfAuditorium(int id, List<SeatEntity> seats)
        {
            var auditorium = await _auditoriumsRepository.GetAsync(id, new CancellationToken());

            if (auditorium == null)
                throw new AuditoriumNotFoundException("Auditorium not found.");

            return seats.TrueForAll(s =>
                auditorium.Seats.Any(ads => ads.SeatNumber == s.SeatNumber & ads.Row == s.Row));
        }

        public async Task<IEnumerable<SeatEntity>> GetSeatsForTicket(int id, List<SeatEntity> seats)
        {
            var auditorium = await _auditoriumsRepository.GetAsync(id, new CancellationToken());

            if (auditorium == null)
                throw new AuditoriumNotFoundException("Auditorium not found.");
            
            var auditoriumSeats = auditorium.Seats?.Where(seat =>
                seats.Any(sr => sr.SeatNumber == seat.SeatNumber && sr.Row == seat.Row));
            
            return auditoriumSeats;
        }
    }
}