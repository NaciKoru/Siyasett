using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Siyasett.Data.Data;
using Siyasett.Data;
using Siyasett.Web.Models;
using Siyasett.Core.People;
using Siyasett.Core.Colums;
using Microsoft.EntityFrameworkCore;
using Siyasett.Core.Extensions;
using Siyasett.Core.PoliticalLibrary;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using NetTopologySuite.Index.HPRtree;
using System.Drawing;
using Siyasett.Core;

namespace Siyasett.Web.Areas.Admin.Controllers
{
    public class ColumsManagementController : AdminController
    {
        private readonly IWebHostEnvironment _host;
        public ColumsManagementController(ILogger<AdminController> logger, IWebHostEnvironment env, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {
            _host = env;
        }

        public async Task<IActionResult> Index(int page = 1, byte sort = 0, byte desc = 1, int pagesize = 36, string query = "", string date = "", string contexttext = "", int personId = 0, int turId = 0, int sirketId=0,int dilId=0)
        {

            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;
            string[] orderFields = new string[] { "Date", "Header", "YazarAdSoyad", "TurAdi", "SirketAdi", "DilAdi" };
            pager.SetQuery(Request.Query);
            ColumsSearchModel searchModel = new ColumsSearchModel();
            searchModel.OrderBy = sort;
            searchModel.OrderByDesc = desc;
            searchModel.ContextText = contexttext;
            searchModel.YazarId= personId;
            searchModel.TurId= turId;
            searchModel.DilId= dilId;
            searchModel.SirketId= sirketId;

            if (!string.IsNullOrEmpty(date))
                searchModel.Date = Convert.ToDateTime(date);




            var koseYazilari = await (from a in context.YynKoseYazilaris
                                      where (string.IsNullOrEmpty(date) || a.Tarih.Date == searchModel.Date.Value.Date)
                                      && (string.IsNullOrEmpty(contexttext) || a.Baslik.ToLower().Contains(contexttext.ToLower()) || (string.IsNullOrEmpty(a.BaslikEn)==false && a.BaslikEn.ToLower().Contains(contexttext.ToLower())))
                                && (searchModel.YazarId == 0 || (searchModel.YazarId == a.YazarId))
                                && (searchModel.SirketId == 0 || (searchModel.SirketId == a.SirketId))
                                && (searchModel.DilId == 0 || (searchModel.DilId == a.DilId))
                                && (searchModel.TurId == 0 || (searchModel.TurId == a.TurId))
                                      let sirket = context.Companies.FirstOrDefault(b => b.Id == a.SirketId)
                                      let tur = context.YynTurlers.FirstOrDefault(b => b.Id == a.TurId)
                                      let yazaradsoyad = context.People.FirstOrDefault(b => b.Id == a.YazarId)
                                      let dil = context.YynDillers.FirstOrDefault(b => b.Id == a.DilId)
                                      select new ColumsModel
                                      {
                                          Id = a.Id,
                                          DilId = a.DilId,
                                          TypeId = a.TurId,
                                          PeopleId = a.YazarId,
                                          CompanyId = a.SirketId,
                                          Header = a.Baslik,
                                          Context = a.Metin,
                                          Date = a.Tarih,
                                          OrjLink = a.Url,
                                          ReadCount = a.OkunmaSayisi,
                                          YazarAdSoyad = yazaradsoyad.FirstName + " " + yazaradsoyad.LastName,
                                          TurAdi = tur.Adi,
                                          SirketAdi = sirket.Name,
                                          DilAdi = dil.Adi,
                                      }).OrderByField(orderFields[searchModel.OrderBy], searchModel.OrderByDesc == 0).Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).AsNoTracking().ToListAsync();


            var varolankoseyazarlari = koseYazilari.Select(a => a.PeopleId).Distinct().ToList();
            var varolansirketler = koseYazilari.Select(a => a.CompanyId).Distinct().ToList();
            var varolandiller = context.YynKoseYazilaris.Select(a => a.DilId).Distinct().ToList();
            var varolankategoriler = context.YynKoseYazilariKategorileris.Select(a => a.KategoriId).Distinct().ToList();
            var varolanturler = context.YynKoseYazilaris.Select(a => a.TurId).Distinct().ToList();


            var yazarlar = await (from a in context.People
                                  where varolankoseyazarlari.Contains(a.Id)
                                  select new PeopleListModel
                                  {
                                      LastName = a.LastName,
                                      FirstName = a.FirstName,
                                      Id = a.Id,
                                  }
    ).OrderByField("FirstName", Ascending: true).ToListAsync();

            var sirketler = await context.Companies.Where(a => a.CompanySectors.FirstOrDefault(b => b.CompanyId == a.Id) != null && varolansirketler.Contains(a.Id)).OrderBy(c => c.Name).ToListAsync();
            var diller = await context.YynDillers.Where(a => varolandiller.Contains(a.Id)).ToListAsync();
            var kategoriler = await context.YynKategorilers.Where(a => varolankategoriler.Contains(a.Id)).ToListAsync();
            var turler = await context.YynTurlers.Where(a => varolanturler.Contains(a.Id)).ToListAsync();


            pager.Total = await (from a in context.YynKoseYazilaris
                                 where (string.IsNullOrEmpty(date) || a.Tarih.Date == searchModel.Date.Value.Date)
                                     && (string.IsNullOrEmpty(contexttext) || a.Baslik.ToLower().Contains(contexttext.ToLower()) || (string.IsNullOrEmpty(a.BaslikEn) == false && a.BaslikEn.ToLower().Contains(contexttext.ToLower())))
                               && (searchModel.YazarId == 0 || (searchModel.YazarId == a.YazarId))
                               && (searchModel.SirketId == 0 || (searchModel.SirketId == a.SirketId))
                               && (searchModel.DilId == 0 || (searchModel.DilId == a.DilId))
                                 select new ColumsModel
                                 {
                                 }).AsNoTracking().CountAsync();

            ViewBag.Pager = pager;

            ViewBag.Turler = turler;
            ViewBag.Yazarlar = yazarlar;
            ViewBag.Kategoriler = kategoriler;
            ViewBag.Diller = diller;
            ViewBag.Sirketler = sirketler;
            ViewBag.SearchModel = searchModel;
            ViewBag.Query = query;

            return View(koseYazilari);
        }


