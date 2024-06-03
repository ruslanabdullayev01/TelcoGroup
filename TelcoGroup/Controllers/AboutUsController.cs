using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Globalization;
using TelcoGroup.DAL;
using TelcoGroup.Models;

namespace TelcoGroup.Controllers
{
    public class AboutUsController : Controller
    {
        private readonly AppDbContext _db;
        public AboutUsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            AboutUs? aboutUs = await _db.AboutUs.AsNoTracking().FirstOrDefaultAsync(x => !x.IsDeleted && x.Language!.Culture == CultureInfo.CurrentCulture.Name);
            ViewBag.Header = await _db.Headers.AsNoTracking().FirstOrDefaultAsync(x => x.PageKey == "About" && x.Language!.Culture == CultureInfo.CurrentCulture.Name);
            return View(aboutUs);
        }
    }
}
