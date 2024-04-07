using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TelcoGroup.DAL;
using TelcoGroup.Models;

namespace TelcoGroup.Controllers
{
    public class NewsController : Controller
    {
        private readonly AppDbContext _db;
        public NewsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<News> news = await _db.News
                .Where(x => !x.IsDeleted && x.Language!.Culture == CultureInfo.CurrentCulture.Name)
                .OrderByDescending(x => x.CreatedAt).AsNoTracking().ToListAsync();
            return View(news);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            News? temp = await _db.News
                .Include(a => a.Language)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id && s.IsDeleted == false);

            if(temp is null) return NotFound();

            News? newsContent = await _db.News
                                     .Include(x => x.Language)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(x => x.LanguageGroup == temp!.LanguageGroup && x.Language!.Culture == CultureInfo.CurrentCulture.Name);

            if (newsContent == null) return View(temp);

            return View(newsContent);
        }
    }
}
