using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiApplication.Database.Entities;

namespace ApiApplication.Domain.Services.Interfaces
{
    public interface IAuditoriumService
    {
        Task<bool> IsAuditoriumAvailable(int id, DateTime date);
        
        Task<bool> AreSeatsInRangeOfAuditorium(int id, List<SeatEntity> seats);

        Task<IEnumerable<SeatEntity>> GetSeatsForTicket(int id, List<SeatEntity> seats);

    }
}