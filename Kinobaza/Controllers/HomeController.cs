using AutoMapper;
using Kinobaza.BLL.DTO;
using Kinobaza.BLL.Interfaces;
using Kinobaza.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kinobaza.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movieServ;
        public HomeController(IMovieService movieServ) => _movieServ = movieServ;

        public async Task<IActionResult> Index()
        {
            try
            {
                //check authorization
                CheckAuthorization(HttpContext);

                //mapping movies view models
                IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<MovieDTO, MovieVM>()).CreateMapper();
                IEnumerable<MovieVM> movieVMs = mapper.Map<IEnumerable<MovieDTO>, IEnumerable<MovieVM>>(await _movieServ.GetAllMovies());

                return View(movieVMs);
            }
            catch { return NotFound(); }
        }

        private static void CheckAuthorization(HttpContext httpContext)
        {
            var login = httpContext.Session.GetString("login");
            var cookiesLogin = httpContext.Request.Cookies["login"];
            if (cookiesLogin is not null && login is null)
            {
                httpContext.Session.SetString("login", httpContext.Request.Cookies["login"]);
            }
        }
    }
}