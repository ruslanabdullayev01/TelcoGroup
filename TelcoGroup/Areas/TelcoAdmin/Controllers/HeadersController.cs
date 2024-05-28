using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelcoGroup.DAL;
using TelcoGroup.Models;

namespace TelcoGroup.Areas.TelcoAdmin.Controllers
{
    [Area("TelcoAdmin")]
    public class HeadersController : Controller
    {
        private readonly AppDbContext _db;
        public HeadersController(AppDbContext db)
        {
            _db = db;
        }
        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            List<Language> languages = await _db.Languages.ToListAsync();
            ViewBag.Languages = languages;

            Header? firstHeader = await _db.Headers.FirstOrDefaultAsync(c => c.Id == id);

            if (firstHeader == null) return NotFound();

            List<Header> headers = await _db.Headers.Where(c => c.LanguageGroup == firstHeader.LanguageGroup).ToListAsync();

            return View(headers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, List<Header> header)
        {
            ViewBag.Languages = await _db.Languages.ToListAsync();

            if (!ModelState.IsValid) return View(header);

            if (id == null) return BadRequest();

            Header? firstHeader = await _db.Headers.FirstOrDefaultAsync(c => c.Id == id);

            List<Header> dbHeaders = new List<Header>();

            if (firstHeader == null) return NotFound();

            dbHeaders = await _db.Headers.Where(c => c.LanguageGroup == firstHeader.LanguageGroup).ToListAsync();

            if (dbHeaders == null || dbHeaders.Count == 0) return NotFound();
            if (!ModelState.IsValid) return View(header);

            foreach (Header item in header)
            {
                Header? dbHeader = dbHeaders.FirstOrDefault(s => s.LanguageId == item.LanguageId);
                dbHeader!.BlueText = item.BlueText;
                dbHeader!.GreenText = item.GreenText;
            }

            await _db.SaveChangesAsync();

            if (firstHeader.PageKey == "About") return RedirectToAction("Index", "AboutUs");
            if (firstHeader.PageKey == "Services") return RedirectToAction("Index", "Services");
            if (firstHeader.PageKey == "Solutions") return RedirectToAction("Index", "Solutions");
            if (firstHeader.PageKey == "Contact") return RedirectToAction("Index", "Bios");
            if (firstHeader.PageKey == "News") return RedirectToAction("Index", "News");
            return RedirectToAction("Index", "Partners");
        }
        #endregion
    }
}
