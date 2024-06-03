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
    public class BiosController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;

        public BiosController(AppDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        #region Index
        public IActionResult Index(int pageIndex = 1)
        {
            IQueryable<Bio> query = _db.Bios.AsNoTracking().Where(x => !x.IsDeleted && x.Language.Culture == CultureInfo.CurrentCulture.Name);
            ViewBag.DataCount = query.Count();
            ViewBag.Headers = _db.Headers.AsNoTracking().Where(x => x.PageKey == "Contact" && x.Language!.Culture == CultureInfo.CurrentCulture.Name);
            return View(PageNatedList<Bio>.Create(query, pageIndex, 5, 5));
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
        public async Task<IActionResult> Create(List<Bio> models)
        {
            ViewBag.Languages = await _db.Languages.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(models);
            }

            Bio? temp = await _db.Bios.OrderByDescending(a => a.Id).FirstOrDefaultAsync();

            string currentUsername = _userManager.GetUserName(HttpContext.User);
            foreach (Bio item in models)
            {
                item.PhoneNumber = models[0].PhoneNumber;
                item.Email = models[0].Email;
                item.Facebook = models[0].Facebook;
                item.Instagram = models[0].Instagram;
                item.Linkedin = models[0].Linkedin;
                item.Twitter = models[0].Twitter;
                item.MapLink = models[0].MapLink;
                item.LanguageGroup = temp != null ? temp.LanguageGroup + 1 : 1;
                item.CreatedAt = DateTime.UtcNow.AddHours(4);
                item.CreatedBy = currentUsername;
                await _db.Bios.AddAsync(item);
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

            Bio? firstBio = await _db.Bios.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);

            if (firstBio == null) return NotFound();

            List<Bio> bios = await _db.Bios
                .Where(c => c.LanguageGroup == firstBio.LanguageGroup && c.IsDeleted == false).ToListAsync();

            return View(bios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, List<Bio> bios)
        {
            ViewBag.Languages = await _db.Languages.ToListAsync();

            if (!ModelState.IsValid) return View(bios);

            if (id == null) return BadRequest();

            Bio? firstBio = await _db.Bios.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);

            List<Bio> dbBios = new List<Bio>();

            if (firstBio == null) return NotFound();

            dbBios = await _db.Bios.Where(c => c.LanguageGroup == firstBio.LanguageGroup && c.IsDeleted == false).ToListAsync();

            if (dbBios == null || dbBios.Count == 0) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(bios);
            }

            string currentUsername = _userManager.GetUserName(HttpContext.User);
            foreach (Bio item in bios)
            {
                Bio dbBio = dbBios.FirstOrDefault(s => s.LanguageId == item.LanguageId);
                dbBio.PhoneNumber = bios[0].PhoneNumber;
                dbBio.Email = bios[0].Email;
                dbBio.Facebook = bios[0].Facebook;
                dbBio.Instagram = bios[0].Instagram;
                dbBio.Linkedin = bios[0].Linkedin;
                dbBio.Twitter = bios[0].Twitter;
                dbBio.MapLink = bios[0].MapLink;
                dbBio.Address = item.Address;
                dbBio.UpdatedAt = DateTime.UtcNow.AddHours(4);
                dbBio.UpdatedBy = currentUsername;
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

            Bio? temp = await _db.Bios.AsNoTracking().Include(a => a.Language).FirstOrDefaultAsync(s => s.IsDeleted == false);

            Bio? bio = await _db.Bios.AsNoTracking()
                .Include(x => x.Language)
                .FirstOrDefaultAsync(x => x.LanguageGroup == temp.LanguageGroup && x.Language.Culture == CultureInfo.CurrentCulture.Name); ;
            if (bio == null) return BadRequest();
            return View(bio);
        }
        #endregion
    }
}
