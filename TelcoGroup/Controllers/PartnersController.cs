using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TelcoGroup.DAL;
using TelcoGroup.Models;

namespace TelcoGroup.Controllers
{
    public class PartnersController : Controller
    {
        private readonly AppDbContext _db;
        public PartnersController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Partner> partners = await _db.Partners.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Header = await _db.Headers.AsNoTracking().FirstOrDefaultAsync(x => x.PageKey == "Partners" && x.Language!.Culture == CultureInfo.CurrentCulture.Name);
            return View(partners);
        }
    }
}
