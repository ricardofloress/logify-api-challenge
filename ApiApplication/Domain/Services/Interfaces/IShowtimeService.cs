using System.Threading.Tasks;
using ApiApplication.Database.Entities;
using ApiApplication.Domain.Dtos;
using ApiApplication.Domain.Dtos.Response;

namespace ApiApplication.Domain.Services.Interfaces
{
    public interface IShowtimeService
    {
        Task<ShowtimeResponseDto> CreateShowtime(ShowtimeCreationRequestDto request, MovieEntity movie);
    }
}