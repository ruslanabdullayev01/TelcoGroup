using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TelcoGroup.DAL;
using TelcoGroup.Helpers;
using TelcoGroup.Models;

namespace TelcoGroup.Areas.TelcoAdmin.Controllers
{
    [Area("TelcoAdmin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class PartnersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;

        public PartnersController(AppDbContext db
            , IWebHostEnvironment env
            , UserManager<User> userManager)
        {
            _db = db;
            _env = env;
            _userManager = userManager;
        }

        #region Index
        public IActionResult Index(int pageIndex = 1)
        {
            IQueryable<Partner> query = _db.Partners.AsNoTracking().Where(x => !x.IsDeleted);
            ViewBag.Setting = _db.Settings.AsNoTracking().Where(x => x.Language!.Culture == CultureInfo.CurrentCulture.Name && x.Key == "PartnersPageDescription");
            ViewBag.Headers = _db.Headers.AsNoTracking().Where(x => x.PageKey == "Partners" && x.Language!.Culture == CultureInfo.CurrentCulture.Name);
            return View(PageNatedList<Partner>.Create(query, pageIndex, 10, 10));
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Partner model)
        {
            if (model.Photo != null)
            {
                string currentUsername = _userManager.GetUserName(HttpContext.User);
                if (!(model.Photo.CheckFileContenttype("image/jpeg") || model.Photo.CheckFileContenttype("image/png")))
                {
                    ModelState.AddModelError("Photo", $"{model.Photo.FileName} is not the correct format");
                    return View(model);
                }

                if (model.Photo.CheckFileLength(5120))
                {
                    ModelState.AddModelError("Photo", $"Photo must be less than 5 mb");
                    return View(model);
                }

                model.ImagePath = await model.Photo.CreateFileAsync(_env, "src", "images", "partners");
                model.CreatedAt = DateTime.UtcNow.AddHours(4);
                model.CreatedBy = currentUsername;
            }
            else
            {
                ModelState.AddModelError("Photo", "Image is empty");
                return View(model);
            }

            await _db.AddAsync(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Partner? dbPartner = await _db.Partners.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (dbPartner == null) return NotFound();

            return View(dbPartner);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Partner partner)
        {
            if (id == null) return BadRequest();

            Partner? dbPartner = await _db.Partners.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (dbPartner == null) return NotFound();

            if (!ModelState.IsValid) return View();

            #region Image
            if (partner.Photo != null)
            {
                if (!(partner.Photo.CheckFileContenttype("image/jpeg") || partner.Photo.CheckFileContenttype("image/png")))
                {
                    ModelState.AddModelError("Photo", $"{partner.Photo.FileName} is not the correct format");
                    return View();
                }

                if (partner.Photo.CheckFileLength(5120))
                {
                    ModelState.AddModelError("Photo", $"Photo must be less than 5 mb");
                    return View();
                }

                string previousFilePath = dbPartner.ImagePath;
                if (previousFilePath != null)
                {
                    FileHelper.DeleteFile(previousFilePath, _env, "src", "images", "partners");
                }

                dbPartner.ImagePath = await partner.Photo.CreateFileAsync(_env, "src", "images", "partners");
            }
            #endregion
            string currentUsername = _userManager.GetUserName(HttpContext.User);

            dbPartner.Link = partner.Link;
            dbPartner.UpdatedBy = currentUsername;
            dbPartner.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            Partner? partner = await _db.Partners
                .FirstOrDefaultAsync(b => b.Id == id && b.IsDeleted == false);

            if (partner == null) return NotFound();

            if (partner.ImagePath != null)
            {
                FileHelper.DeleteFile(partner.ImagePath, _env, "src", "images", "partners");
            }

            string currentUsername = _userManager.GetUserName(HttpContext.User);
            partner.IsDeleted = true;
            partner.DeletedBy = currentUsername;
            partner.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        #endregion
    }
}
