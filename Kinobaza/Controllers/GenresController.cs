using AutoMapper;
using Kinobaza.BLL.DTO;
using Kinobaza.BLL.Interfaces;
using Kinobaza.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace Kinobaza.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreService _genreServ ;
        public GenresController(IGenreService genreServ) => _genreServ = genreServ;

        // GET: Genres
        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                //check authorization
                if (HttpContext.Session.GetString("login") != "admin") return NotFound();

                //create view model
                IEnumerable<GenreVM> genreVMs = new List<GenreVM>();

                //mapping genres view models
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<GenreDTO, GenreVM>()).CreateMapper();
                genreVMs = mapper.Map<IEnumerable<GenreDTO>, IEnumerable<GenreVM>>(await _genreServ.GetAllGenres());

                return View(genreVMs);
            }
            catch { return NotFound(); }
        }

        // GET: Genres/Upsert/id
        [HttpGet]
        public async Task<IActionResult> Upsert(int? id) 
        {
            try
            {
                //check authorization
                if (HttpContext.Session.GetString("login") != "admin") return NotFound();

                //create view model
                GenreVM genreVM = new();

                //if create
                if (id is null) return View(genreVM);

                //if update
                var genreDTO = await _genreServ.GetGenreById((int)id);
                //check if genre is not found
                if (genreDTO is null) return NotFound();
                //fill genre view model
                genreVM.Id = genreDTO.Id;
                genreVM.Name = genreDTO.Name;
                genreVM.MoviesNames = genreDTO.MoviesNames;
                genreVM.TitleName = genreDTO.Name;
                return View(genreVM);
            }
            catch { return NotFound(); }
        }

        // POST: Genres/Upsert/id
        [HttpPost, ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertConfirmed(GenreVM genreVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //if create
                    if (genreVM.Id == 0)
                        await _genreServ.CreateGenre(new GenreDTO { Name = genreVM.Name });
                    //if update
                    else
                    {
                        await _genreServ.UpdateGenre(new GenreDTO { Id = genreVM.Id, Name = genreVM.Name });
                    }

                    // Genres/Index
                    return RedirectToAction(nameof(List));
                }
                // Genres/Upsert
                return View(genreVM);
            }
            catch { return NotFound(); }
        }

        // GET: Genres/Delete/id
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                //check authorization
                if (HttpContext.Session.GetString("login") != "admin") return NotFound();

                //check if id is null
                if (id is null) return NotFound();

                //find a genre data by id
                var genreDTO = await _genreServ.GetGenreById((int)id);

                //check if genre data is not found
                if (genreDTO is null) return NotFound();

                //get fill view model by genre data
                var genreVM = new GenreVM
                {
                    Id = genreDTO.Id,
                    Name = genreDTO.Name,
                    MoviesNames = genreDTO.MoviesNames
                };

                return View(genreVM);
            }
            catch { return NotFound(); }
        }

        // POST: Genres/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                //check if id is null
                if(id is null) return NotFound();

                //check if genre data is null
                var genreDTO = await _genreServ.GetGenreById((int)id);
                if (genreDTO is null) return NotFound();

                //check if genre is not related with any movies and delete it
                if (genreDTO?.MoviesNames?.Count() == 0)
                    await _genreServ.DeleteGenre((int)id);

                return RedirectToAction(nameof(List));
            }
            catch { return NotFound(); }
        }
    }
}
