using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TelcoGroup.DAL;
using TelcoGroup.Models;
using TelcoGroup.ViewModels;

namespace TelcoGroup.Controllers
{
    public class ServicesController : Controller
    {
        private readonly AppDbContext _db;
        public ServicesController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null) return BadRequest();

            Service? temp = await _db.Services.Include(a => a.Language).FirstOrDefaultAsync(s => s.Id == id && s.IsDeleted == false);

            if (temp == null) return NotFound();

            Service? service = await _db.Services
                .FirstOrDefaultAsync(x => !x.IsDeleted && x.LanguageGroup == temp.LanguageGroup && x.Language!.Culture == CultureInfo.CurrentCulture.Name);
            ServiceVM serviceVM = new ServiceVM()
            {
                Services = await _db.Services
                .Where(x => !x.IsDeleted && x.Language!.Culture == CultureInfo.CurrentCulture.Name)
                .OrderByDescending(x => x.CreatedAt)
                .Take(4)
                .ToListAsync(),
                Service = service
            };
            if (service == null) return View(temp);

            return View(serviceVM);
        }
    }
}
