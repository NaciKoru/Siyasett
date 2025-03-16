
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siyasett.Core;
using Siyasett.Core.Extensions;
using Siyasett.Core.Poll;
using Siyasett.Data;
using Siyasett.Data.Data;
using Siyasett.Models;
using Siyasett.Web.Models;
using System.Globalization;
using System.Security.Claims;


namespace Siyasett.Web.Areas.Admin.Controllers
{
  
    public class PollsController : AdminController
    {
        private readonly IWebHostEnvironment _host;
        
        public PollsController(ILogger<AdminController> logger, IWebHostEnvironment env, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {
            _host = env;
            
        }

        public async Task<IActionResult> Index(int page = 1,byte sort = 0, byte desc = 0, int pagesize = 30, int company = 0, double partyRatio=0.0,int sampleSize=0,int year=0,int month=0,int partyId=0)
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;
            string[] orderFields = new string[] { "PollCompanyNameTr","Year","Month","SampleSize","Ratio" };
            pager.SetQuery(Request.Query);
            PollSearchModel searchModel = new PollSearchModel();
            searchModel.CompanyId = company;
            searchModel.Year = year;
            searchModel.Month = month;
            searchModel.SampleSize = sampleSize;
            searchModel.PartyRatio = partyRatio;
            searchModel.OrderBy = sort;
            searchModel.OrderByDesc = desc;

            var liste = await (from a in context.Polls
                               orderby a.Year descending,a.Month descending,a.PollCompany.Name
                               where (searchModel.Year==0 || searchModel.Year==a.Year) && (searchModel.Month==0 || searchModel.Month==a.Month) && (searchModel.CompanyId==0 || searchModel.CompanyId==a.PollCompanyId)
                               select new PollModel
                               {
                                   Id = a.Id,
                                   PollCompanyId = a.PollCompanyId,
                                   PollCompanyNameTr = a.PollCompany.ShortName,
                                   //PollKindId = a.PollKindId,
                                   //PollKindName = a.PollKind.NameTr,
                                   Month = a.Month,
                                   CreatedDate = a.CreatedDate,
                                   Active = a.Active,
                                   //PollTypeName = a.PollType.NameTr,
                                   //PollTypeId = a.PollTypeId,
                                   SampleSize = a.SampleSize,
                                   Year = a.Year,
                                   UpdatedDate = a.UpdatedDate,
                                   Results = a.PollResults.Select(i => new PollResultModel
                                   {
                                       PollId= a.Id,
                                       PollResultId = i.Id,
                                       PartyName = i.Party.Name,
                                       PartyRatio = i.Ratio,
                                       PartyId=i.PartyId,

                                   }).ToList(),
                                   Ratio=a.PollResults.FirstOrDefault(i=>i.PartyId==partyId).Ratio,
                               }
                       ).OrderByField(orderFields[searchModel.OrderBy],searchModel.OrderByDesc==0).Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).AsNoTracking().ToListAsync();

            ViewBag.Companies = await context.Companies.AsNoTracking().ToListAsync();
            pager.Total = await (from a in context.Polls
                                 orderby a.Year descending, a.Month descending, a.PollCompany.Name
                                 where (searchModel.Year == 0 || searchModel.Year == a.Year) && (searchModel.Month == 0 || searchModel.Month == a.Month) && (searchModel.CompanyId == 0 || searchModel.CompanyId == a.PollCompanyId)
                                 select new { a.Id }).AsNoTracking().CountAsync();
            ViewBag.SearchModel = searchModel;
            ViewBag.Pager = pager;
            return View(liste);

        }

