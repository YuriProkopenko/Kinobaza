using Kinobaza.Data.Repository.IRepository;
using Kinobaza.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kinobaza.Data.Repository
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        private readonly KinobazaDbContext _context;
        public GenreRepository(KinobazaDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Genre genre)
        {
            _context.Update(genre);
        }

        public IEnumerable<SelectListItem>? GetSelectList()
        {
            return _context.Genres.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = g.Id.ToString()
            });
        }

    }
}
