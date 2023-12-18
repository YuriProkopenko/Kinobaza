using Kinobaza.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kinobaza.Data.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User user);

        IEnumerable<SelectListItem>? GetSelectList();

    }
}
