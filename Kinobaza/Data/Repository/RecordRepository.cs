using Kinobaza.Data.Repository.IRepository;
using Kinobaza.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kinobaza.Data.Repository
{
    public class RecordRepository : Repository<Record>, IRecordRepository
    {
        private readonly KinobazaDbContext _context;
        public RecordRepository(KinobazaDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem>? GetSelectList()
        {
            throw new NotImplementedException();
        }

        public void Update(Record record)
        {
            throw new NotImplementedException();
        }
    }
}
