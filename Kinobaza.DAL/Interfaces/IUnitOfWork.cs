using Kinobaza.DAL.Entities;

namespace Kinobaza.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Genre> Genres { get; }
        IRepository<Movie> Movies { get; }
        IRepository<User> Users { get; }
        IRepository<Topic> Topics { get; }
        IRepository<Record> Records { get; }
    }
}
