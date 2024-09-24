using System;

namespace ApiApplication.Domain.Dtos
{
    public class MovieDto
    {
        public string Title { get; set; }
        public string ImdbId { get; set; }
        public string Stars { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}