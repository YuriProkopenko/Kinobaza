using Kinobaza.Data.Repository.IRepository;
using Kinobaza.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kinobaza.Data.Repository
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        private readonly KinobazaDbContext _context;
        public MovieRepository(KinobazaDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Movie movie)
        {
            _context.Update(movie);
        }

        public IEnumerable<SelectListItem>? GetSelectList()
        {
            return _context.Movies.Select(g => new SelectListItem
            {
                Text = g.TitleRU,
                Value = g.Id.ToString()
            });
        }
    }
}
