using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;
using TelcoGroup.DAL;
using TelcoGroup.Helpers;
using TelcoGroup.Models;
using TelcoGroup.Resources;
using TelcoGroup.Services;

var builder = WebApplication.CreateBuilder(args);

#region Builder Configurations
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddRouting(options => options.LowercaseUrls = true);

#region Localization
builder.Services.AddSingleton<LanguageService>();
builder.Services.AddScoped<LayoutService>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var smtp = builder.Configuration
        .GetSection("SmtpSetting")
        .Get<SmtpSetting>();
builder.Services.AddSingleton(smtp);


builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddMvc()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(ShareResource).GetTypeInfo().Assembly.FullName);
            return factory.Create("ShareResource", assemblyName.Name);
        };
    });

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("az-Latn-AZ"),
                    new CultureInfo("en-US"),
                };
    options.DefaultRequestCulture = new RequestCulture(culture: "az-Latn-AZ", uiCulture: "az-Latn-AZ");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
});
#endregion

#region Identity
builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
    opt.Password.RequireDigit = true;
    opt.Password.RequiredLength = 8;
    opt.Password.RequiredUniqueChars = 3;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Lockout.MaxFailedAccessAttempts = 5;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._";
    opt.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppDbContext>();
#endregion

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

#region App Configurations

#region Localization
var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);
#endregion

#region DbInitialization
var scopFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

using (var scope = scopFactory.CreateScope())
{
    var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
    var db = scope.ServiceProvider.GetService<AppDbContext>();

    await DbInitializer.SeedAsync(roleManager, userManager, db);
}
#endregion

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
      name: "Admin",
      pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
#endregion

app.Run();
