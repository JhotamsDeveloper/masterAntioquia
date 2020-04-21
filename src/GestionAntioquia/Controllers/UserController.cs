using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAntioquia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Model.DTOs;
using Model.Identity;
using Service;

namespace GestionAntioquia.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        [TempData]
        public string ErrorMessage { get; set; }
        public DateTime DataBrithDay { get; set; } = new DateTime(0000, 00);
        public UserController(IUserService userService,
                              UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        // GET: User
        public async Task<ActionResult> Index()
        {
            var _userList = await _userService.GetAll();
            return View(_userList);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _user = await _userService.GetByIdString(id);
            if (_user == null)
            {
                return NotFound();
            }

 

            return View(_user);
        }


        // GET: User/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var _user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Birthday = model.Birthday,
                    Country = model.Country
                };

                var _result = await _userManager.CreateAsync(_user, model.Password);
                await _userManager.AddToRoleAsync(_user, "UserApp");

                if (_result.Succeeded)
                {
                    //Preguntar si requiere confirmacion de la cuenta

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToAction("RegisterConfirmation", new { email = model.Email });
                    //}
                    //else
                    //{
                    //    await _signInManager.SignInAsync(_user, isPersistent: false);
                    //    return RedirectToAction("Index", "Home");
                    //}

                    await _signInManager.SignInAsync(_user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in _result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // GET: User/Login
        [HttpGet]
        public ActionResult Login()
        {

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, 
                                                                      lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Inicio de sesión invalida");
                }
            }
            return View(model);
        }

        // GET: User/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _user = await _userManager.FindByIdAsync(id);
            var _userClaims = await _userManager.GetClaimsAsync(_user);
            var _userRoles = await _userManager.GetRolesAsync(_user);

            if (_user == null)
            {
                return NotFound();
            }

            var _dataBirthday = _user.Birthday.ToString();

            if (_dataBirthday != "")
            {
                DataBrithDay = _user.Birthday.Value;
            }

            var _userView = new UserEditViewModel
            {
                Id = _user.Id,
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                UserName = _user.UserName,
                Email = _user.Email,
                EmailConfirmed = _user.EmailConfirmed,
                PhoneNumber = _user.PhoneNumber,
                Birthday = DataBrithDay,
                Country = _user.Country,
                Cleims = _userClaims.Select(c=>c.Value).ToList(),
                Roles = _userRoles
            };

            return View(_userView);

        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        // GET: User/Edit/5
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var _user = await _userManager.FindByIdAsync(id);

            if (_user == null)
            {
                return NotFound();
            }

            var _dataBirthday = _user.Birthday.ToString();

            if (_dataBirthday != "")
            {
                DataBrithDay = _user.Birthday.Value;
            }

            var _userView = new UserViewModel
            {
                Id = _user.Id,
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                UserName = _user.UserName,
                Email = _user.Email,
                EmailConfirmed = _user.EmailConfirmed,
                PhoneNumber = _user.PhoneNumber,
                Birthday = DataBrithDay,
                Country = _user.Country
            };

            return View(_userView);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var _user = await _userManager.FindByIdAsync(id);

            if (_user == null)
            {
                ViewBag.ErrorMesage = $"El Usario identificado con el id {id} no fue encontrado";
                return NotFound();
            }
            else
            {
                var _result = await _userManager.DeleteAsync(_user);

                if (_result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in _result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }


        // GET
        public ActionResult Logout()
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}