using AutoMapper;
using Kinobaza.BLL.DTO;
using Kinobaza.BLL.Interfaces;
using Kinobaza.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Kinobaza.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userServ;
        public UsersController(IUserService userServ) => _userServ = userServ;

        // GET: Users/Registration
        [HttpGet]
        public IActionResult Registration()
        {
            //create view model
            var userRegVM = new UserRegVM();

            return View(userRegVM);
        }

        // POST: Users/Registration
        [HttpPost, ActionName("Registration")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationConfirmed(UserRegVM userRegVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //check if view model is null
                    if (userRegVM is null || userRegVM.Login is null || userRegVM.Password is null) return NotFound();

                    //check if login same as email
                    if (userRegVM.Login == userRegVM.Email)
                    {
                        ModelState.AddModelError("", "Логин и электронный адрес не должны совпадать");
                        return View(userRegVM);
                    }

                    //check if login already exists
                    var isUserExists = await _userServ.IsUserExists(userRegVM.Login);
                    if (isUserExists)
                    {
                        ModelState.AddModelError("", "Такой логин уже существует");
                        return View(userRegVM);
                    }

                    //create new user data
                    var newUserData = new UserDTO
                    {
                        Login = userRegVM.Login,
                        Email = userRegVM.Email,
                        Password = userRegVM.Password
                    };

                    //register user
                    await _userServ.RegisterUser(newUserData);

                    //Home/Index
                    return RedirectToAction("Index", "Home");
                }
                //Users/Registration
                return View(userRegVM);
            }
            catch { throw; }
        }

        // GET: Users/Authorization
        [HttpGet]
        public IActionResult Authorization()
        {
            //create view model
            var userAuthVM = new UserAuthVM();

            return View(userAuthVM);
        }

        // POST: Users/Authorization
        [HttpPost, ActionName("Authorization")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AuthorizationConfirmed(UserAuthVM userAuthVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //check if user view model exists
                    if (userAuthVM.Login is null || userAuthVM.Password is null) return NotFound();

                    //check if user is exists
                    var isUserExists = await _userServ.IsUserExists(userAuthVM.Login);
                    if (!isUserExists)
                    {
                        ModelState.AddModelError("Login", "Такого пользователя не существует!");
                        return View(userAuthVM);
                    }

                    //check if password is correct
                    if (!await _userServ.IsPasswordCorrect(userAuthVM.Login, userAuthVM.Password))
                    {
                        ModelState.AddModelError("Password", "Неверно введён пароль!");
                        return View(userAuthVM);
                    }

                    //check if user status is waiting
                    var userStatus = await _userServ.GetUserStatus(userAuthVM.Login);
                    if (userStatus == "waiting")
                    {
                        ModelState.AddModelError("", "Ваш аккаунт еще не подтверждён!");
                        return View(userAuthVM);
                    }

                    //check if user status is banned
                    if (userStatus == "banned")
                    {
                        ModelState.AddModelError("", "Ваш аккаунт заблокирован");
                        return View(userAuthVM);
                    }

                    Response.Cookies.Append("login", userAuthVM.Login);
                    HttpContext.Session.SetString("login", userAuthVM.Login);
                    return RedirectToAction("Index", "Home");
                }
                //User/Authorization
                return View(userAuthVM);
            }
            catch { throw; }
        }

        // GET: Users/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }

        // POST: User/Logout
        [HttpPost, ActionName("Logout")]
        [ValidateAntiForgeryToken]
        public IActionResult LogoutConfirmed()
        {
            HttpContext.Session?.Clear();
            Response.Cookies.Delete("login");
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Accounts
        [HttpGet]
        public async Task<IActionResult> Accounts(UserAccountsVM accountsVM)
        {
            try
            {
                //check authorization
                if (HttpContext.Session.GetString("login") != "admin") return NotFound();

                IEnumerable<UserDTO>? usersDTO = new List<UserDTO>();
                IEnumerable<UserAccountVM> accountVMs = new List<UserAccountVM>();

                //if search login is not null, get all found users
                if (accountsVM.SearchLogin is not null)
                {
                    usersDTO = await _userServ.GetUsersByLoginPart(accountsVM.SearchLogin);
                }

                //if search login is null, get all users without admin
                else
                {
                    usersDTO = await _userServ.GetAllWithoutAdmin();
                }
                if (usersDTO is not null)
                {
                    IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserAccountVM>()).CreateMapper();
                    accountVMs = mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserAccountVM>>(usersDTO);
                    accountsVM.UserAccountVMs = accountVMs;
                    accountsVM.Quantity = accountVMs.Count();
                }

                return View(accountsVM);
            }
            catch { return NotFound(); }
        }

        // POST: Users/AccountsStatus
        [HttpPost, ActionName("Accounts")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountsStatus(UserAccountsVM accounts)
        {
            try
            {
                //get user by id
                var userDTO = await _userServ.GetUserById(accounts.UserId);

                //check if user is null
                if (userDTO is null) return NotFound();

                //get new status
                var modificator = accounts.Status;
                string changedStatus = "waiting";
                switch (modificator)
                {
                    case "зарегистрировать":
                        changedStatus = "ok";
                        break;
                    case "заблокировать":
                        changedStatus = "banned";
                        break;
                    case "разблокировать":
                        changedStatus = "ok";
                        break;
                }

                //update user status and save to db
                userDTO.Status = changedStatus;
                await _userServ.UpdateUser(userDTO);

                return RedirectToAction(nameof(Accounts));
            }
            catch { return NotFound(); }
        }

        // GET: Users/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            try
            {
                //check if user exists
                var userLogin = HttpContext.Session.GetString("login");
                if (userLogin is null) return NotFound();
                var userDTO = await _userServ.GetUserByLogin(userLogin);

                //create new view model
                var userProfileVM = new UserProfileVM()
                {
                    Login = userDTO.Login,
                };

                return View(userProfileVM);
            }
            catch { return NotFound(); }
        }

        // POST: Users/Profile
        [HttpPost, ActionName("Profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileConfirmed(UserProfileVM userProfileVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //check if user view model login is null 
                    if (userProfileVM.Login is null) return NotFound();

                    //check if user is exists
                    var userDTO = await _userServ.GetUserByLogin(userProfileVM.Login);
                    if (userDTO is null) return NotFound();

                    //check if password is correct
                    if (userProfileVM.Password is null) return NotFound();
                    var isCorrect = await _userServ.IsPasswordCorrect(userDTO.Login, userProfileVM.Password);
                    if (!isCorrect)
                    {
                        ModelState.AddModelError("Password", "Неверно введён пароль!");
                        return View(userProfileVM);
                    }

                    //update user
                    userDTO.Password = userProfileVM.NewPassword;
                    await _userServ.UpdateUser(userDTO);

                    return RedirectToAction("Index", "Home");
                }
                return View(userProfileVM);
            }
            catch { throw; }
        }
    }
}
