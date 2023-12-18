using Kinobaza.Data;
using Kinobaza.Data.Repository.IRepository;
using Kinobaza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Kinobaza.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieRepository _movieRepo;
        public HomeController(IMovieRepository movieRepo) => _movieRepo = movieRepo;

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

            var movies = await _movieRepo.GetAllAsync(includeProperties: "Genres");
            return View(movies);
        }
    }
}