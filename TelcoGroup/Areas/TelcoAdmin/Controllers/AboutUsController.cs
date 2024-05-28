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
    public class AboutUsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;
        public AboutUsController(AppDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        #region Index
        public IActionResult Index(int pageIndex = 1)
        {
            IQueryable<AboutUs> query = _db.AboutUs.Where(x => !x.IsDeleted && x.Language!.Culture == CultureInfo.CurrentCulture.Name);
            ViewBag.DataCount = query.Count();
            ViewBag.Headers = _db.Headers.Where(x => x.PageKey == "About" && x.Language!.Culture == CultureInfo.CurrentCulture.Name);
            return View(PageNatedList<AboutUs>.Create(query, pageIndex, 5, 5));
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
        public async Task<IActionResult> Create(List<AboutUs> models)
        {
            ViewBag.Languages = await _db.Languages.ToListAsync();

            if (!ModelState.IsValid) return View(models);

            Bio? temp = await _db.Bios.OrderByDescending(a => a.Id).FirstOrDefaultAsync();

            string? currentUsername = _userManager.GetUserName(HttpContext.User);
            foreach (AboutUs item in models)
            {
                item.LanguageGroup = temp != null ? temp.LanguageGroup + 1 : 1;
                item.CreatedAt = DateTime.UtcNow.AddHours(4);
                item.CreatedBy = currentUsername;
                await _db.AboutUs.AddAsync(item);
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

            AboutUs? firstAboutUs = await _db.AboutUs.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);

            if (firstAboutUs == null) return NotFound();

            List<AboutUs> aboutUs = await _db.AboutUs
                .Where(c => c.LanguageGroup == firstAboutUs.LanguageGroup && c.IsDeleted == false).ToListAsync();

            return View(aboutUs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, List<AboutUs> aboutUs)
        {
            ViewBag.Languages = await _db.Languages.ToListAsync();

            if (!ModelState.IsValid) return View(aboutUs);

            if (id == null) return BadRequest();

            AboutUs? firstAboutUs = await _db.AboutUs.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);

            List<AboutUs> dbAboutUs = new List<AboutUs>();

            if (firstAboutUs == null) return NotFound();

            dbAboutUs = await _db.AboutUs.Where(c => c.LanguageGroup == firstAboutUs.LanguageGroup && c.IsDeleted == false).ToListAsync();

            if (dbAboutUs == null || dbAboutUs.Count == 0) return NotFound();

            if (!ModelState.IsValid) return View(aboutUs);

            string? currentUsername = _userManager.GetUserName(HttpContext.User);
            foreach (AboutUs item in aboutUs)
            {
                AboutUs? dbAbout = dbAboutUs.FirstOrDefault(s => s.LanguageId == item.LanguageId);
                dbAbout!.GeneralDescription = item.GeneralDescription;
                dbAbout!.TopDescription = item.TopDescription;
                dbAbout!.MiddleDescription = item.MiddleDescription;
                dbAbout!.BottomDescription = item.BottomDescription;
                dbAbout.UpdatedAt = DateTime.UtcNow.AddHours(4);
                dbAbout.UpdatedBy = currentUsername;
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

            AboutUs? temp = await _db.AboutUs.Include(a => a.Language).FirstOrDefaultAsync(s => s.IsDeleted == false);

            AboutUs? aboutUs = await _db.AboutUs
                .Include(x => x.Language)
                .FirstOrDefaultAsync(x => x.LanguageGroup == temp!.LanguageGroup && x.Language!.Culture == CultureInfo.CurrentCulture.Name); ;
            if (aboutUs == null) return BadRequest();
            return View(aboutUs);
        }
        #endregion
    }
}
