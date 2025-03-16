using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Siyasett.Core.Abstracts;
using Siyasett.Core.People;
using Siyasett.Data;
using Siyasett.Data.Data;
using Siyasett.Models;
using Siyasett.Web.Models;
using Siyasett.Core.Emails;
using Siyasett.Core.SocialMedia;

using Siyasett.Core.Party;
using Siyasett.Core;
using Siyasett.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Siyasett.Core.PoliticalLibrary;
using Siyasett.Core.Addresses;
using Siyasett.Core.Phones;
using System.Linq;

namespace Siyasett.Web.Areas.Admin.Controllers
{
    public class PoliticalLibraryManagementController : AdminController
    {
        private readonly IWebHostEnvironment _host;
        public PoliticalLibraryManagementController(ILogger<AdminController> logger, IWebHostEnvironment env, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {
            _host = env;
        }


        public async Task<IActionResult> Index(int page = 1, byte sort = 1, byte desc = 0, int pagesize = 25, string query = "")
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;
            string[] orderFields = new string[] { "KategoriAdi", "Adi", "YazarAdi", "YayinEviAdi", "DilAdi" };

            PoliticalLibrarySearchModel searchModel = new PoliticalLibrarySearchModel();
            searchModel.OrderBy = sort;
            searchModel.OrderByDesc = desc;
            

            var yazarlar = await(from a in context.People
                              select new PeopleListModel
                              {
                                  LastName = a.LastName,
                                  FirstName = a.FirstName,
                                  Id = a.Id,
                              }
            ).OrderByField("FirstName", Ascending:true).ToListAsync();

            var sirketler= await context.Companies.Where(i => i.CompanySectors.Any(a => a.SectorId == 37)).OrderBy(i => i.Name).AsNoTracking().ToListAsync();
            var kategoriler=await context.YynKategorilers.ToListAsync();
            var diller=await context.YynDillers.ToListAsync();
            var kitaplar= await (from a in context.YynKitaplars
                                 where string.IsNullOrEmpty(query) || (!string.IsNullOrEmpty(query) && a.Adi.ToLower().Contains(query.ToLower())) 
                                 select new KitapModel { 
                                     Aciklama = a.Aciklama,
                                     Adi = a.Adi,
                                     DilId = a.DilId,
                                     EkitapUrl = a.EKitapUrl,
                                     Id = a.Id,
                                     Image = a.Image,
                                     YayinEviId = a.YayinEviId,
                                     Yili = a.Yili,
                                     KategoriId=context.YynKitapKategorilers.Where(b=>b.KitapId==a.Id).Select(c=>c.KategoriId).ToArray(),
                                     Yazari = context.YynKitapYazars.Where(b => b.KitapId == a.Id).Select(c => c.YazarId).ToArray(),
                                     YayinEviAdi=context.Companies.FirstOrDefault(d=>d.Id==a.YayinEviId).Name,
                                     YazarAdi = context.People.FirstOrDefault(z => z.Id == context.YynKitapYazars.FirstOrDefault(b => b.KitapId == a.Id).YazarId).FirstName,
                                     KategoriAdi= context.YynKitapKategorilers.FirstOrDefault(e => e.KitapId == a.Id).Kategori.Adi,
                                     DilAdi=context.YynDillers.FirstOrDefault(f=>f.Id==a.DilId).Adi
                                 }).OrderByField(orderFields[searchModel.OrderBy], searchModel.OrderByDesc == 0).Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).AsNoTracking().ToListAsync();
            


            pager.Total = await (from a in context.YynKitaplars
                                 where string.IsNullOrEmpty(query) || (!string.IsNullOrEmpty(query) && a.Adi.ToLower().Contains(query.ToLower()))
                                 select new KitapModel
                                 {
                                     Aciklama = a.Aciklama,
                                     Adi = a.Adi,
                                     DilId = a.DilId,
                                     EkitapUrl = a.EKitapUrl,
                                     Id = a.Id,
                                     Image = a.Image,
                                     YayinEviId = a.YayinEviId,
                                     Yili = a.Yili,
                                     KategoriId = context.YynKitapKategorilers.Where(b => b.KitapId == a.Id).Select(c => c.KategoriId).ToArray(),
                                     Yazari = context.YynKitapYazars.Where(b => b.KitapId == a.Id).Select(c => c.YazarId).ToArray(),
                                     YayinEviAdi = context.Companies.FirstOrDefault(d => d.Id == a.YayinEviId).Name,
                                     YazarAdi = context.People.FirstOrDefault(z => z.Id == context.YynKitapYazars.FirstOrDefault(b => b.KitapId == a.Id).YazarId).FirstName,
                                     KategoriAdi = context.YynKitapKategorilers.FirstOrDefault(e => e.KitapId == a.Id).Kategori.Adi,
                                     DilAdi = context.YynDillers.FirstOrDefault(f => f.Id == a.DilId).Adi
                                 }).AsNoTracking().CountAsync();

            ViewBag.Pager = pager;