        public async Task<IActionResult> New()
        {
            PollEditModel model = new PollEditModel();
            model.Month = DateTime.Today.Month;
            model.Year = DateTime.Today.Year;
            model.Day = DateTime.Today.Day;
            ViewBag.Companies = await context.Companies.AsNoTracking().ToListAsync();
            ViewBag.Types = await context.PollTypes.AsNoTracking().ToListAsync();
            ViewBag.Kinds = await context.PollKinds.AsNoTracking().ToListAsync();
            ViewBag.Parties = await context.Parties.OrderBy(x => x.Name).AsNoTracking().ToListAsync();

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> New(PollEditModel model)
        {
           

            if (context.Polls.Any(i=>i.PollCompanyId==model.PollCompanyId  && model.Year==i.Year && i.Month==model.Month && ((i.Day !=null && i.Day==model.Day)|| (i.Day==null))))
                ModelState.AddModelError("","Seçilen değerlerde bir anket zaten kayıtlı.");

            if (ModelState.IsValid)
            {
                using (var trans = await context.Database.BeginTransactionAsync())
                {
                    try
                    {


                        var poll = new Poll()
                        {
                            Active = true,
                            CreatedDate = DateTime.Now,
                            CreatedUserId = this.appUser.Id,
                            //PollKindId = model.PollKindId,
                            Month = model.Month,
                            PollCompanyId = model.PollCompanyId,
                            SampleSize = model.SampleSize,
                            //PollTypeId = model.PollTypeId,
                            Year = model.Year,
                            Day= model.Day,
                        };

                        context.Polls.Add(poll);
                        await context.SaveChangesAsync();

                        foreach (var item in model.Results)
                        {
                            var pr = new PollResult
                            {
                                Active = true,
                                CreatedDate = DateTime.Now,
                                CreatedUserId = this.appUser.Id,
                                PartyId = item.PartyId,
                                PollId = poll.Id,
                                Ratio = item.Ratio,
                                UpdatedUserId = this.appUser.Id,
                                UpdatedDate = DateTime.Now,

                            };

                            context.PollResults.Add(pr);
                            await context.SaveChangesAsync();


                        }


                        await trans.CommitAsync();
                        //AddToastMessage("Yeni Anket", "Yeni anket başarıyla eklendi", Siyasett.Models.ToastType.success);

                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        await trans.RollbackAsync();
                        ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu.");
                    }
                }
            }



            ViewBag.Companies = await context.Companies.AsNoTracking().ToListAsync();
            ViewBag.Types = await context.PollTypes.AsNoTracking().ToListAsync();
            ViewBag.Kinds = await context.PollKinds.AsNoTracking().ToListAsync();
            ViewBag.Parties = await context.Parties.OrderBy(x => x.Name).AsNoTracking().ToListAsync();

            return View(model);

        }

        public async Task<IActionResult> GetChartData(int id)
        {
            var model = await (from a in context.Polls
                               where a.Id == id
                               select new 
                               {
                                   Id = a.Id,
                                   PollCompanyId = a.PollCompanyId,
                                   
                                   //PollKindId = a.PollKindId,

                                   Month = a.Month,
                                   CompanyName=a.PollCompany.ShortName,


                                   //PollTypeId = a.PollTypeId,

                                   Year = a.Year,
                                   Active = a.Active,
                                   SampleSize = a.SampleSize,
                                   Results = a.PollResults.Select(i => new PollResultEditModel
                                   {
                                       PartyId = i.PartyId,
                                       Id = i.Id,
                                       PartyName = i.Party.ShortName,
                                       Ratio = i.Ratio,

                                   }).OrderByDescending(i=>i.Ratio).ToList()

                               }
                           ).FirstOrDefaultAsync();

            return Ok(new { 
                title=$"{model.Year}-{model.Month.ToString("00")} {model.CompanyName}",
                data = model.Results.Select(i => Math.Round( i.Ratio,1)).ToList(), 
                categories = model.Results.Select(i => i.PartyName).ToList() });

        }

            //[HttpPost]
            //public async Task<IActionResult> NewCompany(string nametr, string nameen)
            //{
            //    try
            //    {
            //        var newItem = new PollCompany
            //        {
            //            NameTr = nametr,
            //            NameEn = nameen,
            //            CreatedDate = DateTime.Now,
            //            UpdatedDate = DateTime.Now,
            //            CreatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
            //            UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
            //            IsActive = true
            //        };

            //        if (context.PollCompanies.Any(i => i.NameTr == nametr || i.NameEn == nameen))
            //            return Ok(new { success = false, msg = "This item already exists." });

            //        context.PollCompanies.Add(newItem);
            //        await context.SaveChangesAsync();
            //        return Ok(new { success = true, id = newItem.Id });
            //    }
            //    catch
            //    {
            //        return Ok(new { success = false, msg = "An error occured." });
            //    }
            //}

            public async Task<IActionResult> Edit(int id)
        {

            var model = await (from a in context.Polls
                                      where a.Id == id
                                      select new PollEditModel
                                      {
                                          Id = a.Id,
                                          PollCompanyId = a.PollCompanyId,
                                          
                                          //PollKindId = a.PollKindId,

                                          Month = a.Month,
                                          Day=a.Day,


                                          //PollTypeId = a.PollTypeId,
                                          
                                          Year = a.Year,
                                          Active = a.Active,
                                          SampleSize=a.SampleSize,
                                          Results=a.PollResults.Select(i=>new PollResultEditModel
                                          {
                                              PartyId=i.PartyId,
                                              Id=i.Id,
                                              PartyName=i.Party.Name,
                                              Ratio=i.Ratio,
                                          }).ToList()

                                      }
                                       ).FirstOrDefaultAsync();



            ViewBag.Companies = await context.Companies.AsNoTracking().ToListAsync();

            //ViewBag.Types = await context.PollTypes.AsNoTracking().ToListAsync();
            //ViewBag.Kinds = await context.PollKinds.AsNoTracking().ToListAsync();
            ViewBag.Parties = await context.Parties.AsNoTracking().ToListAsync();
            ViewBag.Prev = await context.Polls.Where(i => i.Id < id).OrderByDescending(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name=context.Companies.FirstOrDefault(j=>j.Id==i.PollCompanyId).Name }).FirstOrDefaultAsync();
            ViewBag.Next = await context.Polls.Where(i => i.Id > id).OrderBy(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = context.Companies.FirstOrDefault(j => j.Id == i.PollCompanyId).Name }).FirstOrDefaultAsync();

            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> Edit(PollEditModel model)
        {

            var poll = context.Polls.FirstOrDefault(x => x.Id == model.Id);

            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {

                    poll.UpdatedDate = DateTime.Now;
                    poll.UpdatedUserId = this.appUser.Id;
                    poll.Month = model.Month;
                    poll.PollCompanyId = model.PollCompanyId;
                    poll.SampleSize = model.SampleSize;
                    poll.Year = model.Year;
                    poll.Day= model.Day;
                    await context.SaveChangesAsync();

                    var recordedList = await context.PollResults.Where(i => i.Id == model.Id).ToListAsync();
                    foreach (var item in recordedList)
                    {
                        if (!model.Results.Any(i => i.Id == item.Id))
                            context.PollResults.Remove(item);
                    }
                    await context.SaveChangesAsync();


                    foreach (var item in model.Results)
                    {
                        if (item.Id > 0)
                        {
                            var result = context.PollResults.FirstOrDefault(a => a.Id == item.Id);
                            result.UpdatedDate = DateTime.Now;
                            result.UpdatedUserId = this.appUser.Id;
                            result.PollId = poll.Id;
                            result.PartyId = item.PartyId;
                            result.Ratio = item.Ratio;
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            PollResult result = new();
                            result.PollId = poll.Id;

                            result.UpdatedDate = DateTime.Now;
                            result.UpdatedUserId = this.appUser.Id;

                            result.PartyId = item.PartyId;
                            result.Ratio = item.Ratio;
                            context.PollResults.Add(result);
                            await context.SaveChangesAsync();

                        }
                    }
                    await trans.CommitAsync();

                    //AddToastMessage("Anket Düzenleme", "Anket başarıyla güncellendi", Siyasett.Models.ToastType.success);
                }
                catch
                {
                    await trans.RollbackAsync();
                    ModelState.AddModelError("", "Kayıt sırasında hata oluştu");
                }

            }



            ViewBag.Companies = await context.Companies.AsNoTracking().ToListAsync();

            //ViewBag.Types = await context.PollTypes.AsNoTracking().ToListAsync();
            //ViewBag.Kinds = await context.PollKinds.AsNoTracking().ToListAsync();
            ViewBag.Parties = await context.Parties.OrderBy(x => x.Name).AsNoTracking().ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {

            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {

                    var delObject = context.PollResults.Where(a => a.PollId == id);

                    foreach (var item in delObject)
                    {
                        context.PollResults.Remove(item);
                    }
                    await context.SaveChangesAsync();
                    var delpoll = context.Polls.FirstOrDefault(a => a.Id == id);
                    context.Polls.Remove(delpoll);
                    await context.SaveChangesAsync();
                    await trans.CommitAsync();
                    
                }
                catch
                {
                    await trans.RollbackAsync();
                }

            }


            return Ok();

        }

    }
}
