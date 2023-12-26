using AutoMapper;
using Kinobaza.BLL.DTO;
using Kinobaza.BLL.Interfaces;
using Kinobaza.DAL.Entities;
using Kinobaza.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Kinobaza.BLL.Services
{
    internal class UserService : IUserService
    {

        private readonly IUnitOfWork _uow;

        public UserService(IUnitOfWork uow) => _uow = uow;

        public async Task RegisterUser(UserDTO userDTO)
        {
            //hash user password
            var salt = GetSalt();
            var hash = HashPassword(salt, userDTO.Password);

            var newUser = new User
            {
                Login = userDTO.Login,
                Email = userDTO.Email,
                Password = hash,
                Salt = salt,
                Status = userDTO.Status
            };
            await _uow.Users.AddAsync(newUser);
            await _uow.Users.SaveAsync();
        }

        public async Task DeleteUser(int id)
        {
            await _uow.Users.Delete(id);
            await _uow.Users.SaveAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>());
            var mapper = new Mapper(config);
            return mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(await _uow.Users.GetAllAsync());
        }

        public async Task<IEnumerable<UserDTO>?> GetAllWithoutAdmin()
        {
            var usersDTO = await GetAllUsers();
            return usersDTO?.Where(u => u.Status != "admin");
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _uow.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new ValidationException("Wrong user!");
            return new UserDTO
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                Password = user.Password,
                Salt = user.Salt,
                Status = user.Status
            };
        }

        public async Task<UserDTO> GetUserByLogin(string login)
        {
            var user = await _uow.Users.FirstOrDefaultAsync(u => u.Login == login) ?? throw new ValidationException("Wrong user!");
            return new UserDTO
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                Password = user.Password,
                Salt = user.Salt,
                Status = user.Status
            };
        }

        public async Task<string> GetUserStatus(string login)
        {
            var userDTO = await GetUserByLogin(login);
            return userDTO.Status;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersByLoginPart(string loginPart)
        {
            var userDTOList = await GetAllUsers();
            var usersResult = new List<UserDTO>();
            foreach (var userDTO in userDTOList)
            {
                if (userDTO.Login is not null && userDTO.Login.Contains(loginPart) && userDTO.Status != "admin")
                    usersResult.Add(userDTO);
            }
            return usersResult;
        }

        public async Task<bool> IsUserExists(string login)
        {
            var userDTO = await _uow.Users.FirstOrDefaultAsync(u => u.Login == login);
            return userDTO is not null;
        }

        public async Task<bool> IsPasswordCorrect(string login, string password)
        {
            var userDTO = await GetUserByLogin(login);
            if (userDTO is not null)
            {
                var hash = HashPassword(userDTO.Salt, password);
                return hash == userDTO.Password;
            }
            return false;
        }

        public async Task UpdateUser(UserDTO userDTO)
        {
            //hash user password
            var salt = GetSalt();
            var hash = HashPassword(salt, userDTO.Password);

            //get user
            var user = await _uow.Users.FirstOrDefaultAsync(u => u.Login == userDTO.Login) ?? throw new ValidationException("Wrong user!");

            //update user
            user.Id = userDTO.Id;
            user.Email = userDTO.Email;
            user.Password = userDTO.Password;
            user.Salt = userDTO.Salt;
            user.Status = userDTO.Status;
            _uow.Users.Update(user);

            //save to db
            await _uow.Users.SaveAsync();
        }

        //-----Get-Salt-----
        private static string GetSalt()
        {
            try
            {
                //byte array of secure random number 
                var saltbuf = new byte[16];
                var randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);

                //make salt
                var sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                string salt = sb.ToString();

                return salt;
            }
            catch { throw; }
        }

        //-----Hash-Password-----
        private static string? HashPassword(string? salt, string? userPassword)
        {
            try
            {
                if (salt is null || userPassword is null) return null;

                //hash password
                var password = Encoding.Unicode.GetBytes(salt + userPassword);
                var byteHash = MD5.HashData(password);
                var hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                return hash.ToString();
            }
            catch { throw; }
        }
    }
}
