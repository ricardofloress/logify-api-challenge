using System;
using ApiApplication.Database.Entities;

namespace ApiApplication.Domain.Dtos
{
    public class ShowtimeCreationRequestDto
    {
        public string MovieId { get; set; }

        public DateTime SessionDate { get; set; }
        
        public int AuditoriumId { get; set; }
    }
}