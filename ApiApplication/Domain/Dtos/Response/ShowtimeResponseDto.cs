using System;

namespace ApiApplication.Domain.Dtos.Response
{
    public class ShowtimeResponseDto
    {
        public int Id { get; set; }
        public MovieDto Movie { get; set; }
        public DateTime SessionDate { get; set; }
        public int AuditoriumId { get; set; }
    }
}