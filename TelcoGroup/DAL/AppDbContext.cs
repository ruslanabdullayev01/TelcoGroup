using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TelcoGroup.Models;

namespace TelcoGroup.DAL
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Appeal> Appeals { get; set; }
        public DbSet<Bio> Bios { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<PartnerSlider> PartnerSliders { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<Header> Headers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Language>().HasData(
                new Language() { Id = 1, Name = "azLanguageName", Culture = "az-Latn-AZ" },
                new Language() { Id = 2, Name = "enLanguageName", Culture = "en-US"}
                );
        }
    }
}
