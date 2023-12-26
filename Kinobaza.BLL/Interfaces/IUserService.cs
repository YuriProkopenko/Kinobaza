using Kinobaza.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinobaza.BLL.Interfaces
{
    public interface IUserService
    {
        Task RegisterUser(UserDTO userDTO);
        Task UpdateUser(UserDTO userDTO);
        Task DeleteUser(int id);
        Task<UserDTO> GetUserById(int id);
        Task<UserDTO> GetUserByLogin(string login);
        Task<string> GetUserStatus(string login);
        Task<IEnumerable<UserDTO>> GetUsersByLoginPart(string loginPart);
        Task<bool> IsUserExists(string login);
        Task<bool> IsPasswordCorrect(string login, string password);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<IEnumerable<UserDTO>?> GetAllWithoutAdmin();
    }
}
