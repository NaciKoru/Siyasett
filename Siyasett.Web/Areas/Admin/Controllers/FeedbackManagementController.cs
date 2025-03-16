using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Siyasett.Data.Data;
using Siyasett.Data;
using Siyasett.Core;
using Siyasett.Web.Models;
using Microsoft.EntityFrameworkCore;
using Siyasett.Core.Faq;
using Siyasett.Core.PoliticalLibrary;
using Siyasett.Core.FeedBack;
using Siyasett.Core.Extensions;
using Siyasett.Models;

namespace Siyasett.Web.Areas.Admin.Controllers
{
  
    public class FeedbackManagementController : AdminController
    {
        private readonly IWebHostEnvironment _host;
        public FeedbackManagementController(ILogger<FaqManagementController> logger, IWebHostEnvironment env, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {
            _host = env;
        }


        public async Task<IActionResult> Index(int page = 1, int pagesize = 30, string query = "", byte sort = 0, byte desc = 1)
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;

            string[] orderFields = new string[] { "SendTime", "Name", "LastName", "Topic", "Text", "Answer", "AnswerTime" };
            FeedBackSearchModel searchModel = new FeedBackSearchModel();
            searchModel.OrderBy = sort;
            searchModel.OrderByDesc = desc;
            var normQuery = query.ToLower();

            var liste = await (from a in context.Feedbacks
                               where (string.IsNullOrEmpty(query)  || (!string.IsNullOrEmpty(query) && a.Name.ToLower().Contains(normQuery)))
                               || (string.IsNullOrEmpty(query)  || (!string.IsNullOrEmpty(query) && a.SendTime != null && a.SendTime.ToString().ToLower().Contains(normQuery)))
                               || (string.IsNullOrEmpty(query)  || (!string.IsNullOrEmpty(query) && string.IsNullOrEmpty(a.Lastname) != true && a.Lastname.ToLower().Contains(normQuery)))
                               || (string.IsNullOrEmpty(query)  || (!string.IsNullOrEmpty(query) && string.IsNullOrEmpty(a.Topic) != true && a.Topic.ToLower().Contains(normQuery)))
                               || (string.IsNullOrEmpty(query)  || (!string.IsNullOrEmpty(query) && string.IsNullOrEmpty(a.Message) != true && a.Message.ToLower().Contains(normQuery)))
                               || (string.IsNullOrEmpty(query)  || (!string.IsNullOrEmpty(query) && string.IsNullOrEmpty(a.Answer) != true && a.Answer.ToLower().Contains(normQuery)))
                               || (string.IsNullOrEmpty(query)  || (!string.IsNullOrEmpty(query) && a.AnswerTime != null && a.AnswerTime.ToString().ToLower().Contains(normQuery)))
                               select new FeedBackModel
                               {
                                   Id = a.Id,
                                   Name=a.Name,
                                   LastName=a.Lastname,
                                   Email=a.Email,
                                   Text=a.Message,
                                   PageUrl=a.PageLink,
                                   Answer=a.Answer!=null?a.Answer:"",
                                   SendTime=(DateTime)a.SendTime,
                                   AnswerTime=(DateTime)a.AnswerTime,
                                   Topic=a.Topic,
                                   Title=a.Title,
                               }
                       ).OrderByField(orderFields[searchModel.OrderBy], searchModel.OrderByDesc == 0).Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).AsNoTracking().ToListAsync();

            ViewBag.SearchModel = searchModel;
            ViewBag.Query = query;

            return View(liste);

        }

       public async Task<IActionResult> DeleteFeedBack(int id)
        {

            try
            {
                using (var trans = await context.Database.BeginTransactionAsync())
                {
                    var feedback = await context.Feedbacks.FirstOrDefaultAsync(i => i.Id == id);
                    if (feedback == null)
                        return RedirectToAction("Index");


                    context.Feedbacks.Remove(feedback);
                    await context.SaveChangesAsync();
                    await trans.CommitAsync();
                }

                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }

        }
    }
}
