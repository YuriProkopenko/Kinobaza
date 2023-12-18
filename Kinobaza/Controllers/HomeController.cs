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

            //check authorization and get user login from cookies
            var login = HttpContext.Session.GetString("login");
            var cookiesLogin = Request.Cookies["login"];
            if (cookiesLogin is not null && login is null)
            {
                HttpContext.Session.SetString("login", Request.Cookies["login"]);
                WC.UserLogin = cookiesLogin;
            }

            IEnumerable<Movie> movies = await Task.Run(() => _context.Movies.Include(m => m.Genres).ToListAsync());
            return View(movies);
        }
    }
}