        public async Task<IActionResult> New()
        {
            ColumsModel model = new ColumsModel();

            var yazarlar = await (from a in context.People
                                  where a.PeopleJobs.FirstOrDefault(b => b.JobId == 25 && b.PeopleId == a.Id) != null
                                  select new PeopleListModel
                                  {
                                      LastName = a.LastName,
                                      FirstName = a.FirstName,
                                      Id = a.Id,
                                  }
           ).OrderByField("FirstName", Ascending: true).ToListAsync();

            var sirketler = await context.Companies.ToListAsync();
            var kategoriler = await context.YynKategorilers.ToListAsync();
            var diller = await context.YynDillers.ToListAsync();
            var turler = await context.YynTurlers.ToListAsync();

            ViewBag.Turler = turler;
            ViewBag.Yazarlar = yazarlar;
            ViewBag.Kategoriler = kategoriler;
            ViewBag.Diller = diller;
            ViewBag.Sirketler = sirketler;


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> New(ColumsModel model, int durum)
        {
            var asd = model;
            if (ModelState.IsValid)
            {
                var koseYazilari = new YynKoseYazilari();
                koseYazilari.Baslik = model.Header;
                koseYazilari.Tarih = model.Date;
                koseYazilari.YazarId = model.PeopleId;
                koseYazilari.SirketId = model.CompanyId;
                koseYazilari.TurId = model.TypeId;
                koseYazilari.DilId = model.DilId;
                koseYazilari.Url = model.OrjLink;
                koseYazilari.OkunmaSayisi = model.ReadCount;
                koseYazilari.Metin = model.Context;
                koseYazilari.Keywords = model.Keywords;
                koseYazilari.BaslikEn = model.HeaderEn;
                koseYazilari.MetinEn = model.ContextEn;

                context.YynKoseYazilaris.Add(koseYazilari);
                await context.SaveChangesAsync();

                for (int i = 0; i < model.CategoryIds.Length; i++)
                {
                    context.YynKoseYazilariKategorileris.Add(new YynKoseYazilariKategorileri { KoseYazisiId = koseYazilari.Id, KategoriId = model.CategoryIds[i] });
                }

                await context.SaveChangesAsync();
                if (durum == 1)
                {
                    return RedirectToAction("New");
                }
                else if (durum == 2)
                {

                    return RedirectToAction("Index");
                }
                else if (durum == 3)
                {
                    return RedirectToAction("New");
                }

            }



            var yazarlar = await (from a in context.People
                                  select new PeopleListModel
                                  {
                                      LastName = a.LastName,
                                      FirstName = a.FirstName,
                                      Id = a.Id,
                                  }
 ).OrderByField("FirstName", Ascending: true).ToListAsync();

            var sirketler = await context.Companies.ToListAsync();
            var kategoriler = await context.YynKategorilers.ToListAsync();
            var diller = await context.YynDillers.ToListAsync();
            var turler = await context.YynTurlers.ToListAsync();

            ViewBag.Turler = turler;
            ViewBag.Yazarlar = yazarlar;
            ViewBag.Kategoriler = kategoriler;
            ViewBag.Diller = diller;
            ViewBag.Sirketler = sirketler;


            return View(model);

        }

        public async Task<IActionResult> Edit(int id)
        {

            ColumsModel? model = await (from a in context.YynKoseYazilaris
                                        where a.Id == id
                                        select new ColumsModel
                                        {
                                            Id = a.Id,
                                            Header = a.Baslik,
                                            Date = a.Tarih,
                                            PeopleId = a.YazarId,
                                            CompanyId = a.SirketId,
                                            TypeId = a.TurId,
                                            DilId = a.DilId,
                                            OrjLink = a.Url,
                                            ReadCount = a.OkunmaSayisi,
                                            Context = a.Metin,
                                            Keywords = a.Keywords,
                                            ContextEn=a.MetinEn,
                                            HeaderEn=a.BaslikEn,
                                        }
                                       ).FirstOrDefaultAsync();

            BaseModel? prev = await context.YynKoseYazilaris.Where(i => i.Id < id).OrderByDescending(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.Baslik }).FirstOrDefaultAsync();
            BaseModel? next = await context.YynKoseYazilaris.Where(i => i.Id > id).OrderBy(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.Baslik }).FirstOrDefaultAsync();
            ViewBag.Prev = prev;
            ViewBag.Next = next;

