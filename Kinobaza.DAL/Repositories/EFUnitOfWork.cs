using Kinobaza.DAL.EF;
using Kinobaza.DAL.Interfaces;
using Kinobaza.DAL.Entities;

namespace Kinobaza.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly KinobazaDbContext _dbContext;
        private Repository<Genre>? _genreRepository;
        private Repository<Movie>? _movieRepository;
        private Repository<User>? _userRepository;
        private Repository<Topic>? _topicRepository;
        private Repository<Record>? _recordRepository;

        public EFUnitOfWork(KinobazaDbContext dbContext) => _dbContext = dbContext;

        public IRepository<Genre> Genres
        {
            get => _genreRepository ??= new Repository<Genre>(_dbContext);
        }

        public IRepository<Movie> Movies
        {
            get => _movieRepository ??= new Repository<Movie>(_dbContext);
        }

        public IRepository<User> Users
        {
            get => _userRepository ??= new Repository<User>(_dbContext);
        }

        public IRepository<Topic> Topics
        {
            get => _topicRepository ??= new Repository<Topic>(_dbContext);
        }

        public IRepository<Record> Records
        {
            get => _recordRepository ??= new Repository<Record>(_dbContext);
        }
    }
}
