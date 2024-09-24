using System;
using System.Text.Json;
using System.Threading.Tasks;
using ApiApplication.Database.Entities;
using ApiApplication.Domain.Exceptions;
using ApiApplication.Domain.Extensions;
using ApiApplication.Domain.Services.Interfaces;
using ApiApplication.Infra.GrpcServices.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using ProtoDefinitions;

namespace ApiApplication.Domain.Services
{
    public class MovieService : IMovieService
    {
        private readonly IApiClientGrpc _clientGrpc;
        private readonly IDistributedCache _redisCache;

        public MovieService(IApiClientGrpc clientGrpc, IDistributedCache redisCache)
        {
            _clientGrpc = clientGrpc;
            _redisCache = redisCache;
        }

        public async Task<MovieEntity> GetMovieById(string id)
        {
            try
            {
                var movie = await _clientGrpc.GetById(id);

                if (movie == null)
                    return null;

                return movie.ToMovieEntity();
            }
            catch (Exception e)
            {
                throw new MovieErrorException($"Error searching movie: {e.Message}");
            }
        }
        
        private async Task<showResponse> GetMovieByGrpcOrCache(string movieId)
        {
            var cacheKey = $"movie_{movieId}";
            showResponse movie = null;

            try
            {
                movie = await _clientGrpc.GetById(movieId);

                if (movie != null)
                {
                    var movieJson = JsonSerializer.Serialize(movie);
                    await _redisCache.SetStringAsync(cacheKey, movieJson, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                    });
                }
            }
            catch (Exception ex)
            {
                var cachedMovie = await _redisCache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedMovie))
                    movie = JsonSerializer.Deserialize<showResponse>(cachedMovie);
            }

            return movie;
        }
        
    }
}