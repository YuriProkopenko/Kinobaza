using AutoMapper;
using Kinobaza.BLL.DTO;
using Kinobaza.BLL.Interfaces;
using Kinobaza.DAL.Entities;
using Kinobaza.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Kinobaza.BLL.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _uow;

        public MovieService(IUnitOfWork uof) => _uow = uof;

        public async Task CreateMovie(MovieDTO movieDTO)
        {
            //get genres list
            IEnumerable<Genre> genres = new List<Genre>();
            var genresIds = movieDTO.GenresIds?.Select(g => int.Parse(g)).ToList();
            if(genresIds is not null)
                genres = await _uow.Genres.GetAllAsync(filter: (g => genresIds!.Any(y => y == g.Id)));

            var movie = new Movie
            {
                TitleEn = movieDTO.TitleEn,
                TitleRu = movieDTO.TitleRu,
                Poster = movieDTO.Poster,
                PremiereDate = movieDTO.PremiereDate,
                Director = movieDTO.Director,
                Cast = movieDTO.Cast,
                Description = movieDTO.Description,
                Genres = genres
            };
            await _uow.Movies.AddAsync(movie);
            await _uow.Movies.SaveAsync();
        }

        public async Task DeleteMovie(int id)
        {
            await _uow.Movies.Delete(id);
            await _uow.Movies.SaveAsync();
        }

        public async Task<IEnumerable<MovieDTO>> GetAllMovies()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Movie, MovieDTO>()
                .ForMember("GenresIds", opt => opt.MapFrom(m => m.Genres.Select(g => g.Id.ToString())))
                .ForMember("GenresNames", opt => opt.MapFrom(m => m.Genres.Select(g => g.Name))));
            var mapper = new Mapper(config);
            return mapper.Map<IEnumerable<Movie>, IEnumerable<MovieDTO>>(await _uow.Movies.GetAllAsync()); 
        }

        public async Task<MovieDTO> GetMovieById(int id)
        {
            var movie = await _uow.Movies.FirstOrDefaultAsync(m => m.Id == id, includeProperties: "Genres") ?? throw new ValidationException("Wrong movie!");
            return new MovieDTO
            {
                Id = movie.Id,
                TitleEn = movie.TitleEn,
                TitleRu = movie.TitleRu,
                Poster = movie.Poster,
                PremiereDate = movie.PremiereDate,
                Director = movie.Director,
                Cast = movie.Cast,
                Description = movie.Description,
                GenresIds = movie.Genres?.Select(g => g.Id.ToString()),
                GenresNames = movie.Genres?.Select(g => g.Name)
            };
        }

        public async Task UpdateMovie(MovieDTO movieDTO)
        {
            //get movie
            var movie = await _uow.Movies.FirstOrDefaultAsync(m => m.Id == movieDTO.Id, includeProperties: "Genres") ?? throw new ValidationException("Wrong movie!");

            //get genres
            IEnumerable<Genre> genres = new List<Genre>();
            var genresIds = movieDTO.GenresIds?.Select(g => int.Parse(g)).ToList();
            if (genresIds is not null)
                genres = await _uow.Genres.GetAllAsync(filter: (g => genresIds!.Any(y => y == g.Id)));

            //update movie
            movie.TitleEn = movieDTO.TitleEn;
            movie.TitleRu = movieDTO.TitleRu;
            movie.Poster = movieDTO.Poster;
            movie.PremiereDate = movieDTO.PremiereDate;
            movie.Director = movieDTO.Director;
            movie.Cast = movieDTO.Cast;
            movie.Description = movieDTO.Description;
            movie.Genres = genres;
            _uow.Movies.Update(movie);

            //save to db
            await _uow.Movies.SaveAsync();
        }
    }
}
