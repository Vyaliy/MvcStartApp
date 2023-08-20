using MvcStartApp.Models.DB;

namespace MvcStartApp.Models.Repositories;

public interface IBlogRepository
{
    Task AddUser(User user);
    Task<User[]> GetUsers();
}