            ViewBag.Yazarlar=yazarlar;
            ViewBag.Kategoriler=kategoriler;
            ViewBag.Diller=diller;
            ViewBag.Sirketler = sirketler;
            ViewBag.SearchModel = searchModel;
            ViewBag.Query=query;

            return View(kitaplar);
        }


        public async Task<IActionResult> New()
        {
            KitapModel model = new KitapModel();

            var yazarlar = await (from a in context.People
                                  select new PeopleListModel
                                  {
                                      LastName = a.LastName,
                                      FirstName = a.FirstName,
                                      Id = a.Id,
                                  }
           ).OrderByField("FirstName", Ascending: true).ToListAsync();

            var sirketler = await context.Companies.Where(i => i.CompanySectors.Any(a => a.SectorId == 37)).OrderBy(i => i.Name).AsNoTracking().ToListAsync();
            var kategoriler = await context.YynKategorilers.ToListAsync();
            var diller = await context.YynDillers.ToListAsync();


            ViewBag.Yazarlar = yazarlar;
            ViewBag.Kategoriler = kategoriler;
            ViewBag.Diller = diller;
            ViewBag.Sirketler = sirketler;


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> New(KitapModel model, int durum)
        {
            var asd = model;
            if (ModelState.IsValid)
            {
                var Kitap = new YynKitaplar();
                Kitap.Adi = model.Adi;
                Kitap.YayinEviId = model.YayinEviId;
                Kitap.DilId = model.DilId;
                Kitap.Yili = model.Yili;
                Kitap.EKitapUrl = model.EkitapUrl;
                Kitap.Aciklama = model.Aciklama;

                
                context.YynKitaplars.Add(Kitap);
                await context.SaveChangesAsync();
                foreach (var item in model.KategoriId)
                {

                    context.YynKitapKategorilers.Add(new YynKitapKategoriler { KitapId = Kitap.Id, KategoriId = item });
                }
                foreach (var item in model.Yazari)
                {

                    context.YynKitapYazars.Add(new YynKitapYazar { KitapId = Kitap.Id, YazarId = item });
                }
                
                if (!string.IsNullOrEmpty(model.Image))
                {

                    byte[] bytes = Convert.FromBase64String(model.Image.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[1]);
                    var extension = model.Image.Contains("png") ? ".png" : ".jpg";

                    string path = _host.WebRootPath;

                    string filename = Guid.NewGuid().ToString("N") + extension;
                    if (!Directory.Exists(Path.Combine(path, "images", "books")))
                        Directory.CreateDirectory(Path.Combine(path, "images", "books"));

                    string savefile = Path.Combine(path, "images", "books", filename);
                    using (var imageFile = new FileStream(savefile, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();


                    }
                    Kitap.Image = filename;
                }

                await context.SaveChangesAsync();
                if (durum == 1)
                {
                    return RedirectToAction("Edit", new { id = Kitap.Id });
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

            var sirketler = await context.Companies.Where(i => i.CompanySectors.Any(a => a.SectorId == 37)).OrderBy(i => i.Name).AsNoTracking().ToListAsync();
            var kategoriler = await context.YynKategorilers.ToListAsync();
            var diller = await context.YynDillers.ToListAsync();


            ViewBag.Yazarlar = yazarlar;
            ViewBag.Kategoriler = kategoriler;
            ViewBag.Diller = diller;
            ViewBag.Sirketler = sirketler;

            return View(model);

        }

        public async Task<IActionResult> Edit(int id)
        {

            KitapModel? model = await (from a in context.YynKitaplars
                                              where a.Id == id
                                              select new KitapModel
                                              {
                                                  Id = a.Id,
                                                  Adi=a.Adi,
                                                  Aciklama=a.Aciklama,
                                                  DilId=a.DilId,
                                                  EkitapUrl= a.EKitapUrl,
                                                  Image= a.Image,
                                                  Yili=a.Yili,
                                                  YayinEviId=a.YayinEviId,

                                              }
                                       ).FirstOrDefaultAsync();

            BaseModel? prev = await context.YynKitaplars.Where(i => i.Id < id).OrderByDescending(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.Adi }).FirstOrDefaultAsync();
            BaseModel? next = await context.YynKitaplars.Where(i => i.Id > id).OrderBy(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.Adi }).FirstOrDefaultAsync();
            ViewBag.Prev = prev;
            ViewBag.Next = next;

            ViewBag.kitapyazarlari = await (from a in context.YynKitapYazars
                                             where a.KitapId == id
                                             select new KitapYazarlarModel
                                             {
                                                 Id = a.Id,
                                                 KitapId= a.KitapId,
                                                 YazarId= a.YazarId,
                                             }).ToListAsync();

            ViewBag.kitapkategori = await (from a in context.YynKitapKategorilers
                                    where a.KitapId == id
                                    select new KitapKategorilerModel
                                    {
                                        Id= a.Id,
                                        KategoriId= a.KategoriId,
                                        KitapId=a.KitapId,

                                    }).AsNoTracking().ToListAsync();


            var yazarlar = await (from a in context.People
                                  select new PeopleListModel
                                  {
                                      LastName = a.LastName,
                                      FirstName = a.FirstName,
                                      Id = a.Id,
                                  }
           ).OrderByField("FirstName", Ascending: true).ToListAsync();

            var sirketler = await context.Companies.Where(i => i.CompanySectors.Any(a => a.SectorId == 37)).OrderBy(i => i.Name).AsNoTracking().ToListAsync();
            var kategoriler = await context.YynKategorilers.ToListAsync();
            var diller = await context.YynDillers.ToListAsync();


            ViewBag.Yazarlar = yazarlar;
            ViewBag.Kategoriler = kategoriler;
            ViewBag.Diller = diller;
            ViewBag.Sirketler = sirketler;



            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, KitapModel model, int durum)
        {

            if (ModelState.IsValid)
            {
                using (var trans = await context.Database.BeginTransactionAsync())
                {
                    var Kitap = await context.YynKitaplars.FirstOrDefaultAsync(i => i.Id == id);
                    if (Kitap == null)
                        return RedirectToAction("Index");

                    Kitap.Adi = model.Adi;
                    Kitap.YayinEviId = model.YayinEviId;
                    Kitap.Yili = model.Yili;
                    Kitap.DilId = model.DilId;
                    Kitap.EKitapUrl = model.EkitapUrl;
                    Kitap.Aciklama = model.Aciklama;

                    
                    var yazaroncekikayitlar= context.YynKitapYazars.Where(i => i.KitapId == id);
                    foreach (var item in yazaroncekikayitlar)
                    {

                            context.YynKitapYazars.Remove(item);

                        
                    }

                    var kategorioncekikayitlar = context.YynKitapKategorilers.Where(i => i.KitapId == id);
                    foreach (var item in kategorioncekikayitlar)
                    {

                            context.YynKitapKategorilers.Remove(item);

                    }

                    foreach (var item in model.KategoriId)
                    {

                        context.YynKitapKategorilers.Add(new YynKitapKategoriler { KitapId = Kitap.Id, KategoriId = item });
                    }
                    foreach (var item in model.Yazari)
                    {

                        context.YynKitapYazars.Add(new YynKitapYazar { KitapId = Kitap.Id, YazarId = item });
                    }


                    if (!string.IsNullOrEmpty(model.Image))
                    {

                        byte[] bytes = Convert.FromBase64String(model.Image.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[1]);
                        var extension = model.Image.Contains("png") ? ".png" : ".jpg";

                        string path = _host.WebRootPath;

                        string filename = Guid.NewGuid().ToString("N") + extension;
                        if (!Directory.Exists(Path.Combine(path, "images", "books")))
                            Directory.CreateDirectory(Path.Combine(path, "images", "books"));

                        string savefile = Path.Combine(path, "images", "books", filename);
                        using (var imageFile = new FileStream(savefile, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();


                        }
                        Kitap.Image = filename;
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

            ViewBag.kitapyazarlari = await (from a in context.YynKitapYazars
                                            where a.KitapId == id
                                            select new KitapYazarlarModel
                                            {
                                                Id = a.Id,
                                                KitapId = a.KitapId,
                                                YazarId = a.YazarId,
                                            }).ToListAsync();

            ViewBag.kitapkategori = await (from a in context.YynKitapKategorilers
                                           where a.KitapId == id
                                           select new KitapKategorilerModel
                                           {
                                               Id = a.Id,
                                               KategoriId = a.KategoriId,
                                               KitapId = a.KitapId,

                                           }).AsNoTracking().ToListAsync();


            var yazarlar = await (from a in context.People
                                  select new PeopleListModel
                                  {
                                      LastName = a.LastName,
                                      FirstName = a.FirstName,
                                      Id = a.Id,
                                  }
           ).OrderByField("FirstName", Ascending: true).ToListAsync();

            var sirketler = await context.Companies.Where(i => i.CompanySectors.Any(a => a.SectorId == 37)).OrderBy(i => i.Name).AsNoTracking().ToListAsync();
            var kategoriler = await context.YynKategorilers.ToListAsync();
            var diller = await context.YynDillers.ToListAsync();


            ViewBag.Yazarlar = yazarlar;
            ViewBag.Kategoriler = kategoriler;
            ViewBag.Diller = diller;
            ViewBag.Sirketler = sirketler;





            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                using (var trans = await context.Database.BeginTransactionAsync())
                {
                    var Kitap = await context.YynKitaplars.FirstOrDefaultAsync(i => i.Id == id);
                    if (Kitap == null)
                        return RedirectToAction("Index");

                    

                    
                    var yazaroncekikayitlar= context.YynKitapYazars.Where(i => i.KitapId == id);
                    foreach (var item in yazaroncekikayitlar)
                    {

                            context.YynKitapYazars.Remove(item);

                        
                    }

                    var kategorioncekikayitlar = context.YynKitapKategorilers.Where(i => i.KitapId == id);
                    foreach (var item in kategorioncekikayitlar)
                    {

                            context.YynKitapKategorilers.Remove(item);

                    }

                    context.YynKitaplars.Remove(Kitap);
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
