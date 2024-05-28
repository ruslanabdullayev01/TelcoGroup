using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Globalization;
using System.Text;
using TelcoGroup.DAL;
using TelcoGroup.Helpers;
using TelcoGroup.Models;
using TelcoGroup.ViewModels;

namespace TelcoGroup.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _con;
        private readonly SmtpSetting _smtpSetting;

        public HomeController(AppDbContext db, IConfiguration con, SmtpSetting smtpSetting)
        {
            _db = db;
            _con = con;
            _smtpSetting = smtpSetting;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                AboutUs = await _db.AboutUs.FirstOrDefaultAsync(x => !x.IsDeleted && x.Language!.Culture == CultureInfo.CurrentCulture.Name),
                PartnerSliders = await _db.PartnerSliders.Where(x => !x.IsDeleted).ToListAsync(),
                LatestNews = await _db.News
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => !x.IsDeleted && x.Language!.Culture == CultureInfo.CurrentCulture.Name)
                .Take(4)
                .ToListAsync(),
                Services = await _db.Services
                    .Where(x => !x.IsDeleted && x.Language!.Culture == CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(4)
                    .ToListAsync(),
                Solutions = await _db.Solutions
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => !x.IsDeleted && x.Language!.Culture == CultureInfo.CurrentCulture.Name)
                .Take(4)
                .ToListAsync(),
                MainNews = await _db.News
                .Where(x=>!x.IsDeleted && x.IsMain && x.Language!.Culture == CultureInfo.CurrentCulture.Name)
                .FirstOrDefaultAsync()
            };
            return View(homeVM);
        }

        #region Change Language
        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return Redirect(Request.Headers["Referer"].ToString());
        }
        #endregion

        #region For Form
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
        public class PreventDuplicateRequestAttribute : ActionFilterAttribute
        {
            public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.HttpContext.Request.Form.ContainsKey("__RequestVerificationToken"))
                {
                    await context.HttpContext.Session.LoadAsync();

                    var currentToken = context.HttpContext.Request.Form["__RequestVerificationToken"].ToString();
                    var lastToken = context.HttpContext.Session.GetString("LastProcessedToken");

                    if (lastToken == currentToken)
                    {
                        context.ModelState.AddModelError(string.Empty, "Looks like you accidentally submitted the same form twice.");
                    }
                    else
                    {
                        context.HttpContext.Session.SetString("LastProcessedToken", currentToken);
                        await context.HttpContext.Session.CommitAsync();
                    }
                }
                await next();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public async Task<IActionResult> Index(HomeVM homeVM)
        {
            if (homeVM.Appeal == null) return BadRequest();

            homeVM = new HomeVM()
            {
                AboutUs = await _db.AboutUs.FirstOrDefaultAsync(x=>!x.IsDeleted && x.Language!.Culture == CultureInfo.CurrentCulture.Name),
                Appeal = homeVM.Appeal,
                PartnerSliders = await _db.PartnerSliders.Where(x => !x.IsDeleted).ToListAsync(),
                LatestNews = await _db.News
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => !x.IsDeleted && x.Language!.Culture == CultureInfo.CurrentCulture.Name)
                .Take(4)
                .ToListAsync(),
                Services = await _db.Services
                    .Where(x => !x.IsDeleted && x.Language!.Culture == CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(4)
                    .ToListAsync(),
                Solutions = await _db.Solutions
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => !x.IsDeleted && x.Language!.Culture == CultureInfo.CurrentCulture.Name)
                .Take(4)
                .ToListAsync(),
            };

            if (!ModelState.IsValid)
            {
                return View("Index", homeVM);
            }

            Appeal appeal = homeVM.Appeal;

            appeal.IsDeleted = false;
            appeal.CreatedAt = DateTime.UtcNow.AddHours(4);

            #region String Builder
            StringBuilder sb = new StringBuilder();

            sb.Append("<h1>Form</h1>");
            sb.Append("<p><strong>Full Name: </strong>" + homeVM.Appeal.FullName + "</p>");
            sb.Append("<p><strong>Email: </strong>" + homeVM.Appeal.Email + "</p>");
            sb.Append("<p><strong>Phone Number: </strong>" + homeVM.Appeal.Phone + "</p>");
            sb.Append("<p><strong>Subject: </strong>" + homeVM.Appeal.Subject + "</p>");
            sb.Append("<p><strong>Message: </strong>" + homeVM.Appeal.Message + "</p>");
            sb.Append("<p><strong>Send At: </strong>" + homeVM.Appeal.CreatedAt.ToString() + "</p>");

            string htmlTable = sb.ToString();
            #endregion

            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(MailboxAddress.Parse(_smtpSetting.Email));
            mimeMessage.To.Add(MailboxAddress.Parse("officeupgradesolutions@gmail.com"));
            //mimeMessage.To.Add(MailboxAddress.Parse(homeVM.Appeal.Email));
            mimeMessage.Subject = homeVM.Appeal.Email;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = htmlTable;
            mimeMessage.Body = new TextPart("html")
            {
                Text = bodyBuilder.HtmlBody
            };

            using (SmtpClient smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(_smtpSetting.Host, _smtpSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_smtpSetting.Email, _smtpSetting.Password);
                await smtpClient.SendAsync(mimeMessage);
                await smtpClient.DisconnectAsync(true);
                smtpClient.Dispose();
            }

            await _db.Appeals.AddAsync(appeal);
            await _db.SaveChangesAsync();

            string referer = Request.Headers["Referer"].ToString();

            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("index", "home");
        }
        #endregion
    }
}
