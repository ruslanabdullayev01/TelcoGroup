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
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class AppealsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly SmtpSetting _smtpSetting;
        public AppealsController(AppDbContext db
            , UserManager<User> userManager
            , SmtpSetting smtpSetting)
        {
            _db = db;
            _userManager = userManager;
            _smtpSetting = smtpSetting;
        }

        #region Index
        public IActionResult Index(int pageIndex = 1, string? isReadFilter = "unread")
        {
            IQueryable<Appeal> query = _db.Appeals.Where(item =>
                (isReadFilter == "read" && item.IsRead && !item.IsDeleted) ||
                (isReadFilter == "unread" && !item.IsRead && !item.IsDeleted) ||
                (isReadFilter == "all" && !item.IsDeleted)
            ).OrderByDescending(x => x.Id);

            return View(PageNatedList<Appeal>.Create(query, pageIndex, 10, 10));
        }

        [HttpPost]
        public IActionResult Index(string? isReadFilter = "false")
        {
            return RedirectToAction("Index", isReadFilter);
        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();

            Appeal? appeal = await _db.Appeals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (appeal == null) return BadRequest();

            string? currentUsername = _userManager.GetUserName(HttpContext.User);
            appeal.IsRead = true;
            if (appeal.ReadedBy == null)
            {
                appeal.ReadedBy = currentUsername;
            }
            await _db.SaveChangesAsync();

            return View(appeal);
        }
        #endregion

        #region Reply
        //public async Task<IActionResult> Reply(int id)
        //{
        //    Appeal appeal = await _db.Appeals.FindAsync(id);
        //    return View(appeal);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Reply(int id, string adminMessage)
        //{
        //    Appeal? appeal = await _db.Appeals.FindAsync(id);
        //    if (appeal == null) return NotFound();

        //    appeal.Replied = true;

        //    var response = new ReplyAppeal
        //    {
        //        AppealId = id,
        //        AdminMessage = adminMessage
        //    };

        //    await _db.ReplyAppeals.AddAsync(response);
        //    await _db.SaveChangesAsync();

        //    #region SMTP

        //    #region String Builder
        //    StringBuilder sb = new StringBuilder();

        //    sb.Append("<h1>Reply to Form</h1>");
        //    //sb.Append("<p><strong>Full Name: </strong>" + appeal.FullName + "</p>");
        //    //sb.Append("<p><strong>Email: </strong>" + appeal.Email + "</p>");
        //    //sb.Append("<p><strong>Subject: </strong>" + appeal.Subject + "</p>");
        //    sb.Append("<p><strong>Reply: </strong>" + appeal.Message + "</p>");
        //    sb.Append("<hr />");
        //    sb.Append("<p><strong>Admin Message: </strong>" + adminMessage + "</p>");

        //    string htmlTable = sb.ToString();
        //    #endregion

        //    MimeMessage mimeMessage = new MimeMessage();
        //    mimeMessage.From.Add(MailboxAddress.Parse(_smtpSetting.Email));
        //    //mimeMessage.To.Add(MailboxAddress.Parse("ruslanabdullayev01@gmail.com"));
        //    mimeMessage.To.Add(MailboxAddress.Parse(appeal.Email));
        //    mimeMessage.Subject = appeal.Subject;
        //    var bodyBuilder = new BodyBuilder();
        //    bodyBuilder.HtmlBody = htmlTable;
        //    mimeMessage.Body = new TextPart("html")
        //    {
        //        Text = bodyBuilder.HtmlBody
        //    };

        //    using (SmtpClient smtpClient = new SmtpClient())
        //    {
        //        await smtpClient.ConnectAsync(_smtpSetting.Host, _smtpSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
        //        await smtpClient.AuthenticateAsync(_smtpSetting.Email, _smtpSetting.Password);
        //        await smtpClient.SendAsync(mimeMessage);
        //        await smtpClient.DisconnectAsync(true);
        //        smtpClient.Dispose();
        //    }
        //    #endregion

        //    return RedirectToAction(nameof(Index));
        //}
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            Appeal? appeal = await _db.Appeals
                .FirstOrDefaultAsync(b => b.Id == id && b.IsDeleted == false);

            if (appeal == null) return NotFound();

            string? currentUsername = _userManager.GetUserName(HttpContext.User);

            appeal.IsDeleted = true;
            appeal.DeletedBy = currentUsername;
            appeal.DeletedAt = DateTime.UtcNow.AddHours(4);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        #endregion
    }
}
