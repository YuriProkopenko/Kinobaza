using Kinobaza.DAL.EF;
using Kinobaza.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kinobaza.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly KinobazaDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(KinobazaDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task AddAsync(T item)
        {
            await _dbSet.AddAsync(item); 
        }

        public async Task Delete(int id)
        {
            var item = await _context.FindAsync<T>(id);
            if(item is not null) _dbSet.Remove(item); 
        }

        public async Task<T?> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> querry = _dbSet;

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

        public void Update(T item)
        {
            _context.Update(item);
        }

        public async Task<T?> FindAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> querry = _dbSet;

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
