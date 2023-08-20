using Microsoft.EntityFrameworkCore;
using MvcStartApp.Models.AppContext;
using MvcStartApp.Models.DB;

namespace MvcStartApp.Models.Repositories;

public class BlogRepository : IBlogRepository
{
    // ссылка на контекст
    private readonly BlogContext _context;

    // Метод-конструктор для инициализации
    public BlogRepository(BlogContext context)
    {
        _context = context;
    }

    public async Task<User[]> GetUsers()
    {
        // Получим всех активных пользователей
        return await _context.Users.ToArrayAsync();
    }

    public async Task AddUser(User user)
    {
        user.JoinDate = DateTime.Now;
        user.Id = Guid.NewGuid();

        // Добавление пользователя
        var entry = _context.Entry(user);
        if (entry.State == EntityState.Detached)
            await _context.Users.AddAsync(user);

        // Сохранение изенений
        await _context.SaveChangesAsync();
    }
}
