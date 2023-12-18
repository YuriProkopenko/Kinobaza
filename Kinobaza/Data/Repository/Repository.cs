using Kinobaza.Data.Repository.IRepository;
using Kinobaza.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kinobaza.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly KinobazaDbContext _context;
        internal DbSet<T> dbSet;

        public Repository(KinobazaDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
        public async Task AddAsync(T item)
        {
            await dbSet.AddAsync(item); 
        }

        public void Remove(T item)
        {
            dbSet.Remove(item); 
        }

        public async Task<T?> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> querry = dbSet;

            //filter
            if (filter is not null)
                querry = querry.Where(filter);

            //include
            if (includeProperties is not null)
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    querry = querry.Include(property);

            //isTracking
            if (!isTracking)
                querry = querry.AsNoTracking();

            return await querry.FirstOrDefaultAsync();
        }

        public async Task<T?> FindAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> querry = dbSet;

            //filter
            if(filter is not null)
                querry = querry.Where(filter);

            //include
            if(includeProperties is not null)
                foreach(var property in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                    querry = querry.Include(property);

            //orderBy
            if(orderBy is not null)
                querry = orderBy(querry);

            //isTracking
            if(!isTracking)
                querry = querry.AsNoTracking();

            return await querry.ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
