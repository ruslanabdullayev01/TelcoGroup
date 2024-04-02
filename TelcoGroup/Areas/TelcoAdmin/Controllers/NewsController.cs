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
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class NewsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;

        public NewsController(AppDbContext db
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
            IQueryable<News> query = _db.News.Where(x => !x.IsDeleted && x.Language!.Culture == CultureInfo.CurrentCulture.Name);
            return View(PageNatedList<News>.Create(query, pageIndex, 10, 10));
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Languages = await _db.Languages.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<News> models)
        {
            ViewBag.Languages = await _db.Languages.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(models);
            }

            #region Image
            if (models[0].Photo != null)
            {
                if (!(models[0].Photo.CheckFileContenttype("image/jpeg") || models[0].Photo.CheckFileContenttype("image/png")))
                {
                    ModelState.AddModelError("[0].Photo", $"{models[0].Photo.FileName} is not the correct format");
                    return View(models);
                }

                if (models[0].Photo.CheckFileLength(5120))
                {
                    ModelState.AddModelError("[0].Photo", $"Photo must be less than 5 mb");
                    return View(models);
                }

                models[0].ImagePath = await models[0].Photo.CreateFileAsync(_env, "src", "images", "news");
            }
            else
            {
                ModelState.AddModelError("[0].Photo", "Image is empty");
                return View(models);
            }
            #endregion

            News? temp = await _db.News.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            string currentUsername = _userManager.GetUserName(HttpContext.User);
            foreach (News item in models)
            {
                item.ImagePath = models[0].ImagePath;
                item.LanguageGroup = temp != null ? temp.LanguageGroup + 1 : 1;
                item.CreatedAt = DateTime.UtcNow.AddHours(4);
                item.CreatedBy = currentUsername;
                await _db.News.AddAsync(item);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {

            if (id == null) return BadRequest();

            List<Language> languages = await _db.Languages.ToListAsync();
            ViewBag.Languages = languages;

            News? firstNews = await _db.News.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);

            if (firstNews == null) return NotFound();

            List<News> news = await _db.News
                .Where(c => c.LanguageGroup == firstNews.LanguageGroup && c.IsDeleted == false).ToListAsync();

            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, List<News> news)
        {
            ViewBag.Languages = await _db.Languages.ToListAsync();

            if (!ModelState.IsValid) return View(news);

            if (id == null) return BadRequest();

            News? firstNews = await _db.News.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);
            if (firstNews == null) return NotFound();

            List<News> dbNewss = await _db.News.Where(c => c.LanguageGroup == firstNews.LanguageGroup && c.IsDeleted == false).ToListAsync();

            if (dbNewss == null || dbNewss.Count == 0) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(news);
            }

            #region Image
            if (news[0].Photo != null)
            {
                if (!(news[0].Photo.CheckFileContenttype("image/jpeg") || news[0].Photo.CheckFileContenttype("image/png")))
                {
                    ModelState.AddModelError("[0].Photo", $"{news[0].Photo.FileName} is not the correct format");
                    return View(news);
                }

                if (news[0].Photo.CheckFileLength(5120))
                {
                    ModelState.AddModelError("[0].Photo", $"Photo must be less than 5 mb");
                    return View(news);
                }

                string previousFilePath = dbNewss[0].ImagePath;
                if (previousFilePath != null)
                {
                    FileHelper.DeleteFile(previousFilePath, _env, "src", "images", "news");
                }
                string imagePath = await news[0].Photo.CreateFileAsync(_env, "src", "images", "news");
                foreach (News news1 in dbNewss)
                {
                    news1.ImagePath = imagePath;
                }
            }
            #endregion

            string? currentUsername = _userManager.GetUserName(HttpContext.User);
            foreach (News item in news)
            {
                News? dbNews = dbNewss.FirstOrDefault(s => s.LanguageId == item.LanguageId);
                dbNews.Title = item.Title.Trim();
                dbNews.Description = item.Description.Trim();
                dbNews.UpdatedAt = DateTime.UtcNow.AddHours(4);
                dbNews.UpdatedBy = currentUsername;
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            News? temp = await _db.News.Include(a => a.Language).FirstOrDefaultAsync(s => s.IsDeleted == false && s.Id == id);
            if (temp == null) return NotFound();

            News? news = await _db.News
                .Include(x => x.Language)
                .FirstOrDefaultAsync(x => x.LanguageGroup == temp.LanguageGroup && x.Language!.Culture == CultureInfo.CurrentCulture.Name); ;
            if (news == null) return BadRequest();
            return View(news);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            List<Language> languages = await _db.Languages.ToListAsync();
            ViewBag.Languages = languages;

            News? firstNews = await _db.News.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);

            if (firstNews == null) return NotFound();

            List<News> newses = await _db.News
                                       .Where(c => c.LanguageGroup == firstNews.LanguageGroup && c.IsDeleted == false)
                                       .ToListAsync();

            string? currentUsername = _userManager.GetUserName(HttpContext.User);
            foreach (News news in newses)
            {
                if (news == null) return NotFound();
                FileHelper.DeleteFile(news.ImagePath, _env, "src", "images", "news");
                news.IsDeleted = true;
                news.DeletedBy = currentUsername;
                news.DeletedAt = DateTime.UtcNow.AddHours(4);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        #endregion
    }
}
