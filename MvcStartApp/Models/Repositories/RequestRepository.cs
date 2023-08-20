using Microsoft.EntityFrameworkCore;
using MvcStartApp.Models.AppContext;
using MvcStartApp.Models.DB;

namespace MvcStartApp.Models.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        // ссылка на контекст
        private readonly BlogContext _context;

        public RequestRepository(BlogContext context) 
        {
            _context = context;
        }

        public async Task AddRequest(string url)
        {
            var request = new Request
            {
                Date = DateTime.Now,
                Id = Guid.NewGuid(),
                Url = url
            };
            _context.Requests.Add(request);

            // Сохранение изенений
            await _context.SaveChangesAsync();
        }

        public async Task<Request[]> GetRequests()
        {
            // Получим все запросы
            return await _context.Requests.ToArrayAsync();
        }
    }
}
