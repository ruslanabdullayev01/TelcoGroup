using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            List<Partner> partners = await _db.Partners.Where(x => !x.IsDeleted).ToListAsync();
            return View(partners);
        }
    }
}
