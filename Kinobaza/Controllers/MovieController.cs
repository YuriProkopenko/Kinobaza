using Humanizer.Localisation;
using Kinobaza.Data;
using Kinobaza.Models;
using Kinobaza.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Kinobaza.Controllers
{
    public class MovieController : Controller
    {
        private readonly KinobazaDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MovieController(KinobazaDbContext context, IWebHostEnvironment webHostEnvironment) 
        { 
            _context = context; 
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Movie> movies = await _context.Movies.Include(m => m.Genres).ToListAsync();
            List<MovieVM> moviesVM = new();
            foreach (var movie in movies)
            {
                moviesVM.Add(new MovieVM()
                {
                    MovieId = movie.Id,
                    Image = movie.Image,
                    TitleRu = movie.TitleRU,
                    TitleEn = movie.TitleEN,
                    PremiereDate = movie.PremiereDate
                });
            }
            return View(moviesVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movies == null) return NotFound();

            var movie = await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            var genreSelectList = _context.Genres.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = g.Id.ToString()
            });

            //create movieVM
            var movieVM = new MovieVM() { Items = genreSelectList, PremiereDate = new DateTime(1900, 1, 1) };

            //save no image name
            WC.MovieImageName = null;

            //check if id is null
            if (id == null) return View(movieVM);

            //get movie from db
            var movie = await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == id);

            //check if movie is null
            if (movie == null) return NotFound();

            //save image name
            WC.MovieImageName = movie.Image;

            //fill movieVM with parametres from selected movie
            movieVM.MovieId = movie.Id;
            movieVM.Image = movie.Image;
            movieVM.TempTitle = movie.TitleRU;
            movieVM.TitleRu = movie.TitleRU;
            movieVM.TitleEn = movie.TitleEN;
            movieVM.PremiereDate = movie.PremiereDate ?? new DateTime(1900, 1, 1);
            movieVM.Genres = movie.Genres?.Select(g => g.Id.ToString());
            movieVM.Director = movie.Director;
            movieVM.Cast = movie.Cast;
            movieVM.Description = movie.Description;

            return View(movieVM);
        }

        [HttpPost, ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertConfirmed(MovieVM movieVM)
        {
            if (movieVM == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    #region save image to db
                    //find a path to an image directory
                    var webRootPath = _webHostEnvironment.WebRootPath;
                    var upload = webRootPath + WC.MovieImagePath;

                    //checking for uploaded image and save it
                    var files = HttpContext.Request.Form.Files;

                    var newImagePath = string.Empty;

                    //define create or update
                    if(files.Count > 0)
                    {
                        //get new file name
                        var fileName = Guid.NewGuid().ToString();
                        var extension = Path.GetExtension(files[0].FileName);
                        fileName += extension;

                        //save new image to db
                        var imagePath = Path.Combine(upload, fileName);
                        using (var fileStream = new FileStream(imagePath, FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        //if image exists, add new image path
                        newImagePath = WC.MovieImagePath + fileName;

                    }
                    #endregion

                    var genresId = movieVM?.Genres?.Select(g => int.Parse(g)).ToList();
                    var genres = await _context.Genres.Where(g => genresId!.Any(y => y == g.Id)).ToListAsync();

                    var movie = new Movie()
                    {
                        TitleRU = movieVM?.TitleRu,
                        TitleEN = movieVM?.TitleEn,
                        PremiereDate = movieVM?.PremiereDate ?? new DateTime(1900, 1, 1),
                        Genres = genres,
                        Director = movieVM?.Director,
                        Cast = movieVM?.Cast,
                        Description = movieVM?.Description,
                    };

                    //cheking for existence of movie
                    var temp = await _context.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == movieVM.MovieId);

                    //create and adding to db a new movie
                    if (temp is null)
                    {
                        movie.Image = newImagePath;

                        //save image name
                        WC.MovieImageName = movie.Image;

                        //adding to db a new movie
                        await _context.Movies.AddAsync(movie);
                    }
                    //update db with old movie
                    else
                    {
                        //delete an old image if exists and set a new image path to movie
                        if (files.Count > 0)
                        {
                            var oldFile = webRootPath + temp.Image;
                            if (System.IO.File.Exists(oldFile)) System.IO.File.Delete(oldFile);
                            temp.Image = newImagePath;

                            //save image name
                            WC.MovieImageName = movie.Image;
                        }

                        //change properties in the movie
                        temp.TitleRU = movie.TitleRU;
                        temp.TitleEN = movie.TitleEN;
                        temp.PremiereDate = movie.PremiereDate ?? new DateTime(1900, 1, 1);
                        temp.Genres = movie.Genres;
                        temp.Director = movie.Director;
                        temp.Cast = movie.Cast;
                        temp.Description = movie.Description;

                        //update an old movie
                        _context.Movies.Update(temp);
                    }

                    //save progress to db and return
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch { throw; }
            }
            //if model is not valid return to view with same movie
            var genreSelectList = _context.Genres.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = g.Id.ToString()
            });
            movieVM.Items = genreSelectList;
            movieVM.Image = WC.MovieImageName;
            return View(movieVM);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movies == null) return NotFound();

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null || _context.Movies == null) return NotFound();
            try
            {
                var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
                if (movie == null) return NotFound();

                //find a path to an image directory
                var webRootPath = _webHostEnvironment.WebRootPath;

                //delete an old image if exists
                var oldFile = webRootPath + movie.Image;
                if (System.IO.File.Exists(oldFile)) System.IO.File.Delete(oldFile);

                //delete movie and save it to  db
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
            catch { throw; }
            return RedirectToAction(nameof(Index));
        }
    }
}
