using MvcStartApp.Models.DB;

namespace MvcStartApp.Models.Repositories
{
    public interface IRequestRepository
    {
        Task AddRequest(string Url);
        Task<Request[]> GetRequests();
    }
}