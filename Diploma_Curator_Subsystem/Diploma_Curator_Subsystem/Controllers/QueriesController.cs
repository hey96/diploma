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
    public class QueriesController : Controller
    {
        private readonly SubsystemContext _context;
        int maxProjects = 2;

        public class Note
        {
            public int[] arrExpertsId;
            public DateTime date;
        }

        public QueriesController(SubsystemContext context)
        {
            _context = context;
        }

        // GET: Queries
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var subsystemContext = _context.Queries.Include(q => q.Task).ThenInclude(q => q.Domain);
            return View(await subsystemContext.ToListAsync());
        }

        // GET: Queries/Details/5
        [Authorize]
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
            if (result == null)
            {
                ViewBag.IsResult = "false";
            }
            else if (result != null)
            {
                ViewBag.IsResult = "true";
                var mas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(result);
                List<Note> notes = new List<Note>();
                for (var l = 0; l < mas.Count; l++)
                {
                    if (mas[l] != "{}")
                    {
                        notes.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<Note>(mas.ElementAt(l)));
                    }
                }
                List<int> sizes = new List<int>();
                List<DateTime> dates = new List<DateTime>();
                List<int[]> arrays = new List<int[]>();
                List<int> uniqueIds = new List<int>();
                for (var j = 0; j < notes.Count; j++)
                {
                    for (var len = 0; len < notes.ElementAt(j).arrExpertsId.Length; len++)
                    {
                        if (!uniqueIds.Contains(notes.ElementAt(j).arrExpertsId[len]))
                            uniqueIds.Add(notes.ElementAt(j).arrExpertsId[len]);
                    }
                    dates.Add(notes.ElementAt(j).date);
                    sizes.Add(notes.ElementAt(j).arrExpertsId.Length);
                    arrays.Add(notes.ElementAt(j).arrExpertsId);
                }
                viewModel.Users = await _context.Users
                    .Where(u => uniqueIds.Contains(u.ID))
                    .Include(u => u.UserDomains)
                    .ToListAsync();
                ViewBag.Arrays = arrays;
                ViewBag.Dates = dates;
                ViewBag.Sizes = sizes;
            }
            return View(viewModel);
        }

        // GET: Queries/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["TaskID"] = new SelectList(_context.Tasks, "ID", "Title");
            return View();
        }

        // POST: Queries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,MaxNumExpert,MinNumExpert,MinCompetitionCoef,AvgCompetitionCoef,RequiredDate,Step,LastDate,Result,TaskID")] Query query)
        {
            if (ModelState.IsValid)
            {
                _context.Add(query);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaskID"] = new SelectList(_context.Tasks, "ID", "Title", query.TaskID);
            return View(query);
        }

        // GET: Queries/Edit/5
        [Authorize]
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
            ViewData["TaskID"] = new SelectList(_context.Tasks, "ID", "Title", query.TaskID);
            return View(query);
        }

        // POST: Queries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MaxNumExpert,MinNumExpert,MinCompetitionCoef,AvgCompetitionCoef,RequiredDate,Step,LastDate,Result,TaskID")] Query query)
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

                    //Обновление и пересчет Result-а
                    var queryForUpdateResult = await _context.Queries.Include(q => q.Task).SingleOrDefaultAsync(m => m.ID == id);
                    if (queryForUpdateResult == null)
                    {
                        return NotFound();
                    }
                    var result = DistributeFunc(queryForUpdateResult.MaxNumExpert, queryForUpdateResult.MinNumExpert, queryForUpdateResult.MinCompetitionCoef, queryForUpdateResult.AvgCompetitionCoef, queryForUpdateResult.RequiredDate, queryForUpdateResult.Step, queryForUpdateResult.LastDate, queryForUpdateResult.Task.DomainID);
                    queryForUpdateResult.Result = result;

                    var taskStatus = _context.Statuses.Where(r => r.Name == "Формирование экспертной группы").SingleOrDefault();
                    var taskStatusId = taskStatus.ID;
                    var task = await _context.Tasks.SingleOrDefaultAsync(t => t.ID == queryForUpdateResult.TaskID);
                    task.StatusID = taskStatusId;

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
            ViewData["TaskID"] = new SelectList(_context.Tasks, "ID", "Title", query.TaskID);
            return View(query);
        }

        // GET: Queries/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
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
            if (result == null)
            {
                ViewBag.IsResult = "false";
            }
            else if (result != null)
            {
                ViewBag.IsResult = "true";
                var mas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(result);
                List<Note> notes = new List<Note>();
                for (var l = 0; l < mas.Count; l++)
                {
                    if (mas[l] != "{}")
                    {
                        notes.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<Note>(mas.ElementAt(l)));
                    }
                }
                List<int> sizes = new List<int>();
                List<DateTime> dates = new List<DateTime>();
                List<int[]> arrays = new List<int[]>();
                List<int> uniqueIds = new List<int>();
                for (var j = 0; j < notes.Count; j++)
                {
                    for (var len = 0; len < notes.ElementAt(j).arrExpertsId.Length; len++)
                    {
                        if (!uniqueIds.Contains(notes.ElementAt(j).arrExpertsId[len]))
                            uniqueIds.Add(notes.ElementAt(j).arrExpertsId[len]);
                    }
                    dates.Add(notes.ElementAt(j).date);
                    sizes.Add(notes.ElementAt(j).arrExpertsId.Length);
                    arrays.Add(notes.ElementAt(j).arrExpertsId);
                }
                viewModel.Users = await _context.Users
                    .Where(u => uniqueIds.Contains(u.ID))
                    .Include(u => u.UserDomains)
                    .ToListAsync();
                ViewBag.Arrays = arrays;
                ViewBag.Dates = dates;
                ViewBag.Sizes = sizes;
            }
            return View(viewModel);
        }

        // POST: Queries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var query = await _context.Queries.SingleOrDefaultAsync(m => m.ID == id);
            _context.Queries.Remove(query);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public string ResultJson(ICollection<User> experts, int MaxNumExpert, int MinNumExpert, DateTime curDate)
        {
            if ((experts.Count() <= MaxNumExpert) && (experts.Count() >= MinNumExpert))
            {
                int[] ids = new int[experts.Count];
                for (var i = 0; i < experts.Count; i++)
                {
                    ids[i] = experts.ElementAt(i).ID;
                }
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Note() { arrExpertsId = ids, date = curDate });
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
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Note() { arrExpertsId = ids, date = curDate });
                return json;
            }
            return "{}";
        }

        public ICollection<User> SortedByDate(ICollection<User> experts, DateTime RequiredDate)
        {
            int[] idsUser = new int[experts.Count];
            var j = 0;
            List<User> localExperts = new List<User>(experts);
            foreach (var user in experts)
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
                      .Where(t => t.CreatedAt <= RequiredDate)
                      .Where(t => t.ExpirationDate >= RequiredDate)
                      .OrderBy(t => t.ExpirationDate)
                    .ToList();
                var countProject = newViewModel.Tasks.Count;
                if (countProject >= maxProjects)
                {
                    localExperts.Remove(localExperts.Where(u => u.ID == idUser).SingleOrDefault());
                }
            }
            return localExperts;
        }

        public ICollection<User> AvgExperts(ICollection<User> experts, decimal? AvgCompetitionCoef, int idDomain)
        {
            int[] idsUser = new int[experts.Count];
            var j = 0;
            List<User> localExperts = new List<User>(experts);
            foreach (var user in experts)
            {
                idsUser[j] = user.ID;
                j++;
            }
            var newViewModel = new UserIndexData();
            for (int k = 0; k < idsUser.Length; k++)
            {
                var idUser = idsUser[k];
                if (k == 0)
                {
                    newViewModel.Users = _context.Users
                    .Where(u => u.ID == idUser)
                    .ToList();
                    newViewModel.UserDomains = _context.UserDomains
                        .Where(ud => ud.DomainID == idDomain)
                        .Where(ud => ud.UserID == idUser)
                        .ToList();
                }
                else
                {
                    newViewModel.Users.Add(_context.Users
                        .Where(u => u.ID == idUser)
                        .SingleOrDefault());
                    newViewModel.UserDomains.Add(_context.UserDomains
                        .Where(ud => ud.DomainID == idDomain)
                        .Where(ud => ud.UserID == idUser)
                        .SingleOrDefault());
                }
                
            }
            var sum = 0.0m;
            var num = 0;
            foreach (var e in newViewModel.UserDomains)
            {
                sum += e.CompetitionCoef;
                num++;
            }
            var avg = sum / num;
            while (avg < AvgCompetitionCoef)
            {
                newViewModel.Users.Remove(newViewModel.Users.Last());
                newViewModel.UserDomains.Remove(newViewModel.UserDomains.Last());
                sum = 0.0m;
                num = 0;
                foreach (var e in newViewModel.UserDomains)
                {
                    sum += e.CompetitionCoef;
                    num++;
                }
                avg = sum / num;
            }
            return newViewModel.Users;
        }

        public string DistributeFunc(int MaxNumExpert, int MinNumExpert, decimal? MinCompetitionCoef, decimal? AvgCompetitionCoef, DateTime RequiredDate, int? Step, DateTime? LastDate, int idDomain)
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

            string result = "";
            if ((MinCompetitionCoef == null) && (AvgCompetitionCoef == null))
            {
                if ((Step != null) && (LastDate != null))
                {
                    double st = System.Convert.ToDouble(Step);
                    List<string> variants = new List<string>();
                    for (var date = RequiredDate; date <= LastDate; date = date.AddDays(st))
                    {
                        ICollection<User> experts = SortedByDate(viewModel.Users, date);
                        var tmpRes = ResultJson(experts, MaxNumExpert, MinNumExpert, date);
                        variants.Add(tmpRes);
                    }
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(variants);
                }
                else
                {
                    List<string> variant = new List<string>();
                    ICollection<User> experts = SortedByDate(viewModel.Users, RequiredDate);
                    var tmpRes = ResultJson(experts, MaxNumExpert, MinNumExpert, RequiredDate);
                    variant.Add(tmpRes);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(variant);
                }
            }
            else if (MinCompetitionCoef != null && AvgCompetitionCoef == null)
            {
                E2.Users = _context.UserDomains
                  .Where(ud => ud.DomainID == idDomain)
                  .Where(ud => ud.CompetitionCoef >= MinCompetitionCoef)
                  .OrderByDescending(ud => ud.CompetitionCoef)
                  .Select(ud => ud.User)
                  .Include(u => u.UserRoles)
                  .Include(u => u.UserDomains)
                .ToList();

                if ((Step != null) && (LastDate != null))
                {
                    double st = System.Convert.ToDouble(Step);
                    List<string> variants = new List<string>();
                    for (var date = RequiredDate; date <= LastDate; date = date.AddDays(st))
                    {
                        ICollection<User> experts = SortedByDate(E2.Users, date);
                        var tmpRes = ResultJson(experts, MaxNumExpert, MinNumExpert, date);
                        variants.Add(tmpRes);
                    }
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(variants);
                }
                else
                {
                    List<string> variant = new List<string>();
                    ICollection<User> experts = SortedByDate(E2.Users, RequiredDate);
                    var tmpRes = ResultJson(experts, MaxNumExpert, MinNumExpert, RequiredDate);
                    variant.Add(tmpRes);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(variant);
                }
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

                if ((Step != null) && (LastDate != null))
                {
                    double st = System.Convert.ToDouble(Step);
                    List<string> variants = new List<string>();
                    for (var date = RequiredDate; date <= LastDate; date = date.AddDays(st))
                    {
                        ICollection<User> expertsByDate = SortedByDate(E3.Users, date);
                        ICollection<User> experts = AvgExperts(expertsByDate, AvgCompetitionCoef, idDomain);
                        var tmpRes = ResultJson(experts, MaxNumExpert, MinNumExpert, date);
                        variants.Add(tmpRes);
                    }
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(variants);
                }
                else
                {
                    List<string> variant = new List<string>();
                    ICollection<User> expertsByDate = SortedByDate(E3.Users, RequiredDate);
                    ICollection<User> experts = AvgExperts(expertsByDate, AvgCompetitionCoef, idDomain);
                    var tmpRes = ResultJson(experts, MaxNumExpert, MinNumExpert, RequiredDate);
                    variant.Add(tmpRes);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(variant);
                }
            }
            return result;
        }

        [Authorize]
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
            var result = DistributeFunc(query.MaxNumExpert, query.MinNumExpert, query.MinCompetitionCoef, query.AvgCompetitionCoef, query.RequiredDate, query.Step, query.LastDate, query.Task.DomainID);
            query.Result = result;

            var taskStatus = _context.Statuses.Where(r => r.Name == "Формирование экспертной группы").SingleOrDefault();
            var taskStatusId = taskStatus.ID;
            var task = await _context.Tasks.SingleOrDefaultAsync(t => t.ID == query.TaskID);
            task.StatusID = taskStatusId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        private bool QueryExists(int id)
        {
            return _context.Queries.Any(e => e.ID == id);
        }
    }
}