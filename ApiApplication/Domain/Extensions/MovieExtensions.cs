using System;
using ApiApplication.Database.Entities;
using ApiApplication.Domain.Dtos;
using ProtoDefinitions;

namespace ApiApplication.Domain.Extensions
{
    public static class MovieExtensions
    {
        public static MovieEntity ToMovieEntity(this showResponse movie) => new MovieEntity()
        {
            Title = movie.Title,
            ImdbId = movie.Id,
            Stars = movie.ImDbRating,
            ReleaseDate = DateTime.Parse($"{movie.Year}-01-01"),
        };

        public static MovieDto ToDto(this MovieEntity movie) => new MovieDto()
        {
            Stars = movie.Stars,
            ReleaseDate = movie.ReleaseDate,
            Title = movie.Title,
            ImdbId = movie.ImdbId,
        };
    }
}