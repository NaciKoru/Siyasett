using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siyasett.Core;
using Siyasett.Core.Chronology;
using Siyasett.Data;
using Siyasett.Data.Data;
using Siyasett.Models;
using Siyasett.Web.Models;
using System.Text.RegularExpressions;

namespace Siyasett.Web.Areas.Admin.Controllers
{


    public class ChronologyManagementController : AdminController
    {
        private readonly IWebHostEnvironment _host;
        public ChronologyManagementController(ILogger<AdminController> logger, IWebHostEnvironment env, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {
            _host = env;
        }

        public async Task<IActionResult> Index(int page = 1, int pagesize = 30, string query = "")
        {

            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;

            var liste = await(from a in context.Chronologies
                              where string.IsNullOrEmpty(query) || (a.DescriptionTr).Contains(query) 
                              orderby a.EventDate descending
                              select new ChronologyListModel
                              {
                                  Id = a.Id,
                                  EventDate=a.EventDate,
                                  DescriptionTr=a.DescriptionTr,
                                  PartyNames = context.ChronologiesParties.Where(b => b.Chronologyid == a.Id).Select(d=>context.Parties.FirstOrDefault(e=>e.Id==d.Partyid).ShortName).ToArray()
                              }
                       ).AsNoTracking().Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).ToListAsync();

            pager.Total = await(from a in context.Chronologies
                                where string.IsNullOrEmpty(query) || (a.DescriptionTr).Contains(query)
                                select new { a.Id }).AsNoTracking().CountAsync();

            ViewBag.Pager = pager;
            return View(liste);
        }

