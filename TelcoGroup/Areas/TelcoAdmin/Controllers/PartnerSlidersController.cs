using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelcoGroup.DAL;
using TelcoGroup.Helpers;
using TelcoGroup.Models;

namespace TelcoGroup.Areas.TelcoAdmin.Controllers
{
    [Area("TelcoAdmin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class PartnerSlidersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;

        public PartnerSlidersController(AppDbContext db
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
            IQueryable<PartnerSlider> query = _db.PartnerSliders.Where(x => !x.IsDeleted);
            return View(PageNatedList<PartnerSlider>.Create(query, pageIndex, 10, 10));
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartnerSlider model)
        {
            if (model.Photo != null)
            {
                string? currentUsername = _userManager.GetUserName(HttpContext.User);
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

                model.ImagePath = await model.Photo.CreateFileAsync(_env, "src", "images", "partnersSlide");
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

            PartnerSlider? dbPartnerSlider = await _db.PartnerSliders.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (dbPartnerSlider == null) return NotFound();

            return View(dbPartnerSlider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, PartnerSlider partnerSlider)
        {
            if (id == null) return BadRequest();

            PartnerSlider? dbPartnerSlider = await _db.PartnerSliders.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (dbPartnerSlider == null) return NotFound();

            if (!ModelState.IsValid) return View();

            #region Image
            if (partnerSlider.Photo != null)
            {
                if (!(partnerSlider.Photo.CheckFileContenttype("image/jpeg") || partnerSlider.Photo.CheckFileContenttype("image/png")))
                {
                    ModelState.AddModelError("Photo", $"{partnerSlider.Photo.FileName} is not the correct format");
                    return View();
                }

                if (partnerSlider.Photo.CheckFileLength(5120))
                {
                    ModelState.AddModelError("Photo", $"Photo must be less than 5 mb");
                    return View();
                }

                string previousFilePath = dbPartnerSlider.ImagePath;
                if (previousFilePath != null)
                {
                    FileHelper.DeleteFile(previousFilePath, _env, "src", "images", "partnersSlide");
                }

                dbPartnerSlider.ImagePath = await partnerSlider.Photo.CreateFileAsync(_env, "src", "images", "partnersSlide");
            }
            #endregion
            string? currentUsername = _userManager.GetUserName(HttpContext.User);

            dbPartnerSlider.UpdatedBy = currentUsername;
            dbPartnerSlider.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            PartnerSlider? partnerSlider = await _db.PartnerSliders
                .FirstOrDefaultAsync(b => b.Id == id && b.IsDeleted == false);

            if (partnerSlider == null) return NotFound();

            if (partnerSlider.ImagePath != null)
            {
                FileHelper.DeleteFile(partnerSlider.ImagePath, _env, "src", "images", "partnersSlide");
            }

            string? currentUsername = _userManager.GetUserName(HttpContext.User);
            partnerSlider.IsDeleted = true;
            partnerSlider.DeletedBy = currentUsername;
            partnerSlider.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        #endregion
    }
}
