using ApiApplication.Database.Entities;
using ApiApplication.Domain.Dtos;
using ApiApplication.Domain.Dtos.Response;

namespace ApiApplication.Domain.Extensions
{
    public static class ShowtimeDtosExtensions
    {
        public static ShowtimeEntity ToShowtimeEntity(this ShowtimeCreationRequestDto showtimeDto, MovieEntity movie) =>
            new ShowtimeEntity()
            {
                Movie = movie,
                AuditoriumId = showtimeDto.AuditoriumId,
                SessionDate = showtimeDto.SessionDate
            };
        
        public static ShowtimeResponseDto ToShowtimeResponseDto (this ShowtimeEntity showtime) => new ShowtimeResponseDto()
        {
            SessionDate = showtime.SessionDate,
            AuditoriumId = showtime.AuditoriumId,
            Movie = showtime?.Movie?.ToDto() ,
            Id = showtime.Id
        };
    }
}