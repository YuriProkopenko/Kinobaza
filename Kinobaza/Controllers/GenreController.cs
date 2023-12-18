using Kinobaza.Data;
using Kinobaza.Models;
using Kinobaza.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Kinobaza.Controllers
{
    public class GenreController : Controller
    {
        private readonly KinobazaDbContext _context;
        public GenreController(KinobazaDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //get all genres
            IEnumerable<Genre> genres = await _context.Genres.ToListAsync();

            //get list of genre view model
            var genreVMs = new List<GenreVM>();
            foreach(var genre in genres)
            {
                genreVMs.Add(new GenreVM
                {
                    Id = genre.Id,
                    Name = genre.Name,
                });
            }

            return View(genreVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id) 
        {
            var genreVM = new GenreVM();

            if (id == null) return View(genreVM);

            var genre = await _context.Genres.Include(g => g.Movies).FirstOrDefaultAsync(m => m.Id == id);

            if (genre == null) return NotFound();

            //fill genre view model
            genreVM.Id = genre.Id;
            genreVM.Name = genre.Name;
            genreVM.MoviesNames = genre.Movies?.Select(m => m.TitleRU);
            return View(genreVM);
        }

        [HttpPost, ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertConfirmed(GenreVM genreVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var temp = await _context.Genres.AsNoTracking().FirstOrDefaultAsync(m => m.Id == genreVM.Id);
                    if (temp == null)
                        _context.Genres.Add(new Genre { Name = genreVM.Name });
                    else
                    {
                        temp.Name = genreVM.Name;
                        _context.Genres.Update(temp);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch { throw; }
            }
            return View(genreVM);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            //check if id or genres is null
            if (id == null || _context.Genres == null) return NotFound();

            //find a genre by id and list of movies releated on it 
            var genre = await _context.Genres.Include(g => g.Movies).FirstOrDefaultAsync(m => m.Id == id);

            //check if genre is not found
            if (genre == null) return NotFound();

            //get fill view model by genre
            var genreVM = new GenreVM 
            { 
                Id = genre.Id,
                Name = genre.Name,
                MoviesNames = genre?.Movies?.Select(m => m.TitleRU).ToList()
            };

            return View(genreVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if(id == null || _context.Genres == null) return NotFound();
            try
            {
                //check if id or genre is null
                var genre = await _context.Genres.Include(g => g.Movies).FirstOrDefaultAsync(m => m.Id == id);

                if (genre == null) return NotFound();

                //if genre is not related with any movies
                if(genre.Movies is null || genre!.Movies?.Count == 0)
                    _context.Genres.Remove(genre);
                else return NotFound();

                await _context.SaveChangesAsync();
            }
            catch { throw; }

            return RedirectToAction(nameof(Index));
        }
    }
}
