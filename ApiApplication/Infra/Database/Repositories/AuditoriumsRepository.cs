using ApiApplication.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using ApiApplication.Database.Repositories.Abstractions;

namespace ApiApplication.Database.Repositories
{
    public class AuditoriumsRepository : IAuditoriumsRepository
    {
        private readonly CinemaContext _context;

        public AuditoriumsRepository(CinemaContext context)
        {
            _context = context;
        }

        public async Task<AuditoriumEntity> GetAsync(int auditoriumId, CancellationToken cancel,
            bool asNoTracking = false)
        {
            if (asNoTracking)
                return await _context.Auditoriums.AsNoTracking()
                    .Include(x => x.Seats)
                    .Include(x => x.Showtimes)
                    .FirstOrDefaultAsync(x => x.Id == auditoriumId, cancel);

            return await _context.Auditoriums
                .Include(x => x.Seats)
                .Include(x => x.Showtimes)
                .FirstOrDefaultAsync(x => x.Id == auditoriumId, cancel);
        }
    }
}