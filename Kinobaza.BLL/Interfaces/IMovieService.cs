using Kinobaza.BLL.DTO;

namespace Kinobaza.BLL.Interfaces
{
    public interface IMovieService
    {
        Task CreateMovie(MovieDTO movieDTO);
        Task UpdateMovie(MovieDTO movieDTO);
        Task DeleteMovie(int id);
        Task<MovieDTO> GetMovieById(int id);
        Task<IEnumerable<MovieDTO>> GetAllMovies();
    }
}
