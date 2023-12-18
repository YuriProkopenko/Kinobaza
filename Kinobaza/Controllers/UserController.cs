using Kinobaza.Data;
using Kinobaza.Data.Repository.IRepository;
using Kinobaza.Models;
using Kinobaza.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Kinobaza.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;
        public UserController(IUserRepository userRepo) => _userRepo = userRepo;

        #region Controllers

        //-----Registration-----

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost, ActionName("Registration")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationConfirmed(UserRegVM userRegVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (userRegVM is null || userRegVM.Login is null || userRegVM.Password is null) return NotFound();

                    //check if login same as email
                    if (userRegVM.Login == userRegVM.Email)
                    {
                        ModelState.AddModelError("", "Логин и электронный адрес не должны совпадать");
                        return View();
                    }

                    //check if login already exists
                    var user = await _userRepo.FirstOrDefaultAsync(u => u.Login == userRegVM.Login);
                    if (user is not null)
                    {
                        ModelState.AddModelError("", "Такой логин уже существует");
                        return View();
                    }

                    //if it's ok adding user to db

                    //hash user password
                    var salt = GetSalt();
                    var hash = HashPassword(salt, userRegVM.Password);

                    var newUser = new User
                    {
                        Id = userRegVM.Id,
                        Login = userRegVM.Login,
                        Email = userRegVM.Email,
                        Password = hash,
                        Salt = salt
                    };

                    //add user to db
                    await _userRepo.AddAsync(newUser);
                    await _userRepo.SaveAsync();

                    //Home/Index
                    return RedirectToAction("Index", "Home");
                }
                catch { throw; }
            }
            return View(userRegVM);
        }

        //-----Authorization-----

        [HttpGet]
        public IActionResult Authorization()
        {
            return View();
        }

        [HttpPost, ActionName("Authorization")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AuthorizationConfirmed(UserAuthVM userAuthVM)
        {
            if (userAuthVM == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userRepo.FirstOrDefaultAsync(u => u.Login == userAuthVM.Login);

                    //check if user is exists
                    if (user is null || user.Login is null)
                    {
                        ModelState.AddModelError("Login", "Такого пользователя не существует!");
                        return View(userAuthVM);
                    }

                    //check if password is correct
                    var hash = HashPassword(user.Salt, userAuthVM.Password);
                    if (user.Password != hash)
                    {
                        ModelState.AddModelError("Password", "Неверно введён пароль!");
                        return View(userAuthVM);
                    }

                    //check if user status is waiting
                    if (user.Status == "waiting")
                    {
                        ModelState.AddModelError("", "Ваш аккаунт еще не подтверждён!");
                        return View(userAuthVM);
                    }

                    //check is user status is banned
                    if (user.Status == "banned")
                    {
                        ModelState.AddModelError("", "Ваш аккаунт заблокирован");
                        return View(userAuthVM);
                    }

                    //if login and password are correct and status is ok set cookies and set user login to session
                    Response.Cookies.Append("login", user.Login);
                    HttpContext.Session.SetString("login", user.Login);
                    WC.UserLogin = user.Login;
                    return RedirectToAction("Index", "Home");
                }
                catch { throw; }
            }
            return View(userAuthVM);
        }

        //-----Logout-----

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }

        [HttpPost, ActionName("Logout")]
        [ValidateAntiForgeryToken]
        public IActionResult LogoutConfirmed()
        {
            HttpContext.Session?.Clear();
            Response.Cookies.Delete("login");
            WC.UserLogin = null;
            return RedirectToAction("Index", "Home");
        }

        //-----Accounts-----

        [HttpGet]
        public async Task<IActionResult> Accounts(UserAccountsVM? accounts)
        {
            if (accounts is null) return NotFound();

            //create view model
            var accountsVM = new UserAccountsVM();

            //check if search login is exists
            if (accounts.SearchLogin is not null)
            {
                var user = await _userRepo.FirstOrDefaultAsync(u => u.Login == accounts.SearchLogin);
                if (user is not null)
                {
                    accountsVM.UserAccountVMs = new List<UserAccountVM>(){ new()
                    {
                        Id = user.Id,
                        Login = user!.Login,
                        Email = user!.Email,
                        Status = user!.Status
                    } };
                    accountsVM.Quantity = 1;
                    accountsVM.SearchLogin = accounts.SearchLogin;
                }
                return View(accountsVM);
            }
            //if search login is null, fill view model with all users
            else
            {
                //get all users
                var users = await _userRepo.GetAllAsync();
                if (users is null) return NotFound();

                //remove admin from the list
                var admin = await _userRepo.FirstOrDefaultAsync(u => u.Status == null);
                if (admin is not null) users?.ToList().Remove(admin);

                var accountVMs = new List<UserAccountVM>();
                foreach (var user in users!)
                {
                    accountVMs.Add(new UserAccountVM()
                    {
                        Id = user.Id,
                        Login = user.Login,
                        Email = user.Email,
                        Status = user.Status,
                    });
                    accountsVM.Quantity = users.Where(u => u.Status is not null).Count();
                }
                accountsVM.UserAccountVMs = accountVMs;
            }

            return View(accountsVM);
        }

        [HttpPost, ActionName("Accounts")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountsStatus(UserAccountsVM? accounts)
        {
            try
            {
                //check if user view model is null
                if (accounts is null) return NotFound();

                //get user by id
                var user = await _userRepo.FirstOrDefaultAsync(u => u.Id == accounts.UserId);

                //check if user is null
                if (user is null) return NotFound();

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
                user.Status = changedStatus;
                _userRepo.Update(user);
                await _userRepo.SaveAsync();

                return RedirectToAction("Accounts");
            }
            catch
            {
                return NotFound();
            }
        }

        //-----Profile-----

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            //check if session user not exists
            var userLogin = HttpContext.Session.GetString("login");
            if (userLogin == null) return NotFound();

            //check if user not exists
            var user = await _userRepo.FirstOrDefaultAsync(u => u.Login == userLogin);
            if (user == null) return NotFound();

            //create new view model
            var userProfileVM = new UserProfileVM()
            {
                Login = user.Login,
            };

            return View(userProfileVM);
        }

        [HttpPost, ActionName("Profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileConfirmed(UserProfileVM userProfileVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //check if view model is null
                    if (userProfileVM is null) return NotFound();

                    var user = await _userRepo.FirstOrDefaultAsync(u => u.Login == userProfileVM.Login);
                    if (user is null) return NotFound();

                    //check if password is correct
                    var hash = HashPassword(user.Salt, userProfileVM.Password);
                    if (user.Password != hash)
                    {
                        ModelState.AddModelError("Password", "Неверно введён пароль!");
                        return View(userProfileVM);
                    }

                    //hash user password
                    var salt = GetSalt();
                    var hashPass = HashPassword(salt, userProfileVM.NewPassword);

                    //set salt and password to user
                    user.Salt = salt;
                    user.Password = hashPass;

                    //update user to db
                    _userRepo.Update(user);
                    await _userRepo.SaveAsync();
                    return RedirectToAction("Index","Home");
                }
                catch { throw; }
            }
            return View(userProfileVM);
        }

        #endregion

        #region Private

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

        #endregion

    }
}
