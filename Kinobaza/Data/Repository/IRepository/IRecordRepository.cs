using Kinobaza.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kinobaza.Data.Repository.IRepository
{
    public interface IRecordRepository : IRepository<Record>
    {
        void Update(Record record);

        IEnumerable<SelectListItem>? GetSelectList();
    }
}
