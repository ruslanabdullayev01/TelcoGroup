﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TelcoGroup.DAL;
using TelcoGroup.Models;

namespace TelcoGroup.Controllers
{
    public class SolutionsController : Controller
    {
        private readonly AppDbContext _db;
        public SolutionsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Solution> solutions = await _db.Solutions
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => !x.IsDeleted && x.Language.Culture == CultureInfo.CurrentCulture.Name)
                .ToListAsync();
            ViewBag.Header = await _db.Headers.FirstOrDefaultAsync(x => x.PageKey == "Solutions" && x.Language!.Culture == CultureInfo.CurrentCulture.Name);
            return View(solutions);
        }
    }
}
