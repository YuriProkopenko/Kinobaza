using AutoMapper;
using Kinobaza.BLL.DTO;
using Kinobaza.BLL.Interfaces;
using Kinobaza.DAL.Entities;
using Kinobaza.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Kinobaza.BLL.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _uow;

        public GenreService(IUnitOfWork uow) => _uow = uow;

        public async Task CreateGenre(GenreDTO genreDTO)
        {
            var genre = new Genre
            {
                Name = genreDTO.Name
            };
            await _uow.Genres.AddAsync(genre);
            await _uow.Genres.SaveAsync();
        }

        public async Task DeleteGenre(int id)
        {
            await _uow.Genres.Delete(id);
            await _uow.Genres.SaveAsync();
        }

        public async Task<IEnumerable<GenreDTO>> GetAllGenres()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Genre, GenreDTO>()
                .ForMember("MoviesNames", opt => opt.MapFrom(g => g.Movies.Select(m => m.TitleRu))));
            var mapper = new Mapper(config);
            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreDTO>>(await _uow.Genres.GetAllAsync());
        }

        public async Task<GenreDTO> GetGenreById(int id)
        {
            var genre = await _uow.Genres.FirstOrDefaultAsync(g => g.Id == id, includeProperties: "Movies") ?? throw new ValidationException("Wrong genre!");
            return new GenreDTO
            {
                Id = genre.Id,
                Name = genre.Name,
                MoviesNames = genre?.Movies?.Select(m => m.TitleRu)
            };
        }

        public async Task UpdateGenre(GenreDTO genreDTO)
        {
            var genre = new Genre
            {
                Id= genreDTO.Id,
                Name = genreDTO.Name
            };
            _uow.Genres.Update(genre);
            await _uow.Genres.SaveAsync();
        }
    }
}
