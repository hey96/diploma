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

namespace Diploma_Curator_Subsystem.Controllers
{
    public class TasksController : Controller
    {
        private readonly SubsystemContext _context;

        public TasksController(SubsystemContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var viewModel = new TaskIndexData();
            var statusForCuratorHandling = _context.Statuses.Where(s => s.Name == "Ожидает подбора группы от КЭС").SingleOrDefault();
            var statusForCuratorHandlingId = statusForCuratorHandling.ID;
            viewModel.Tasks = await _context.Tasks
                  .Where(t => t.Status.ID == statusForCuratorHandlingId)
                  .Include(t => t.Domain)
                  .Include(t => t.Status)
                .ToListAsync();
            return View(viewModel);

            //var subsystemContext = _context.Tasks.Include(t => t.Domain).Include(t => t.Status);
            //return View(await subsystemContext.ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Domain)
                .Include(t => t.Status)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            ViewData["DomainID"] = new SelectList(_context.Domains, "ID", "ID");
            ViewData["StatusID"] = new SelectList(_context.Statuses, "ID", "ID");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Description,Alternatives,Math_data,StatusID,DomainID")] Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DomainID"] = new SelectList(_context.Domains, "ID", "ID", task.DomainID);
            ViewData["StatusID"] = new SelectList(_context.Statuses, "ID", "ID", task.StatusID);
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.SingleOrDefaultAsync(m => m.ID == id);
            if (task == null)
            {
                return NotFound();
            }
            ViewData["DomainID"] = new SelectList(_context.Domains, "ID", "ID", task.DomainID);
            ViewData["StatusID"] = new SelectList(_context.Statuses, "ID", "ID", task.StatusID);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description,Alternatives,Math_data,StatusID,DomainID")] Models.Task task)
        {
            if (id != task.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.ID))
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
            ViewData["DomainID"] = new SelectList(_context.Domains, "ID", "ID", task.DomainID);
            ViewData["StatusID"] = new SelectList(_context.Statuses, "ID", "ID", task.StatusID);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Domain)
                .Include(t => t.Status)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.SingleOrDefaultAsync(m => m.ID == id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.ID == id);
        }
    }
}
