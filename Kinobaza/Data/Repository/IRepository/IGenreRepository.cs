using Kinobaza.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kinobaza.Data.Repository.IRepository
{
    public interface IGenreRepository : IRepository<Genre>
    {
        void Update(Genre genre);

        IEnumerable<SelectListItem>? GetSelectList();
    }
}
