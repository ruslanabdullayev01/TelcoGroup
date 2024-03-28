using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TelcoGroup.DAL;
using TelcoGroup.Models;
using TelcoGroup.ViewModels;

namespace TelcoGroup.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly AppDbContext _db;
        public FooterViewComponent(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string tempName = CultureInfo.CurrentCulture.Name;
            Language? tempCulture = await _db.Languages.FirstOrDefaultAsync(l => l.Culture == CultureInfo.CurrentCulture.Name);

            ViewBag.FirstService = await _db.Services.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync(x => !x.IsDeleted);

            FooterVM footerVM = new FooterVM
            {
                Bio = await _db.Bios.FirstOrDefaultAsync(x => !x.IsDeleted && x.Language!.Culture == CultureInfo.CurrentCulture.Name)
            };

            return View(footerVM);
        }
    }
}
