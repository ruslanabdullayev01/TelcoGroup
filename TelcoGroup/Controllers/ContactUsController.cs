using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TelcoGroup.DAL;
using TelcoGroup.ViewModels;

namespace TelcoGroup.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly AppDbContext _db;

        public ContactUsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            ContactUsVM contactVM = new ContactUsVM()
            {
                Bio = await _db.Bios.FirstOrDefaultAsync(x => !x.IsDeleted && x.Language.Culture == CultureInfo.CurrentCulture.Name),
            };
            ViewBag.Header = await _db.Headers.FirstOrDefaultAsync(x => x.PageKey == "Contact" && x.Language!.Culture == CultureInfo.CurrentCulture.Name);
            return View(contactVM);
        }
    }
}
