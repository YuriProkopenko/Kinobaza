using Kinobaza.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kinobaza.Data.Repository.IRepository
{
    public interface ITopicRepository : IRepository<Topic>
    {
        void Update(Topic topic);

        IEnumerable<SelectListItem>? GetSelectList();
    }
}
