using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Siyasett.Core.Abstracts;
using Siyasett.Core.Chronology;
using Siyasett.Core.Colums;
using Siyasett.Core.Extensions;
using Siyasett.Core.People;
using Siyasett.Core.PoliticalLibrary;
using Siyasett.Core.VideoLibrary;
using Siyasett.Data;
using Siyasett.Data.Data;
using Siyasett.Models;
using Siyasett.Web.Languages;
using Siyasett.Web.Models;
using System.Drawing.Printing;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Siyasett.Web.Controllers
{
    public class ActualsController : Controller
    {
        protected readonly AppDbContext context;

        public ActualsController(AppDbContext context)
        {
            this.context = context;
        }

        [Route("Gundem")]
        [Route("Actuals")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("kose-yazilari")]
        [Route("columns")]
        public async Task<IActionResult> Columns(int page = 1, int pagesize = 42, string query = "", int personId = 0, int companyId = 0, int date = 0, string contexttext = "", int categoryId = 0, int dilId = 0)
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;
            string[] orderFields = new string[] { "Date", "Header", "YazarAdSoyad", "TurAdi", "SirketAdi", "DilId" };
            pager.SetQuery(Request.Query);
            ColumsSearchModel searchModel = new ColumsSearchModel();
            searchModel.YazarId = personId;
            searchModel.SirketId = companyId;
            searchModel.ContextText = contexttext;
            searchModel.Kategori = categoryId;
            searchModel.DilId = dilId;
            //if (string.IsNullOrEmpty(date)) { date = DateTime.UtcNow.ToLocalTime().ToString(); };
            searchModel.DateId = date;

            DateTime startDate, endDate;

            switch (date)
            {
                case 1:
                    startDate = DateTime.Today;
                    endDate = DateTime.Today.AddDays(1).AddTicks(-1);
                    break;

                case 2:
                    startDate = DateTime.Today.AddDays(-2);
                    endDate = DateTime.Today.AddDays(1).AddTicks(-1);
                    break;

                case 3:
                    startDate = DateTime.Today.AddDays(-6);
                    endDate = DateTime.Today.AddDays(1).AddTicks(-1);
                    break;

                case 4:
                    startDate = DateTime.Today.AddMonths(-1);
                    endDate = DateTime.Today.AddDays(1).AddTicks(-1);
                    break;
                default:
                    startDate = DateTime.MinValue;
                    endDate = DateTime.MaxValue;
                    break;
            }


            var turler = await context.YynTurlers.ToListAsync();


            var koseYazilari = await (from a in context.YynKoseYazilaris
                                      let koseyazikategoriler = context.YynKoseYazilariKategorileris.Where(k => k.KoseYazisiId == a.Id).Select(l => l.KategoriId).Distinct().ToList()
                                      where (searchModel.DateId == 0 || (a.Tarih >= startDate && a.Tarih <= endDate))
                                      && (string.IsNullOrEmpty(contexttext) || (a.Metin != null && a.Metin.ToLower().Contains(contexttext.ToLower())) || (a.MetinEn != null && a.MetinEn.ToLower().Contains(contexttext.ToLower())) || (a.Baslik != null && a.Baslik.ToLower().Contains(contexttext.ToLower())) || (a.BaslikEn != null && a.BaslikEn.ToLower().Contains(contexttext.ToLower())))
                                && (searchModel.YazarId == 0 || (searchModel.YazarId == a.YazarId))
                                && (searchModel.SirketId == 0 || (searchModel.SirketId == a.SirketId))
                                 && (searchModel.Kategori == 0 || (koseyazikategoriler.Contains(searchModel.Kategori)))
                                && (searchModel.DilId == 0 || (searchModel.DilId == a.DilId))
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
                                          SirketAdi = a.Sirket.Name,
                                          TurAdi = a.Tur.Adi,
                                          YazarAdSoyad = a.Yazar.FirstName + ' ' + a.Yazar.LastName,
                                          AuthorPhoto = a.Yazar.Photo,
                                          ContextEn = a.MetinEn,
                                          HeaderEn = a.BaslikEn,

                                      }).OrderByDescending(b => b.Date).ThenBy(c => c.SirketAdi).Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).AsNoTracking().ToListAsync();

            var varolankoseyazarlari = context.YynKoseYazilaris.Select(a => a.YazarId).Distinct().ToList();
            var varolansirketler = context.YynKoseYazilaris.Select(a => a.SirketId).Distinct().ToList();
            var varolandiller = context.YynKoseYazilaris.Select(a => a.DilId).Distinct().ToList();
            var varolankategoriler = context.YynKoseYazilariKategorileris.Select(a => a.KategoriId).Distinct().ToList();

            pager.Total = await (from a in context.YynKoseYazilaris
                                 let koseyazikategoriler = context.YynKoseYazilariKategorileris.Where(k => k.KoseYazisiId == a.Id).Select(l => l.KategoriId).Distinct().ToList()
                                 where (searchModel.DateId == 0 || (a.Tarih >= startDate && a.Tarih <= endDate))
                                 && (string.IsNullOrEmpty(contexttext) || a.Metin.ToLower().Contains(contexttext.ToLower()))
                           && (searchModel.YazarId == 0 || (searchModel.YazarId == a.YazarId))
                           && (searchModel.SirketId == 0 || (searchModel.SirketId == a.SirketId))
                            && (searchModel.Kategori == 0 || (koseyazikategoriler.Contains(searchModel.Kategori)))
                           && (searchModel.DilId == 0 || (searchModel.DilId == a.DilId))
                                 select a).AsNoTracking().CountAsync();


            var yazarlar = await (from a in context.People
                                  where a.PeopleJobs.FirstOrDefault(b => b.JobId == 25 && b.PeopleId == a.Id) != null && varolankoseyazarlari.Contains(a.Id)
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

        [Route("kose-yazilari-detay/{id:int}")]
        [Route("Actuals/columns-detail/{id:int}")]
        public async Task<IActionResult> ColumnsDetail(int id)
        {



            var koseYazilari = await context.YynKoseYazilaris.FirstOrDefaultAsync(a => a.Id == id);
            koseYazilari.OkunmaSayisi++;
            await context.SaveChangesAsync();
            ViewBag.Sirket = await context.Companies.FirstOrDefaultAsync(i => i.Id == koseYazilari.SirketId);

            ViewBag.Yazar = await context.People.FirstOrDefaultAsync(a => a.Id == koseYazilari.YazarId);



            var koseYazilarioncesonra = await (from a in context.YynKoseYazilaris
                                               let koseyazikategoriler = context.YynKoseYazilariKategorileris.Where(k => k.KoseYazisiId == a.Id).Select(l => l.KategoriId).Distinct().ToList()
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
                                                   SirketAdi = a.Sirket.Name,
                                                   TurAdi = a.Tur.Adi,
                                                   YazarAdSoyad = a.Yazar.FirstName + ' ' + a.Yazar.LastName,
                                                   AuthorPhoto = a.Yazar.Photo,
                                                   ContextEn = a.MetinEn,


                                               }).OrderByDescending(b => b.Date).ThenBy(c => c.SirketAdi).AsNoTracking().ToListAsync();

            var index = koseYazilarioncesonra.FindIndex(a => a.Id == id);
            ViewBag.Onceki = (index - 1) == -1 ? 0 : koseYazilarioncesonra[index - 1].Id;
            ViewBag.Sonraki = (index + 1) > koseYazilarioncesonra.Count - 1 ? 0 : koseYazilarioncesonra[index + 1].Id;

            var yazarindigeryazilari = koseYazilarioncesonra.Where(a => a.PeopleId == koseYazilari.YazarId && a.Id != id).OrderByDescending(b => b.Date).ToList();

            ViewBag.DigerYazilari = yazarindigeryazilari;


            ViewBag.Kategoriler = string.Join(", ", await (from a in context.YynKoseYazilariKategorileris
                                                           join b in context.YynKategorilers on a.KategoriId equals b.Id
                                                           where a.KoseYazisiId == id
                                                           select b.Adi).ToArrayAsync());

            return View(koseYazilari);

        }



        [Route("sosyal-medyadan")]
        [Route("from-socials")]
        public IActionResult FromSocials()
        {
            return View();
        }

        [Route("videolar")]
        [Route("videos")]
        public async Task<IActionResult> Videos(int page = 1, int pagesize = 20, string query = "", int session = 0, int lang = 0, int sekme = 2)
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;



            var liste = await (from a in context.VideoLibraries
                               where (string.IsNullOrEmpty(query) || (!string.IsNullOrEmpty(query) && a.Head.ToUpper().Contains(query.ToUpper())))
                                 && ((session != 0 && a.Session == session) || session == 0)
                                 && ((lang != 0 && a.Language == lang) || lang == 0)
                               select new VideoLibraryModel
                               {
                                   Id = a.Id,
                                   Header = a.Head,
                                   Language = a.Language,
                                   VideoLink = a.VideoLink,
                                   Session = a.Session
                               }
            ).OrderByDescending(j => j.Session).ThenBy(k => k.Header).AsNoTracking().Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(20).ToListAsync();

            pager.Total = await (from a in context.VideoLibraries
                                 where (string.IsNullOrEmpty(query) || (!string.IsNullOrEmpty(query) && a.Head.ToUpper().Contains(query.ToUpper())) || (!string.IsNullOrEmpty(query) && a.VideoLink.ToUpper().Contains(query.ToUpper())))
                                 && ((session != 0 && a.Session == session) || session == 0)
                                 && ((lang != 0 && a.Language == lang) || lang == 0)
                                 select new VideoLibraryModel
                                 {
                                     Id = a.Id,

                                 }
).AsNoTracking().CountAsync();


            var uniqedil = context.VideoLibraries.Select(a => a.Language).Distinct();

            ViewBag.Session = await context.VideoSessions.AsNoTracking().ToListAsync();
            ViewBag.Language = await context.GeneralLanguages.Where(a => uniqedil.Contains(a.Id)).AsNoTracking().ToListAsync();
            ViewBag.Sess = session;
            ViewBag.Lang = lang;
            ViewBag.Sekme = sekme;

            ViewBag.Query = query;
            ViewBag.Pager = pager;
            return View(liste);
        }


        [Route("mevzuat")]
        [Route("regulations")]
        public IActionResult Regulations()
        {
            return View();
        }



        [Route("kronoloji")]
        [Route("Chronology")]
        public async Task<IActionResult> Chronology(int page = 1, int pagesize = 30, int time = 0, string query = "", byte sort = 0, byte desc = 0)
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;

            int day = 999999;
            ViewBag.time = time;
            if (time == 1)
            {
                day = 8;
            }
            else if (time == 2)
            {
                day = 32;
            }
            else if (time == 3)
            {
                day = 366;
            }

            var liste = await (from a in context.Chronologies
                               where (string.IsNullOrEmpty(query) || (!string.IsNullOrEmpty(query) && (a.DescriptionTr).ToUpper().Contains(query.ToUpper())) || (!string.IsNullOrEmpty(query) && context.ChronologiesParties.Where(b => b.Chronologyid == a.Id).Select(d => context.Parties.FirstOrDefault(e => e.Id == d.Partyid).ShortName).Contains(query.ToUpper())) || (!string.IsNullOrEmpty(query) && (a.EventDate.ToDateTime(TimeOnly.Parse("10:00 PM"))).ToString().Contains(query))) && (DateTime.Today - a.EventDate.ToDateTime(TimeOnly.Parse("10:00 PM"))).Days < day
                               orderby a.EventDate descending
                               select new ChronoModel
                               {
                                   Id = a.Id,
                                   EventDate = a.EventDate,
                                   DescriptionTr = a.DescriptionTr,
                                   DescriptionEn = a.DescriptionEn,
                                   PartyNames = context.ChronologiesParties.Where(b => b.Chronologyid == a.Id).Select(d => context.Parties.FirstOrDefault(e => e.Id == d.Partyid).ShortName).ToArray()
                               }
            ).AsNoTracking().Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).ToListAsync();

            pager.Total = await (from a in context.Chronologies
                                 where (string.IsNullOrEmpty(query) || (!string.IsNullOrEmpty(query) && (a.DescriptionTr).ToUpper().Contains(query.ToUpper())) || (!string.IsNullOrEmpty(query) && context.ChronologiesParties.Where(b => b.Chronologyid == a.Id).Select(d => context.Parties.FirstOrDefault(e => e.Id == d.Partyid).ShortName).Contains(query.ToUpper())) || (!string.IsNullOrEmpty(query) && (a.EventDate.ToDateTime(TimeOnly.Parse("10:00 PM"))).ToString().Contains(query))) && (DateTime.Today - a.EventDate.ToDateTime(TimeOnly.Parse("10:00 PM"))).Days < day
                                 orderby a.EventDate descending
                                 select new ChronoModel
                                 {
                                     Id = a.Id,
                                     EventDate = a.EventDate,
                                     DescriptionTr = a.DescriptionTr,
                                     DescriptionEn = a.DescriptionEn,
                                     PartyNames = context.ChronologiesParties.Where(b => b.Chronologyid == a.Id).Select(d => context.Parties.FirstOrDefault(e => e.Id == d.Partyid).ShortName).ToArray()
                                 }).AsNoTracking().CountAsync();

            ViewBag.Query = query;
            ViewBag.Pager = pager;
            return View(liste);
        }

        [Route("politika")]
        [Route("politics")]
        public IActionResult Politics()
        {
            return View();
        }

        [Route("siyasettkutuphanesi")]
        [Route("politicallibrary")]
        public async Task<IActionResult> PoliticalLibrary(int page = 1, int pagesize = 42, string query = "", int yayinevi = 0, int kategori = 0, int dil = 0, int donem = 0, int siralama = 0, int yazar = 0)
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;

            var asc = true;

            if (siralama == 4)
                asc = false;



            string[] orderFields = new string[] { "Adi", "YazarAdi", "YayinEviAdi", "Yili", "Yili" };
            pager.SetQuery(Request.Query);

            var yazarlar = await (from a in context.People
                                  where a.PositionField.Id == 3
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
            var kitaplar = await (from a in context.YynKitaplars
                                  where (yayinevi == 0 || (yayinevi != 0 && a.YayinEviId == yayinevi)) &&
                                  (kategori == 0 || (kategori != 0 && a.YynKitapKategorilers.FirstOrDefault(b => b.KitapId == a.Id && b.KategoriId == kategori) != null)) &&
                                  (dil == 0 || (dil != 0 && a.DilId == dil)) &&
                                  (yazar == 0 || (yazar != 0 && context.YynKitapYazars.Where(p => p.KitapId == a.Id).Where(y => y.YazarId == yazar).Count() > 0)) &&
                                  (donem == 0 || (donem == 1 && a.Yili >= 2021 && a.Yili <= DateTime.Now.Year) || (donem == 2 && a.Yili >= 2011 && a.Yili <= 2020) || (donem == 2 && a.Yili >= 2001 && a.Yili <= 2010) || (donem == 3 && a.Yili < 2001)) &&
                                 (string.IsNullOrEmpty(query) || a.Adi.ToLower().Contains(query.ToLower()))
                                  select new KitapModel { Aciklama = a.Aciklama, Adi = a.Adi, DilId = a.DilId, EkitapUrl = a.EKitapUrl, Id = a.Id, Image = a.Image, YayinEviId = a.YayinEviId, Yili = a.Yili, KategoriId = context.YynKitapKategorilers.Where(b => b.KitapId == a.Id).Select(c => c.KategoriId).ToArray(), Yazari = context.YynKitapYazars.Where(b => b.KitapId == a.Id).Select(c => c.YazarId).ToArray(), YazarAdi = context.People.FirstOrDefault(z => z.Id == context.YynKitapYazars.FirstOrDefault(b => b.KitapId == a.Id).YazarId).FirstName, YayinEviAdi = context.Companies.FirstOrDefault(k => k.Id == a.YayinEviId).Name }).OrderByField(orderFields[siralama], asc).Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(42).AsNoTracking().ToListAsync();


            pager.Total = await (from a in context.YynKitaplars
                                 where (yayinevi == 0 || (yayinevi != 0 && a.YayinEviId == yayinevi)) &&
                                   (kategori == 0 || (kategori != 0 && a.YynKitapKategorilers.FirstOrDefault(b => b.KitapId == a.Id && b.KategoriId == kategori) != null)) &&
                                   (dil == 0 || (dil != 0 && a.DilId == dil)) &&
                                   (yazar == 0 || (yazar != 0 && context.YynKitapYazars.Where(p => p.KitapId == a.Id).Where(y => y.YazarId == yazar).Count() > 0)) &&
                                   (donem == 0 || (donem == 1 && a.Yili >= 2021 && a.Yili <= DateTime.Now.Year) || (donem == 2 && a.Yili >= 2011 && a.Yili <= 2020) || (donem == 2 && a.Yili >= 2001 && a.Yili <= 2010) || (donem == 3 && a.Yili < 2001)) &&
                                  (string.IsNullOrEmpty(query) || a.Adi.ToLower().Contains(query.ToLower()))
                                 select a).AsNoTracking().CountAsync();

            ViewBag.Pager = pager;
            ViewBag.Yazarlar = yazarlar;
            ViewBag.Kategoriler = kategoriler;
            ViewBag.Diller = diller;
            ViewBag.Sirketler = sirketler;


            ViewBag.yayin = yayinevi;
            ViewBag.kategor = kategori;
            ViewBag.dil = dil;
            ViewBag.donem = donem;
            ViewBag.siralama = siralama;
            ViewBag.yazar = yazar;



            ViewBag.Query = query;

            return View(kitaplar);
        }


    }
}
