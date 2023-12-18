using Kinobaza.Data.Repository.IRepository;
using Kinobaza.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kinobaza.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly KinobazaDbContext _context;
        public UserRepository(KinobazaDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(User user)
        {
            _context.Update(user);
        }

        public IEnumerable<SelectListItem>? GetSelectList()
        {
            return _context.Users.Select(g => new SelectListItem
            {
                Text = g.Login,
                Value = g.Id.ToString()
            });
        }
    }
}
