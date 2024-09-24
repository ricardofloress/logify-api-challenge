using System.Threading.Tasks;
using ApiApplication.Database.Entities;
using ProtoDefinitions;

namespace ApiApplication.Domain.Services.Interfaces
{
    public interface IMovieService
    {
        Task<MovieEntity> GetMovieById(string id);

    }
}