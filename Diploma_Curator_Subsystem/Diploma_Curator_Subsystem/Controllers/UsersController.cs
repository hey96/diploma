using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Diploma_Curator_Subsystem.Data;
using Diploma_Curator_Subsystem.Models;
using Diploma_Curator_Subsystem.Models.SubsystemViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Diploma_Curator_Subsystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly SubsystemContext _context;

        public UsersController(SubsystemContext context)
        {
            _context = context;
        }

        // GET: Users
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var viewModel = new UserIndexData();
            var expertRole = _context.Roles.Where(r => r.Name == "Эксперт").SingleOrDefault();
            var expertId = expertRole.ID;
            viewModel.Users = await _context.UserRoles
                  .Where(ur => ur.RoleID == expertId)
                  .Select(ur => ur.User)
                  .Include(u => u.UserRoles)
                  .Include(u => u.UserDomains)
                .ToListAsync();
            viewModel.Roles = await _context.Roles.ToListAsync();
            viewModel.Domains = await _context.Domains.ToListAsync();
            return View(viewModel);

            //var filteredUsers = await _context.Users
            //    .Where(u => u.UserRoles.Any(ur => ur.RoleID == expertId))
            //    .Include(u => u.UserRoles)
            //    .ToListAsync();

            //var filteredUsers = await _context.Users
            //.SelectMany(u => u.UserRoles)
            //.Where(ur => ur.RoleID == expertId)
            //.Select(ur => ur.User)
            //.Include(ur => ur.UserRoles)
            //.ToListAsync();

            //List<Models.User> filteredUsers = new List<Models.User>();
            //var users = await _context.Users
            //      .Include(u => u.UserRoles)
            //        .ThenInclude(u => u.Role)
            //      .Include(u => u.UserDomains)
            //        .ThenInclude(u => u.Domain)
            //    .AsNoTracking()
            //    .OrderBy(i => i.LastName)
            //    .ToListAsync();
            //foreach(var user in users)
            //{
            //    foreach(var userRole in user.UserRoles)
            //    {
            //        if (userRole.RoleID == expertId)
            //        {
            //            filteredUsers.Add(user);
            //        }
            //    }
            //}
            //return View(filteredUsers);
        }

        // GET: Users/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,LastName,Email,Password")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.ID == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
    }
}
