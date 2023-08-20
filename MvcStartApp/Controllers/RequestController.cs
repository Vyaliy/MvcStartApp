using Microsoft.AspNetCore.Mvc;
using MvcStartApp.Models.DB;
using MvcStartApp.Models.Repositories;
using System.Threading.Tasks;

namespace MvcStartApp.Controllers;

public class RequestController : Controller
{
    private readonly IRequestRepository _repo;

    public RequestController(IRequestRepository repo)
    {
        _repo = repo;
    }

    public async Task<IActionResult> Index()
    {
        var requests = await _repo.GetRequests();
        return View(requests);
    }
}
