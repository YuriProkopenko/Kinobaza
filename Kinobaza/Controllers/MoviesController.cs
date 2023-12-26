using AutoMapper;
using Kinobaza.BLL.DTO;
using Kinobaza.BLL.Interfaces;
using Kinobaza.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kinobaza.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieServ;
        private readonly IGenreService _genreServ;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MoviesController(IMovieService movieServ, IGenreService genreServ, IWebHostEnvironment webHostEnvironment)
        {
            _movieServ = movieServ;
            _genreServ = genreServ;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Movies
        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                //check authorization
                if(HttpContext.Session.GetString("login") != "admin") return NotFound();

                //create view model
                IEnumerable<MovieVM> movieVMs = new List<MovieVM>();

                //mapping movies view models
                IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<MovieDTO, MovieVM>()).CreateMapper();
                movieVMs = mapper.Map<IEnumerable<MovieDTO>, IEnumerable<MovieVM>>(await _movieServ.GetAllMovies());

                return View(movieVMs);
            }
            catch { return NotFound(); }
        }

        // GET: Movies/Details/id
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                //check if id is null
                if (id is null) return NotFound();
                var movieData = await _movieServ.GetMovieById((int)id);

                //check if movie is not found
                if (movieData is null) return NotFound();

                MovieVM movieVM = new()
                {
                    TitleEn = movieData.TitleEn,
                    TitleRu = movieData.TitleRu,
                    Description = movieData.Description,
                    Director = movieData.Director,
                    Cast = movieData.Cast,
                    PremiereDate = movieData.PremiereDate,
                    Poster = movieData.Poster,
                    GenresNames = movieData.GenresNames?.ToList()
                };

                return View(movieVM);
            }
            catch { return NotFound(); }
        }

        // GET: Movies/Upsert/id
        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            try
            {
                //check authorization
                if (HttpContext.Session.GetString("login") != "admin") return NotFound();

                //get all genres select list
                var genresDataList = await _genreServ.GetAllGenres();
                var genresSelectList = genresDataList.Select(g => new SelectListItem
                {
                    Text = g.Name,
                    Value = g.Id.ToString()
                });

                //create view model
                MovieVM movieVM = new()
                {
                    Items = genresSelectList,
                    PremiereDate = new DateTime(1900, 1, 1)
                };

                //if create
                if (id is null) return View(movieVM);

                //if update
                var movieData = await _movieServ.GetMovieById((int)id);
                //check if movie is not found
                if (movieData is null) return NotFound();
                //fill movie view model
                movieVM.Id = movieData.Id;
                movieVM.Poster = movieData.Poster;
                movieVM.TitleRu = movieData.TitleRu;
                movieVM.TitleEn = movieData.TitleEn;
                movieVM.PremiereDate = movieData.PremiereDate ?? new DateTime(1900, 1, 1);
                movieVM.GenresIds = movieData.GenresIds?.ToList();
                movieVM.Director = movieData.Director;
                movieVM.Cast = movieData.Cast;
                movieVM.Description = movieData.Description;
                return View(movieVM);
            }
            catch { return NotFound(); }
        }

        // POST: Movies/Upsert/id
        [HttpPost, ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertConfirmed(MovieVM movieVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //upload poster and remove old poster
                    var files = HttpContext.Request.Form.Files;
                    string savedPosterPath = movieVM.Poster ?? string.Empty;
                    if (files.Count > 0)
                    {
                        savedPosterPath = await UploadPoster(_webHostEnvironment, files[0]);

                        //if old poster exists remove it
                        if (movieVM.Id != 0)
                        {
                            var oldMovieData = await _movieServ.GetMovieById(movieVM.Id);
                            var oldFile = _webHostEnvironment.WebRootPath + oldMovieData.Poster;
                            if (System.IO.File.Exists(oldFile)) System.IO.File.Delete(oldFile);
                        }
                    }

                    //create movie DTO
                    var movieDTO = new MovieDTO
                    {
                        Id = movieVM.Id,
                        TitleRu = movieVM?.TitleRu,
                        TitleEn = movieVM?.TitleEn,
                        Poster = savedPosterPath,
                        PremiereDate = movieVM?.PremiereDate ?? new DateTime(1900, 1, 1),
                        GenresIds = movieVM?.GenresIds,
                        Director = movieVM?.Director,
                        Cast = movieVM?.Cast,
                        Description = movieVM?.Description,
                    };

                    //if create
                    if (movieVM.Id == 0)
                        await _movieServ.CreateMovie(movieDTO);
                    //if update
                    else
                        await _movieServ.UpdateMovie(movieDTO);

                    // Movies/MoviesList
                    return RedirectToAction(nameof(List));
                }

                //get all genres select list
                var genresDataList = await _genreServ.GetAllGenres();
                var genresSelectList = genresDataList.Select(g => new SelectListItem
                {
                    Text = g.Name,
                    Value = g.Id.ToString()
                });
                movieVM.Items = genresSelectList;
                return View(movieVM);
            }
            catch { throw; }
        }

        // GET: Movies/Delete/id
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                //check authorization
                if (HttpContext.Session.GetString("login") != "admin") return NotFound();

                //check if id is null
                if (id is null) return NotFound();

                //get a movie dto by id
                var movieDTO= await _movieServ.GetMovieById((int)id);

                //check if movie is not found
                if (movieDTO is null) return NotFound();

                //create view model
                var movieVM = new MovieVM
                {
                    Id = movieDTO.Id,
                    TitleRu = movieDTO.TitleRu
                };

                return View(movieVM);
            }
            catch { return NotFound(); }
        }
        // POST: Movies/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                //check if id is null
                if (id is null) return NotFound();

                //check if movie dto is exists
                var movieDTO = await _movieServ.GetMovieById((int)id);
                if (movieDTO is null) return NotFound();

                //if old poster exists remove it
                var oldFile = _webHostEnvironment.WebRootPath + movieDTO.Poster;
                if (System.IO.File.Exists(oldFile)) System.IO.File.Delete(oldFile);

                //delete movie
                await _movieServ.DeleteMovie((int)id);
            }
            catch { throw; }
            return RedirectToAction(nameof(List));
        }

        private static async Task<string> UploadPoster(IWebHostEnvironment whEnv, IFormFile file)
        {
            try
            {
                //get new file name
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(file.FileName);
                var filePath = @"\images\posters\" + fileName + extension;

                //save new image to storage
                var uploadPath = whEnv.WebRootPath + filePath;
                using var fileStream = new FileStream(uploadPath, FileMode.Create);
                await file.CopyToAsync(fileStream);

                return filePath;
            }
            catch { throw; }
        }
    }
}
