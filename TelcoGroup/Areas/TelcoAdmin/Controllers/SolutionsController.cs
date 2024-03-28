using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TelcoGroup.DAL;
using TelcoGroup.Helpers;
using TelcoGroup.Models;

namespace TelcoGroup.Areas.TelcoAdmin.Controllers
{
    [Area("TelcoAdmin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class SolutionsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;

        public SolutionsController(AppDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        #region Index
        [HttpGet]
        public IActionResult Index(int pageIndex = 1)
        {
            IQueryable<Solution> query = _db.Solutions.Where(x => !x.IsDeleted && x.Language.Culture == CultureInfo.CurrentCulture.Name);
            return View(PageNatedList<Solution>.Create(query, pageIndex, 10, 10));
        }

        [HttpPost]
        public async Task<IActionResult> SetMainSolution(int[] MainSolutionId)
        {
            if (ModelState.IsValid)
            {
                foreach (var id in MainSolutionId)
                {
                    var solution = _db.Solutions.FirstOrDefault(s => s.Id == id);
                    if (solution != null)
                    {
                        var solutions = _db.Solutions.Where(x => x.LanguageGroup == solution.LanguageGroup);
                        var solutionsForFalse = _db.Solutions.Where(x => x.LanguageGroup != solution.LanguageGroup);
                        if (solutions != null)
                        {
                            foreach (var item in solutions)
                            {
                                item.IsMain = true;
                            }
                        }
                        if (solutionsForFalse != null)
                        {
                            foreach (var item in solutionsForFalse)
                            {
                                item.IsMain = false;
                            }
                        }
                    }
                }

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Languages = await _db.Languages.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<Solution> models)
        {
            ViewBag.Languages = await _db.Languages.ToListAsync();


            if (!ModelState.IsValid)
            {
                return View(models);
            }

            Solution? temp = await _db.Solutions.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            string? currentUsername = _userManager.GetUserName(HttpContext.User);
            foreach (Solution item in models)
            {
                if (item.SubTitle == null) item.SubTitle = "";
                item.LanguageGroup = temp != null ? temp.LanguageGroup + 1 : 1;
                item.CreatedAt = DateTime.UtcNow.AddHours(4);
                item.CreatedBy = currentUsername;
                await _db.Solutions.AddAsync(item);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {

            if (id == null) return BadRequest();

            List<Language> languages = await _db.Languages.ToListAsync();
            ViewBag.Languages = languages;

            Solution? firstNews = await _db.Solutions.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);

            if (firstNews == null) return NotFound();

            List<Solution> solutions = await _db.Solutions
                .Where(c => c.LanguageGroup == firstNews.LanguageGroup && c.IsDeleted == false).ToListAsync();

            return View(solutions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, List<Solution> solution)
        {
            ViewBag.Languages = await _db.Languages.ToListAsync();

            if (!ModelState.IsValid) return View(solution);

            if (id == null) return BadRequest();

            Solution? firstSolution = await _db.Solutions.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);
            if (firstSolution == null) return NotFound();

            List<Solution> dbSolutions = await _db.Solutions.Where(c => c.LanguageGroup == firstSolution.LanguageGroup && c.IsDeleted == false).ToListAsync();

            if (dbSolutions == null || dbSolutions.Count == 0) return NotFound();

            if (!ModelState.IsValid) return View(solution);

            string currentUsername = _userManager.GetUserName(HttpContext.User);
            foreach (Solution item in solution)
            {
                Solution dbSolution = dbSolutions.FirstOrDefault(s => s.LanguageId == item.LanguageId);
                dbSolution.Title = item.Title.Trim();
                dbSolution.SubTitle = item.SubTitle != null ? item.SubTitle.Trim() : "";
                dbSolution.Description = item.Description.Trim();
                dbSolution.UpdatedAt = DateTime.UtcNow.AddHours(4);
                dbSolution.UpdatedBy = currentUsername;
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Solution? temp = await _db.Solutions.Include(a => a.Language).FirstOrDefaultAsync(s => s.IsDeleted == false && s.Id == id);
            if (temp == null) return NotFound();

            Solution? solution = await _db.Solutions
                .Include(x => x.Language)
                .FirstOrDefaultAsync(x => x.LanguageGroup == temp.LanguageGroup && x.Language.Culture == CultureInfo.CurrentCulture.Name); ;
            if (solution == null) return BadRequest();
            return View(solution);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            List<Language> languages = await _db.Languages.ToListAsync();
            ViewBag.Languages = languages;

            Solution? firstSolution = await _db.Solutions.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);

            if (firstSolution == null) return NotFound();

            List<Solution> solutions = await _db.Solutions
                                       .Where(c => c.LanguageGroup == firstSolution.LanguageGroup && c.IsDeleted == false)
                                       .ToListAsync();

            string currentUsername = _userManager.GetUserName(HttpContext.User);
            foreach (Solution sol in solutions)
            {
                if (sol == null) return NotFound();
                sol.IsDeleted = true;
                sol.DeletedBy = currentUsername;
                sol.DeletedAt = DateTime.UtcNow.AddHours(4);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        #endregion
    }
}
