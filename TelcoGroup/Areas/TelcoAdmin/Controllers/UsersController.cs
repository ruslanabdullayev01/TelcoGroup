using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TelcoGroup.DAL;
using TelcoGroup.Helpers;
using TelcoGroup.Models;

namespace TelcoGroup.Areas.TelcoAdmin.Controllers
{
    [Area("TelcoAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;
        public UsersController(AppDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;

        }
        public IActionResult Index(int pageIndex = 1)
        {
            IQueryable<User> query = _db.Users;
            return View(PageNatedList<User>.Create(query, pageIndex, 10, 10));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index", "Users");
        }
    }
}
