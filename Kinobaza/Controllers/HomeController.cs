using Kinobaza.Data;
using Kinobaza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Kinobaza.Controllers
{
    public class HomeController : Controller
    {
        private readonly KinobazaDbContext _context;
        public HomeController(KinobazaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Movie> movies = await Task.Run(() => _context.Movies.Include(m => m.Genres).ToListAsync());
            return View(movies);
        }
    }
}