        public async Task<IActionResult> New()
        {
            ChronologyCreateModel model = new ChronologyCreateModel();
            ViewBag.Parties = await context.Parties.OrderBy(c=>c.ShortName).AsNoTracking().ToListAsync();

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> New(ChronologyCreateModel model, int durum)
        {
            DateTime eventDate;
            var culture = System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR");
            if (!DateTime.TryParseExact(model.EventDateStr, "dd.MM.yyyy", culture, System.Globalization.DateTimeStyles.None, out eventDate))
                ModelState.AddModelError("", "Seçilen tarih geçerli değil");

            if (ModelState.IsValid)
            {
                using (var trans = await context.Database.BeginTransactionAsync())
                {
                    var chronology = new Chronology();
                    chronology.KeywordsTr = model.KeywordTr;
                    chronology.KeywordsEn = model.KeywordEn;

                    chronology.DescriptionEn = model.DescriptionEn;
                    chronology.DescriptionTr = model.DescriptionTr;
                    chronology.CreatedDate = DateTime.Now;
                    chronology.EventDate = DateOnly.FromDateTime(eventDate);
                    chronology.UpdatedDate = DateTime.Now;
                    chronology.CreatedUserId = this.appUser.Id;
                    chronology.UpdatedUserId = this.appUser.Id;

                    context.Chronologies.Add(chronology);
                    await context.SaveChangesAsync();

                    foreach (var item in model.PartyIds)
                    {
                        var chroParty = new ChronologiesParty();
                        chroParty.Partyid = item;
                        chroParty.Chronologyid = chronology.Id;
                        context.ChronologiesParties.Add(chroParty);
                        await context.SaveChangesAsync();
                    }

                    AddToastMessage("Kronoloji", "Yeni kayıt başarıyla eklendi", Siyasett.Models.ToastType.success);
                    await trans.CommitAsync();
                    ViewBag.Parties = await context.Parties.OrderBy(c => c.ShortName).AsNoTracking().ToListAsync();
                    if (durum == 1)
                    {
                        return RedirectToAction("Edit", new { id = chronology.Id });
                    }
                    else if (durum == 2)
                    {

                        return RedirectToAction("Index");
                    }
                    else if (durum == 3)
                    {
                        return RedirectToAction("New");

                    }
                    else if (durum == 4)
                    {
                        return View();

                    }

                }
            }
            ViewBag.Parties = await context.Parties.OrderBy(c => c.ShortName).AsNoTracking().ToListAsync();

            return View(model);

        }



        public async Task<IActionResult> Edit(int id)
        {


            ChronologyCreateModel? model = await (from a in context.Chronologies
                                              where a.Id == id
                                              select new ChronologyCreateModel
                                              {
                                                  Id = a.Id,
                                                  CreatedDate = a.CreatedDate,
                                                  DescriptionEn=a.DescriptionEn,
                                                  DescriptionTr=a.DescriptionTr,
                                                  EventDateStr=a.EventDate.ToString("dd.MM.yyyy"),
                                                  KeywordEn=a.KeywordsEn,
                                                  KeywordTr=a.KeywordsTr,
                                                  UpdatedDate = a.UpdatedDate,
                                                  PartyIds = context.ChronologiesParties.Where(b => b.Chronologyid == a.Id).Select(d => d.Partyid).ToArray()

                                              }
                                       ).FirstOrDefaultAsync();


            ViewBag.Prev = await context.Chronologies.Where(i => i.Id < id).OrderByDescending(i => i.Id).Select(i => new BaseModel { Id = i.Id}).FirstOrDefaultAsync();
            ViewBag.Next = await context.Chronologies.Where(i => i.Id > id).OrderBy(i => i.Id).Select(i => new BaseModel { Id = i.Id }).FirstOrDefaultAsync();
            ViewBag.Parties = await context.Parties.OrderBy(c => c.ShortName).AsNoTracking().ToListAsync();


            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChronologyCreateModel model,int durum)
        {

            DateTime eventDate;
            var culture = System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR");
            if (!DateTime.TryParseExact(model.EventDateStr, "dd.MM.yyyy", culture, System.Globalization.DateTimeStyles.None, out eventDate))
                ModelState.AddModelError("", "Seçilen tarih geçerli değil");
            if (ModelState.IsValid)
            {
                using (var trans = await context.Database.BeginTransactionAsync())
                {
                    var chron = context.Chronologies.FirstOrDefault(a => a.Id == model.Id);
                    chron.UpdatedDate = DateTime.Now;
                    chron.EventDate = DateOnly.FromDateTime(eventDate);
                    chron.DescriptionEn = model.DescriptionEn;
                    chron.DescriptionTr = model.DescriptionTr;
                    chron.UpdatedUserId = this.appUser.Id;
                    chron.KeywordsEn = model.KeywordEn;
                    chron.KeywordsTr = model.KeywordTr;
                    context.SaveChanges();
                    await context.SaveChangesAsync();

                    var chronpartyids = context.ChronologiesParties.Where(a => a.Chronologyid == model.Id).Select(b => b.Partyid).ToList();


                    foreach (var item in model.PartyIds)
                    {
                        if (!chronpartyids.Contains(item))
                        {
                            var chroParty = new ChronologiesParty();
                            chroParty.Partyid = item;
                            chroParty.Chronologyid = chron.Id;
                            context.ChronologiesParties.Add(chroParty);
                            await context.SaveChangesAsync();
                        }
                        
                    }
                    foreach (var item in chronpartyids)
                    {
                        if (!model.PartyIds.Contains(item))
                        {
                            var chroParty = context.ChronologiesParties.FirstOrDefault(a => a.Chronologyid == model.Id && a.Partyid == item);
                            context.ChronologiesParties.Remove(chroParty);
                            await context.SaveChangesAsync();
                        }

                    }
                    await trans.CommitAsync();
                    AddToastMessage("Kronoloji", "Kayıt başarıyla güncellendi.", Siyasett.Models.ToastType.success);
                    ViewBag.Parties = await context.Parties.OrderBy(c => c.ShortName).AsNoTracking().ToListAsync();

                    if (durum == 3)
                    {
                        return RedirectToAction("Edit", new { id = model.Id });
                    }
                    else if (durum == 2)
                    {

                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.Parties = await context.Parties.OrderBy(c => c.ShortName).AsNoTracking().ToListAsync();
            return View(model);

        }

        public async Task<IActionResult> Delete(int id)
        {
            using (var trans = await context.Database.BeginTransactionAsync())
            {
                var del = context.ChronologiesParties.Where(a => a.Chronologyid == id).ToList();
                foreach (var item in del)
                {
                    context.ChronologiesParties.Remove(item);
                }
                var delObject = context.Chronologies.FirstOrDefault(a => a.Id == id);
                context.Chronologies.Remove(delObject);
                await context.SaveChangesAsync();
                await trans.CommitAsync();

            }
            return RedirectToAction("Index");

        }

    }
}
