using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Linq.Expressions;

namespace Kinobaza.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T?> FindAsync(int id);

        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null,
            bool isTracking = true
            );

        Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            bool isTracking = true
            );

        Task AddAsync( T item );

        void Remove( T item );

        Task SaveAsync();
    }
}
