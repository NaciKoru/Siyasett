using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Siyasett.Data.Data;
using Siyasett.Data;
using Siyasett.Web.Models;
using Siyasett.Core.Addresses;
using Siyasett.Core.Faq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Siyasett.Core.Chronology;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Drawing.Printing;
using Siyasett.Core;
using System.Security.Claims;

namespace Siyasett.Web.Areas.Admin.Controllers
{

    public class FaqManagementController : AdminController
    {
        private readonly IWebHostEnvironment _host;
        public FaqManagementController(ILogger<FaqManagementController> logger, IWebHostEnvironment env, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {
            _host = env;
        }
        public async Task<IActionResult> Index(int page = 1, int pagesize = 30, string query = "")
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;

            var liste = await (from a in context.Faqs
                               orderby a.CreateDate descending
                               select new FaqCreateModel
                               {
                                   Id = a.Id,
                                   FaqQuestionTr = a.QuestionTr,
                                   FaqQuestionEn = a.QuestionEn,
                                   FaqAnswerTr = a.AnswerTr,
                                   FaqAnswerEn = a.AnswerEn,
                                   FaqGroupId = a.FaqGroupId,
                               }
                       ).AsNoTracking().Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).ToListAsync();
            ViewBag.FaqGroups = context.FaqGroups.ToList();
            return View(liste);
        }

        public async Task<IActionResult> New()
        {
            ViewBag.FaqGroups = context.FaqGroups.ToList();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> New(FaqCreateModel model, int durum)
        {
            if (ModelState.IsValid)
            {

                var faq = new Faq();
                faq.QuestionTr = model.FaqQuestionTr;
                faq.QuestionEn = model.FaqQuestionEn;
                faq.AnswerTr = model.FaqAnswerTr;
                faq.AnswerEn = model.FaqAnswerEn;
                faq.CreateDate = DateTime.Now;
                faq.FaqGroupId = model.FaqGroupId;

                context.Faqs.Add(faq);

                AddToastMessage("S.S.S", "Yeni kayıt başarıyla eklendi", Siyasett.Models.ToastType.success);
                context.SaveChanges();

                ViewBag.Faqs = await context.Faqs.OrderBy(c => c.CreateDate).AsNoTracking().ToListAsync();
                if (durum == 1)
                {
                    ViewBag.FaqGroups = context.FaqGroups.ToList();
                    return RedirectToAction("Edit", new { id = faq.Id });
                }
                else if (durum == 2)
                {
                    ViewBag.Faqs = await context.Faqs.OrderByDescending(c => c.CreateDate).AsNoTracking().ToListAsync();
                    return RedirectToAction("Index");
                }
                else if (durum == 3)
                {
                    ViewBag.FaqGroups = context.FaqGroups.ToList();
                    return RedirectToAction("New");

                }
                else if (durum == 4)
                {
                    ViewBag.FaqGroups = context.FaqGroups.ToList();
                    return View();

                }

            }
            ViewBag.Faqs = await context.Faqs.OrderByDescending(c => c.CreateDate).AsNoTracking().ToListAsync();
            ViewBag.FaqGroups = context.FaqGroups.ToList();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {


            FaqCreateModel? model = await (from a in context.Faqs
                                           where a.Id == id
                                           select new FaqCreateModel
                                           {
                                               Id = a.Id,
                                               FaqQuestionEn = a.QuestionEn,
                                               FaqQuestionTr = a.QuestionTr,
                                               FaqAnswerEn = a.AnswerEn,
                                               FaqAnswerTr = a.AnswerTr,
                                               FaqGroupId = a.FaqGroupId,

                                           }
                                       ).FirstOrDefaultAsync();


            ViewBag.Prev = await context.Chronologies.Where(i => i.Id < id).OrderByDescending(i => i.Id).Select(i => new BaseModel { Id = i.Id }).FirstOrDefaultAsync();
            ViewBag.Next = await context.Chronologies.Where(i => i.Id > id).OrderBy(i => i.Id).Select(i => new BaseModel { Id = i.Id }).FirstOrDefaultAsync();
            ViewBag.Parties = await context.Parties.OrderBy(c => c.ShortName).AsNoTracking().ToListAsync();
            ViewBag.FaqGroups = context.FaqGroups.ToList();

            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> Edit(FaqCreateModel model, int durum)
        {


            if (ModelState.IsValid)
            {

                var faq = context.Faqs.FirstOrDefault(a => a.Id == model.Id);
                faq.QuestionTr = model.FaqQuestionTr;
                faq.QuestionEn = model.FaqQuestionEn;
                faq.AnswerTr = model.FaqAnswerTr;
                faq.AnswerEn = model.FaqAnswerEn;
                faq.CreateDate = DateTime.Now;
                faq.FaqGroupId = model.FaqGroupId;
                context.SaveChanges();

                AddToastMessage("S.S.S", "Kayıt başarıyla güncellendi.", Siyasett.Models.ToastType.success);
                ViewBag.Parties = await context.Parties.OrderBy(c => c.ShortName).AsNoTracking().ToListAsync();

                if (durum == 3)
                {
                    ViewBag.FaqGroups = context.FaqGroups.ToList();
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                else if (durum == 2)
                {
                    ViewBag.Faqs = await context.Faqs.OrderByDescending(c => c.CreateDate).AsNoTracking().ToListAsync();
                    return RedirectToAction("Index");
                }

            }
            ViewBag.Faqs = await context.Faqs.OrderByDescending(c => c.CreateDate).AsNoTracking().ToListAsync();
            ViewBag.FaqGroups = context.FaqGroups.ToList();
            return View(model);

        }

        public async Task<IActionResult> Delete(int id)
        {

            var del = context.Faqs.FirstOrDefault(a => a.Id == id);
            context.Faqs.Remove(del);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> NewTitle(string nametr, string nameen)
        {
            try
            {
                var newItem = new FaqGroup
                {
                    NameTr = nametr,
                    NameEn = nameen,
                };

                if (context.AddressTypes.Any(i => i.NameTr == nametr || i.NameEn == nameen))
                    return Ok(new { success = false, msg = "This item already exists." });

                context.FaqGroups.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

    }
}
