using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TelcoGroup.DAL;
using TelcoGroup.Models;

namespace TelcoGroup.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _db;
        public LayoutService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<string> GetCurrentLangauge()
        {
            string CurrentLangauge = CultureInfo.CurrentCulture.Name;
            return CurrentLangauge;
        }

        public async Task<IEnumerable<Setting>> GetSettings()
        {
            IEnumerable<Setting>? settings = await _db.Settings.Include(a => a.Language).ToListAsync();
            return settings;
        }
    }
}
