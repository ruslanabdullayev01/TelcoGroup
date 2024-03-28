using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelcoGroup.DAL;

namespace TelcoGroup.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly AppDbContext _db;
        public HeaderViewComponent(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.FirstService = await _db.Services.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync(x => !x.IsDeleted);
            return View();
        }
    }
}
