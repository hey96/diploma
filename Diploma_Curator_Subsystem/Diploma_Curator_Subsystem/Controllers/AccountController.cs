using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Diploma_Curator_Subsystem.ViewModels;
using Diploma_Curator_Subsystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Diploma_Curator_Subsystem.Data;
using System;
using Diploma_Curator_Subsystem.Models.SubsystemViewModels;
using System.Linq;

namespace Diploma_Curator_Subsystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly SubsystemContext _context;
        public AccountController(SubsystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var expertRole = _context.Roles.Where(r => r.Name == "Куратор экспертного сообщества").SingleOrDefault();
                var expertId = expertRole.ID;
                User user = await _context.UserRoles
                      .Where(ur => ur.RoleID == expertId)
                      .Select(ur => ur.User)
                      .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, "admin")
                    };
                    ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(principal);
                    return RedirectToAction("Index", "Users");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль, или Вы не являетесь пользователем с ролью \"Куратор экспертного сообщества\".");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}