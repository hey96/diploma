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
    public class QueriesController : Controller
    {
        private readonly SubsystemContext _context;
        int maxProjects = 2;

        public QueriesController(SubsystemContext context)
        {
            _context = context;
        }

        // GET: Queries
        public async Task<IActionResult> Index()
        {
            var subsystemContext = _context.Queries.Include(q => q.Task).ThenInclude(q => q.Domain);
            return View(await subsystemContext.ToListAsync());
        }

        // GET: Queries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = new QueryIndexData();
            viewModel.Query = await _context.Queries
                .Include(q => q.Task)
                .ThenInclude(q => q.Domain)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (viewModel.Query == null)
            {
                return NotFound();
            }
            var result = viewModel.Query.Result;
            var mas = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(result);
            viewModel.Users = await _context.Users
                .Where(u => mas.Contains(u.ID))
                .Include(u => u.UserDomains)
                .ToListAsync();
            return View(viewModel);
        }

        // GET: Queries/Create
        public IActionResult Create()
        {
            ViewData["TaskID"] = new SelectList(_context.Tasks, "ID", "ID");
            return View();
        }

        // POST: Queries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MaxNumExpert,MinNumExpert,MinCompetitionCoef,AvgCompetitionCoef,RequiredDate,Step,Created,Result,TaskID")] Query query)
        {
            if (ModelState.IsValid)
            {
                _context.Add(query);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaskID"] = new SelectList(_context.Tasks, "ID", "ID", query.TaskID);
            return View(query);
        }

        // GET: Queries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = await _context.Queries.SingleOrDefaultAsync(m => m.ID == id);
            if (query == null)
            {
                return NotFound();
            }
            ViewData["TaskID"] = new SelectList(_context.Tasks, "ID", "ID", query.TaskID);
            return View(query);
        }

        // POST: Queries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MaxNumExpert,MinNumExpert,MinCompetitionCoef,AvgCompetitionCoef,RequiredDate,Step,Created,Result,TaskID")] Query query)
        {
            if (id != query.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(query);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QueryExists(query.ID))
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
            ViewData["TaskID"] = new SelectList(_context.Tasks, "ID", "ID", query.TaskID);
            return View(query);
        }

        // GET: Queries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = await _context.Queries
                .Include(q => q.Task)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (query == null)
            {
                return NotFound();
            }

            return View(query);
        }

        // POST: Queries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var query = await _context.Queries.SingleOrDefaultAsync(m => m.ID == id);
            _context.Queries.Remove(query);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public string ResultJson(ICollection<User> experts, int MaxNumExpert, int MinNumExpert)
        {
            if ((experts.Count() <= MaxNumExpert) && (experts.Count() >= MinNumExpert))
            {
                int[] ids = new int[experts.Count];
                for (var i=0;i< experts.Count;i++)
                {
                    ids[i] = experts.ElementAt(i).ID;
                }
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(ids);
                return json;
            }
            else if (experts.Count() > MaxNumExpert)
            {
                var expertsResult = experts.Take(MaxNumExpert);
                int[] ids = new int[MaxNumExpert];
                for (var i = 0; i < MaxNumExpert; i++)
                {
                    ids[i] = experts.ElementAt(i).ID;
                }
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(ids);
                return json;
            }
            return "Impossible to distribute the experts group";
        }

        public string DistributeFunc(int MaxNumExpert, int MinNumExpert, decimal? MinCompetitionCoef, decimal? AvgCompetitionCoef, DateTime? RequiredDate, int? Step, int idDomain)
        {
            var viewModel = new UserIndexData();
            viewModel.Users = _context.UserDomains
                  .Where(ud => ud.DomainID == idDomain)
                  .OrderByDescending(ud => ud.CompetitionCoef)
                  .Select(ud => ud.User)
                  .Include(u => u.UserRoles)
                  .Include(u => u.UserDomains)
                .ToList();
            viewModel.Roles = _context.Roles.ToList();
            viewModel.Domains = _context.Domains.ToList();

            var E2 = new UserIndexData();
            var E3 = new UserIndexData();

            if (MinCompetitionCoef != null && AvgCompetitionCoef == null)
            {
                E2.Users = _context.UserDomains
                  .Where(ud => ud.DomainID == idDomain)
                  .Where(ud => ud.CompetitionCoef >= MinCompetitionCoef)
                  .OrderByDescending(ud => ud.CompetitionCoef)
                  .Select(ud => ud.User)
                  .Include(u => u.UserRoles)
                  .Include(u => u.UserDomains)
                .ToList();
            }
            else if (MinCompetitionCoef != null && AvgCompetitionCoef != null)
            {
                E3.Users = _context.UserDomains
                  .Where(ud => ud.DomainID == idDomain)
                  .Where(ud => ud.CompetitionCoef >= MinCompetitionCoef)
                  .OrderByDescending(ud => ud.CompetitionCoef)
                  .Select(ud => ud.User)
                  .Include(u => u.UserRoles)
                  .Include(u => u.UserDomains)
                .ToList();
                E3.UserDomains = _context.UserDomains
                    .Where(ud => ud.DomainID == idDomain)
                  .Where(ud => ud.CompetitionCoef >= MinCompetitionCoef)
                  .OrderByDescending(ud => ud.CompetitionCoef)
                  .ToList();
                var sum = 0.0m;
                var num = 0;
                foreach (var e in E3.UserDomains)
                {
                    sum += e.CompetitionCoef;
                    num++;
                }
                var avg = sum / num;
                while (avg < AvgCompetitionCoef)
                {
                    E3.Users.Remove(E3.Users.Last());
                    E3.UserDomains.Remove(E3.UserDomains.Last());
                    sum = 0.0m;
                    num = 0;
                    foreach (var e in E3.UserDomains)
                    {
                        sum += e.CompetitionCoef;
                        num++;
                    }
                    avg = sum / num;
                }
            }

            string result = "";
            if ((MinCompetitionCoef == null) && (AvgCompetitionCoef == null))
            {
                int[] idsUser = new int[viewModel.Users.Count];
                var j = 0;
                foreach (var user in viewModel.Users)
                {
                    idsUser[j] = user.ID;
                    j++;
                }
                for (int k = 0; k < idsUser.Length; k++)
                {
                    var idUser = idsUser[k];
                    var newViewModel = new UserIndexData();
                    newViewModel.Tasks = _context.UserTasks
                          .Where(ut => ut.UserID == idUser)
                          .Select(ut => ut.Task)
                          .Where(t => t.ExpirationDate > RequiredDate)
                          .OrderBy(t => t.ExpirationDate)
                        .ToList();
                    var countProject = newViewModel.Tasks.Count;
                    if (countProject >= maxProjects)
                    {
                        viewModel.Users.Remove(viewModel.Users.Where(u => u.ID == idUser).SingleOrDefault());
                    }
                }
                result = ResultJson(viewModel.Users, MaxNumExpert, MinNumExpert);
            }
            else if (MinCompetitionCoef != null && AvgCompetitionCoef == null)
            {
                result = ResultJson(E2.Users, MaxNumExpert, MinNumExpert);
            }
            else if (MinCompetitionCoef != null && AvgCompetitionCoef != null)
            {
                result = ResultJson(E3.Users, MaxNumExpert, MinNumExpert);
            }
            return result;
        }

        public async Task<IActionResult> Distribute(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = await _context.Queries.Include(q => q.Task).SingleOrDefaultAsync(m => m.ID == id);
            if (query == null)
            {
                return NotFound();
            }
            var result = DistributeFunc(query.MaxNumExpert, query.MinNumExpert, query.MinCompetitionCoef, query.AvgCompetitionCoef, query.RequiredDate, query.Step, query.Task.DomainID);
            query.Result = result;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        private bool QueryExists(int id)
        {
            return _context.Queries.Any(e => e.ID == id);
        }
    }
}