            ViewBag.KoseYazilariKategori = await (from a in context.YynKoseYazilariKategorileris
                                                  where a.KoseYazisiId == id
                                                  select new KoseYazilariKategoriModel
                                                  {
                                                      Id = a.Id,
                                                      KoseYazisiId = a.KoseYazisiId,
                                                      KategoriId = a.KategoriId,
                                                  }).ToListAsync();



            var yazarlar = await (from a in context.People
                                  where a.PeopleJobs.FirstOrDefault(b => b.JobId == 25 && b.PeopleId == a.Id) != null
                                  select new PeopleListModel
                                  {
                                      LastName = a.LastName,
                                      FirstName = a.FirstName,
                                      Id = a.Id,
                                  }
     ).OrderByField("FirstName", Ascending: true).ToListAsync();

            var sirketler = await context.Companies.ToListAsync();
            var kategoriler = await context.YynKategorilers.ToListAsync();
            var diller = await context.YynDillers.ToListAsync();
            var turler = await context.YynTurlers.ToListAsync();

            ViewBag.Turler = turler;
            ViewBag.Yazarlar = yazarlar;
            ViewBag.Kategoriler = kategoriler;
            ViewBag.Diller = diller;
            ViewBag.Sirketler = sirketler;


            return View(model);


        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, ColumsModel model, int durum)
        {

            if (ModelState.IsValid)
            {
                using (var trans = await context.Database.BeginTransactionAsync())
                {
                    var KoseYazisi = await context.YynKoseYazilaris.FirstOrDefaultAsync(i => i.Id == id);
                    if (KoseYazisi == null)
                        return RedirectToAction("Index");

                    KoseYazisi.Baslik = model.Header;
                    KoseYazisi.Tarih = model.Date;
                    KoseYazisi.YazarId = model.PeopleId;
                    KoseYazisi.SirketId = model.CompanyId;
                    KoseYazisi.TurId = model.TypeId;
                    KoseYazisi.DilId = model.DilId;
                    KoseYazisi.Url = model.OrjLink;
                    KoseYazisi.OkunmaSayisi = model.ReadCount;
                    KoseYazisi.Metin = model.Context;
                    KoseYazisi.Keywords = model.Keywords;
                    KoseYazisi.BaslikEn = model.HeaderEn;
                    KoseYazisi.MetinEn = model.ContextEn;


                    context.YynKoseYazilaris.Update(KoseYazisi);
                    await context.SaveChangesAsync();

                    var koseyazilarikategoriler = context.YynKoseYazilariKategorileris.Where(a => a.KoseYazisiId == KoseYazisi.Id).ToList();
                    for (int i = 0; i < koseyazilarikategoriler.Count; i++)
                    {
                        var kayit = context.YynKoseYazilariKategorileris.FirstOrDefault(b => b.Id == koseyazilarikategoriler[i].Id);
                        context.YynKoseYazilariKategorileris.Remove(kayit);
                    }

                    for (int i = 0; i < model.CategoryIds.Length; i++)
                    {
                        context.YynKoseYazilariKategorileris.Add(new YynKoseYazilariKategorileri { KoseYazisiId = KoseYazisi.Id, KategoriId = model.CategoryIds[i] });
                    }

                    await context.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                if (durum == 3)
                {
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                else if (durum == 2)
                {

                    return RedirectToAction("Index");
                }

            }

            BaseModel? prev = await context.People.Where(i => i.Id < id).OrderByDescending(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.FirstName + " " + i.LastName }).FirstOrDefaultAsync();
            BaseModel? next = await context.People.Where(i => i.Id > id).OrderBy(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.FirstName + " " + i.LastName }).FirstOrDefaultAsync();
            ViewBag.Prev = prev;
            ViewBag.Next = next;

            ViewBag.KoseYazilariKategori = await (from a in context.YynKoseYazilariKategorileris
                                                  where a.KoseYazisiId == id
                                                  select new KoseYazilariKategoriModel
                                                  {
                                                      Id = a.Id,
                                                      KoseYazisiId = a.KoseYazisiId,
                                                      KategoriId = a.KategoriId,
                                                  }).ToListAsync();



            var yazarlar = await (from a in context.People
                                  select new PeopleListModel
                                  {
                                      LastName = a.LastName,
                                      FirstName = a.FirstName,
                                      Id = a.Id,
                                  }
     ).OrderByField("FirstName", Ascending: true).ToListAsync();

            var sirketler = await context.Companies.ToListAsync();
            var kategoriler = await context.YynKategorilers.ToListAsync();
            var diller = await context.YynDillers.ToListAsync();
            var turler = await context.YynTurlers.ToListAsync();

            ViewBag.Turler = turler;
            ViewBag.Yazarlar = yazarlar;
            ViewBag.Kategoriler = kategoriler;
            ViewBag.Diller = diller;
            ViewBag.Sirketler = sirketler;


            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteColums(int id)
        {
            try
            {
                using (var trans = await context.Database.BeginTransactionAsync())
                {
                    var koseyazisi = await context.YynKoseYazilaris.FirstOrDefaultAsync(i => i.Id == id);
                    if (koseyazisi == null)
                        return RedirectToAction("Index");




                    var koseyazikategoris = context.YynKoseYazilariKategorileris.Where(i => i.KoseYazisiId == id);
                    foreach (var item in koseyazikategoris)
                    {

                        context.YynKoseYazilariKategorileris.Remove(item);


                    }



                    context.YynKoseYazilaris.Remove(koseyazisi);
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
