using System;
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
    public class ShowtimeService : IShowtimeService
    {
        private readonly IShowtimesRepository _showtimesRepository;
        private readonly IAuditoriumService _auditoriumService;

        public ShowtimeService(IShowtimesRepository showtimesRepository, IAuditoriumService auditoriumService)
        {
            _showtimesRepository = showtimesRepository;
            _auditoriumService = auditoriumService;
        }

        public async Task<ShowtimeResponseDto> CreateShowtime(ShowtimeCreationRequestDto request,
            MovieEntity movie)
        {
            try
            {
                if (movie is null)
                    throw new ShowtimeCreationErrorException("Movie cannot be null.");

                var isAuditoriumAvailable =
                    await _auditoriumService.IsAuditoriumAvailable(request.AuditoriumId, request.SessionDate);

                if (!isAuditoriumAvailable)
                    throw new ShowtimeCreationErrorException("Auditorium is not available.");

                var showtimeToSave = request.ToShowtimeEntity(movie);

                var showtime = await _showtimesRepository.CreateShowtime(showtimeToSave, new CancellationToken());

                if (showtime is null)
                    throw new ShowtimeCreationErrorException("Error on saving showtime in database.");

                return showtime.ToShowtimeResponseDto();
            }
            catch (Exception e)
            {
                throw new ShowtimeCreationErrorException($"Error creating showtime: {e.Message}");
            }
        }
    }
}