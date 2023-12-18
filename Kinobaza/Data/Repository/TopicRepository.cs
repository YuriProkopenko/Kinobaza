using Kinobaza.Data.Repository.IRepository;
using Kinobaza.Models;
using Kinobaza.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kinobaza.Data.Repository
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        private readonly KinobazaDbContext _context;
        public TopicRepository(KinobazaDbContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<SelectListItem>? GetSelectList()
        {
            return _context.Topics.Select(g => new SelectListItem
            {
                Text = g.Title,
                Value = g.Id.ToString()
            });
        }

        public void Update(Topic topic)
        {
            _context.Update(topic);
        }
    }
}
