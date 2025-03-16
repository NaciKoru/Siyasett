using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siyasett.Core;
using Siyasett.Core.Abstracts;
using Siyasett.Core.Party;
using Siyasett.Core.Poll;
using Siyasett.Data.Data;
using Siyasett.Web.Models;
using System;
using System.Globalization;

namespace Siyasett.Web.Controllers
{
    public class ElectionsController : Controller
    {
        protected readonly AppDbContext context;
        private readonly ILogger<PeopleController> _logger;

        public ElectionsController(AppDbContext context, ILogger<PeopleController> logger)
        {
            this.context = context;
            _logger = logger;
        }


        [Route("secimler")]
        [Route("elections")]
        public async Task<IActionResult> Index(int? secim)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;



            ViewBag.Iller = await context.SecimIlListesis.OrderBy(i => i.Adi).Select(i => new BaseModel { Id = i.Id, Name = i.Adi }).ToListAsync();

            var secimler = await (from a in context.Secims
                                  join b in context.SecimDetays on a.Id equals b.SecimId
                                  where b.SecimTuruId == 8
                                  orderby a.SecimTarihi descending
                                  select a).ToListAsync();


            ElectionSearchModel searchModel = new ElectionSearchModel();
            searchModel.SecimId = secim ?? secimler.FirstOrDefault().Id;


            var secimDetay = await context.SecimDetays.FirstOrDefaultAsync(a => a.SecimId == searchModel.SecimId && a.SecimTuruId == 8);

            var partiler = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  where a.SecimDetayId == secimDetay.Id
                                  group a by new { a.PartiId, b.ShortName, b.Name } into grp

                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = (int)grp.Key.PartiId,
                                      PartyNameShort = grp.Key.ShortName,

                                  }).OrderBy(i => i.Name).ToListAsync();

            var secimIller = await (from a in context.SecimIllers
                                    where a.SecimDetayId == secimDetay.Id
                                    select new ElectionProvinceModel
                                    {
                                        Id = a.IlId.Value,
                                        Name = a.Adi,
                                        RegionId = a.YskSecimCevresiId

                                    }).OrderBy(i => i.Name).ToListAsync();




            var secimSonuclar = await (from a in context.Sonuclars
                                       where a.SecimDetayId == secimDetay.Id
                                       group a by new { a.IlId, a.YskSecimCevresiId, a.PartiId } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           IlId = grp.Key.IlId,
                                           PartyId = grp.Key.PartiId,
                                           Vote = grp.Sum(i => i.OySayisi),
                                           YskSecimCevresiId = grp.Key.YskSecimCevresiId

                                       }).ToListAsync();

            foreach (var item in partiler)
            {
                item.RatioAvg = secimSonuclar.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }

            ViewBag.SecimDetay = secimDetay;
            ViewBag.Parties = partiler.OrderByDescending(i => i.RatioAvg).ToList();
            ViewBag.SecimIller = secimIller;
            ViewBag.SearchModel = searchModel;
            ViewBag.SecimSonuclar = secimSonuclar;

            return View(secimler);
        }

        [Route("secim-il-detay", Name = "secim-il-detay")]
        [Route("election-province-detail", Name = "secim-il-detay2")]
        public async Task<IActionResult> ElectionDetail1(int secim, int id, int cevreid)
        {

            var gecerliSecim = await (from a in context.Secims
                                      join b in context.SecimDetays on a.Id equals b.SecimId
                                      where a.Id == secim
                                      orderby a.SecimTarihi descending
                                      select a).FirstOrDefaultAsync();

            var secimDetay = await context.SecimDetays.FirstOrDefaultAsync(a => a.SecimId == secim && a.SecimTuruId == 8);



            var partiler = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  where a.SecimDetayId == secimDetay.Id
                                  group a by new { a.PartiId, b.ShortName, b.Name } into grp

                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = (int)grp.Key.PartiId,
                                      PartyNameShort = grp.Key.ShortName,

                                  }).OrderBy(i => i.Name).ToListAsync();


            var secimIl = await (from a in context.SecimIllers
                                 where a.SecimDetayId == secimDetay.Id && a.IlId == id && a.YskSecimCevresiId == cevreid
                                 select new ElectionProvinceModel
                                 {
                                     Id = a.Id,

                                     Name = a.Adi,
                                     RegionId = a.YskSecimCevresiId

                                 }).FirstOrDefaultAsync();

            var secimIlDetay = await context.SecimMuhtarlikOzets.Where(i => i.SecimDetayId == secimDetay.Id && i.SecimIllerId == (int)secimIl.Id).ToListAsync();

            var secimIlceler = await (from a in context.SecimIlcelers
                                      where a.YskSecimCevresiId == cevreid && a.IlId == secimIl.Id
                                      select new ElectionProvinceModel
                                      {
                                          Id = a.Id,
                                          Name = a.Adi,
                                          RegionId = a.YskSecimCevresiId

                                      }).OrderBy(i => i.Name).ToListAsync();



            var secimSonuclar = await (from a in context.Sonuclars
                                       where a.SecimDetayId == secimDetay.Id && a.SecimIllerId == secimIl.Id && a.YskSecimCevresiId == secimIl.RegionId
                                       group a by new { a.SecimIlcelerId, a.YskSecimCevresiId, a.PartiId } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           IlceId = grp.Key.SecimIlcelerId,
                                           PartyId = grp.Key.PartiId,
                                           Vote = grp.Sum(i => i.OySayisi),
                                           YskSecimCevresiId = grp.Key.YskSecimCevresiId

                                       }).ToListAsync();

            foreach (var item in partiler)
            {
                item.RatioAvg = secimSonuclar.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }

            ViewBag.SecimDetay = secimDetay;
            ViewBag.SecimilDetay = secimIlDetay;
            ViewBag.Parties = partiler.OrderByDescending(i => i.RatioAvg).ToList();
            ViewBag.Il = secimIl;
            ViewBag.SecimIlceler = secimIlceler;
            ViewBag.SecimSonuclar = secimSonuclar;
            ViewBag.GecerliSecim = gecerliSecim;
            return View(secimIl);
        }


        [Route("secim-ilce-detay", Name = "secim-ilce-detay")]
        [Route("election-district-detail", Name = "secim-ilce-detay2")]
        public async Task<IActionResult> ElectionDetail2(int secim, int id, int ilId, int cevreid)
        {

            var gecerliSecim = await (from a in context.Secims
                                      join b in context.SecimDetays on a.Id equals b.SecimId
                                      where a.Id == secim
                                      orderby a.SecimTarihi descending
                                      select a).FirstOrDefaultAsync();

            var secimDetay = await context.SecimDetays.FirstOrDefaultAsync(a => a.SecimId == secim && a.SecimTuruId == 8);

            var partiler = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  where a.SecimDetayId == secimDetay.Id
                                  group a by new { a.PartiId, b.ShortName, b.Name } into grp

                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = (int)grp.Key.PartiId,
                                      PartyNameShort = grp.Key.ShortName,

                                  }).OrderBy(i => i.Name).ToListAsync();


            var secimIl = await (from a in context.SecimIllers
                                 where a.SecimDetayId == secimDetay.Id && a.Id == ilId && a.YskSecimCevresiId == cevreid
                                 select new ElectionProvinceModel
                                 {
                                     Id = a.Id,

                                     Name = a.Adi,
                                     RegionId = a.YskSecimCevresiId

                                 }).FirstOrDefaultAsync();

            var secimIlce = await (from a in context.SecimIlcelers
                                   where a.SecimDetayId == secimDetay.Id && a.IlId == ilId && a.YskSecimCevresiId == cevreid && a.Id == id
                                   select new ElectionProvinceModel
                                   {
                                       Id = a.Id,

                                       Name = a.Adi,
                                       RegionId = a.YskSecimCevresiId

                                   }).FirstOrDefaultAsync();

            var secimIlceDetay = await context.SecimMuhtarlikOzets.Where(i => i.SecimDetayId == secimDetay.Id && i.SecimIlcelerId == (int)secimIlce.Id).ToListAsync();

            var secimMahalleler = await (from a in context.SecimMuhtarliklars
                                         where a.SecimDetayId == secimDetay.Id && a.IlceId == id
                                         select new ElectionProvinceModel
                                         {
                                             Id = a.Id,
                                             Name = a.Adi,


                                         }).OrderBy(i => i.Name).ToListAsync();



            var secimSonuclar = await (from a in context.Sonuclars
                                       where a.SecimDetayId == secimDetay.Id && a.SecimIllerId == secimIl.Id && a.YskSecimCevresiId == secimIl.RegionId && a.SecimIlcelerId == id
                                       group a by new { a.SecimMuhtarliklarId, a.YskSecimCevresiId, a.PartiId } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           MuhtarlikId = grp.Key.SecimMuhtarliklarId,
                                           PartyId = grp.Key.PartiId,
                                           Vote = grp.Sum(i => i.OySayisi),
                                           YskSecimCevresiId = grp.Key.YskSecimCevresiId

                                       }).ToListAsync();

            foreach (var item in partiler)
            {
                item.RatioAvg = secimSonuclar.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }

            ViewBag.SecimDetay = secimDetay;
            ViewBag.SecimIlceDetay = secimIlceDetay;
            ViewBag.Parties = partiler.OrderByDescending(i => i.RatioAvg).ToList();
            ViewBag.Il = secimIl;
            ViewBag.Ilce = secimIlce;
            ViewBag.SecimMahalleler = secimMahalleler;
            ViewBag.SecimSonuclar = secimSonuclar;
            ViewBag.GecerliSecim = gecerliSecim;
            return View(secimIl);
        }





        public async Task<IActionResult> GetSecimChartData(int secimdetay)
        {
            var partiler = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  where a.SecimDetayId == secimdetay
                                  group a by new { a.PartiId, b.ShortName, b.Name } into grp
                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = (int)grp.Key.PartiId,
                                      PartyNameShort = grp.Key.ShortName,
                                  }).OrderBy(i => i.Name).ToListAsync();

            var secimSonuclar = await (from a in context.Sonuclars
                                       where a.SecimDetayId == secimdetay
                                       group a by new { a.SecimDetayId, a.PartiId } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           PartyId = grp.Key.PartiId,
                                           Vote = grp.Sum(i => i.OySayisi),
                                       }).ToListAsync();

            foreach (var item in partiler)
            {
                item.RatioAvg = secimSonuclar.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }


            return Ok(new { data = partiler.OrderByDescending(i => i.RatioAvg).ToList() });
        }

        public async Task<IActionResult> GetSecimIlChartData(int secimdetay, int secimil)
        {
            var partiler = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  where a.SecimDetayId == secimdetay
                                  group a by new { a.PartiId, b.ShortName, b.Name } into grp
                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = (int)grp.Key.PartiId,
                                      PartyNameShort = grp.Key.ShortName,
                                  }).OrderBy(i => i.Name).ToListAsync();

            var secimSonuclar = await (from a in context.Sonuclars
                                       where a.SecimDetayId == secimdetay && a.SecimIllerId == secimil
                                       group a by new { a.SecimIlcelerId, a.SecimDetayId, a.PartiId } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           IlceId = grp.Key.SecimIlcelerId,
                                           PartyId = grp.Key.PartiId,
                                           Vote = grp.Sum(i => i.OySayisi),
                                       }).ToListAsync();


            foreach (var item in partiler)
            {
                item.RatioAvg = secimSonuclar.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }


            return Ok(new { data = partiler.OrderByDescending(i => i.RatioAvg).ToList() });
        }

        public async Task<IActionResult> GetSecimIlPositionsChartData(int secimdetay, int secimil)
        {
            var partiler = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  join c in context.PoliticalPositions on b.PoliticalPositionId equals c.Id
                                  where a.SecimDetayId == secimdetay
                                  group a by new { c.Id, c.NameTr } into grp
                                  select new PartyListModel
                                  {
                                      Name = grp.Key.NameTr,
                                      Id = (int)grp.Key.Id,
                                      PartyNameShort = grp.Key.NameTr,
                                  }).OrderBy(i => i.Name).ToListAsync();

            var secimSonuclar = await (from a in context.Sonuclars
                                       join b in context.Parties on a.PartiId equals b.Id
                                       join c in context.PoliticalPositions on b.PoliticalPositionId equals c.Id
                                       where a.SecimDetayId == secimdetay && a.SecimIllerId == secimil
                                       group a by new { a.SecimIlcelerId, a.SecimDetayId, c.Id } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           IlceId = grp.Key.SecimIlcelerId,
                                           PartyId = grp.Key.Id,
                                           Vote = grp.Sum(i => i.OySayisi),
                                       }).ToListAsync();

            foreach (var item in partiler)
            {
                item.RatioAvg = secimSonuclar.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }


            return Ok(new { data = partiler.OrderByDescending(i => i.RatioAvg).ToList() });
        }

        public async Task<IActionResult> GetSecimIlcePositionsChartData(int secimdetay, int secimil, int secimilce)
        {
            var partiler = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  join c in context.PoliticalPositions on b.PoliticalPositionId equals c.Id
                                  where a.SecimDetayId == secimdetay
                                  group a by new { c.Id, c.NameTr } into grp
                                  select new PartyListModel
                                  {
                                      Name = grp.Key.NameTr,
                                      Id = (int)grp.Key.Id,
                                      PartyNameShort = grp.Key.NameTr,
                                  }).OrderBy(i => i.Name).ToListAsync();

            var secimSonuclar = await (from a in context.Sonuclars
                                       join b in context.Parties on a.PartiId equals b.Id
                                       join c in context.PoliticalPositions on b.PoliticalPositionId equals c.Id
                                       where a.SecimDetayId == secimdetay && a.SecimIllerId == secimil && a.SecimIlcelerId == secimilce
                                       group a by new { a.SecimMuhtarliklarId, a.SecimDetayId, c.Id } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           IlceId = grp.Key.SecimMuhtarliklarId,
                                           PartyId = grp.Key.Id,
                                           Vote = grp.Sum(i => i.OySayisi),
                                       }).ToListAsync();

            foreach (var item in partiler)
            {
                item.RatioAvg = secimSonuclar.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }


            return Ok(new { data = partiler.OrderByDescending(i => i.RatioAvg).ToList() });
        }

        public async Task<IActionResult> GetSecimIlIdeologyChartData(int secimdetay, int secimil)
        {
            var partiler = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id

                                  where a.SecimDetayId == secimdetay

                                  select new PartyListModel
                                  {
                                      Name = b.Name,
                                      Id = b.Id,
                                      PartyNameShort = b.ShortName,
                                      Ideoloies = b.PartyIdeologies.Select(i => new ComboBaseModel
                                      {
                                          Id = i.PoliticialIdeologyId,
                                          Weightiness = i.Value
                                      }).ToList(),
                                  }).OrderBy(i => i.Name).ToListAsync();

            var secimSonuclar = await (from a in context.Sonuclars
                                       where a.SecimDetayId == secimdetay && a.SecimIllerId == secimil
                                       group a by new { a.SecimDetayId, a.PartiId } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           PartyId = grp.Key.PartiId,
                                           Vote = grp.Sum(i => i.OySayisi),
                                       }).ToListAsync();



            foreach (var item in partiler)
            {
                item.RatioAvg = secimSonuclar.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }

            var ideologies = await (context.PoliticalIdeologies.Select(i => new PartyListModel
            {
                Id = i.Id,
                Name = i.NameTr,
                PartyNameShort = i.NameTr,
                RatioAvg = 0
            }).ToListAsync());

            foreach (var item in ideologies)
            {
                foreach (var party in partiler)
                {
                    var oran = party.Ideoloies.Where(i => i.Id == item.Id).Select(i => i.Weightiness).FirstOrDefault();

                    if (oran != null)
                    {
                        item.RatioAvg += party.RatioAvg * oran / 100;
                    }
                }
                item.RatioAvg = (double)Math.Round((decimal)item.RatioAvg);
            }

            return Ok(new { data = ideologies.OrderByDescending(i => i.RatioAvg).ToList() });
        }



        public async Task<IActionResult> GetSecimIlceChartData(int secimdetay, int secimil, int secimilce)
        {
            var partiler = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  where a.SecimDetayId == secimdetay
                                  group a by new { a.PartiId, b.ShortName, b.Name } into grp
                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = (int)grp.Key.PartiId,
                                      PartyNameShort = grp.Key.ShortName,
                                  }).OrderBy(i => i.Name).ToListAsync();

            var secimSonuclar = await (from a in context.Sonuclars
                                       where a.SecimDetayId == secimdetay && a.SecimIllerId == secimil && a.SecimIlcelerId == secimilce
                                       group a by new { a.SecimMuhtarliklarId, a.SecimDetayId, a.PartiId } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           IlceId = grp.Key.SecimMuhtarliklarId,
                                           PartyId = grp.Key.PartiId,
                                           Vote = grp.Sum(i => i.OySayisi),
                                       }).ToListAsync();

            foreach (var item in partiler)
            {
                item.RatioAvg = secimSonuclar.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }


            return Ok(new { data = partiler.OrderByDescending(i => i.RatioAvg).ToList() });
        }

        public async Task<IActionResult> GetSecimIlceIdeologyChartData(int secimdetay, int secimil, int secimilce)
        {
            var partiler = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  where a.SecimDetayId == secimdetay
                                  group a by new { a.PartiId, b.ShortName, b.Name } into grp
                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = (int)grp.Key.PartiId,
                                      PartyNameShort = grp.Key.ShortName,
                                      Ideoloies = context.PartyIdeologies.Where(i => i.PartyId == (int)grp.Key.PartiId).Select(i => new ComboBaseModel
                                      {
                                          Id = i.PoliticialIdeologyId,
                                          Weightiness = i.Value
                                      }).ToList(),
                                  }).OrderBy(i => i.Name).ToListAsync();

            var secimSonuclar = await (from a in context.Sonuclars
                                       where a.SecimDetayId == secimdetay && a.SecimIllerId == secimil && a.SecimIlcelerId == secimilce
                                       group a by new { a.SecimDetayId, a.PartiId } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           PartyId = grp.Key.PartiId,
                                           Vote = grp.Sum(i => i.OySayisi),
                                       }).ToListAsync();

            foreach (var item in partiler)
            {
                item.RatioAvg = secimSonuclar.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }

            var ideologies = await (context.PoliticalIdeologies.Select(i => new PartyListModel
            {
                Id = i.Id,
                Name = i.NameTr,
                PartyNameShort = i.NameTr,
                RatioAvg = 0
            }).ToListAsync());

            foreach (var item in ideologies)
            {
                foreach (var party in partiler)
                {
                    var oran = party.Ideoloies.Where(i => i.Id == item.Id).Select(i => i.Weightiness).FirstOrDefault();

                    if (oran != null)
                    {
                        item.RatioAvg += party.RatioAvg * oran / 100;
                    }
                }
                item.RatioAvg = (double)Math.Round((decimal)item.RatioAvg);
            }

            return Ok(new { data = ideologies.OrderByDescending(i => i.RatioAvg).ToList() });
        }

        public async Task<IActionResult> GetData(int t, string[] dates, int[] parties, int[] companies)
        {
            string[] date;

            switch (t)
            {
                case 1: //tarihe göre
                    //var months = dates.Select(i => i.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)).Select(i => Convert.ToInt32(i[1])).GroupBy(i => i).Select(i => i.Key).ToList();
                    var monthYears = dates.Select(i => i.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries))
                        .Select(i => new { year = Convert.ToInt32(i[0]), month = Convert.ToInt32(i[1]) }).OrderByDescending(i => i.year).ThenByDescending(i => i.month).ToList();



                    //int month = Convert.ToInt32(date.Last());
                    //int year = Convert.ToInt32(date.First());
                    var polls = await (from a in context.Polls
                                       where dates.Contains(a.Year.ToString() + "-" + a.Month.ToString()) && companies.Contains(a.PollCompanyId)
                                       select new
                                       {
                                           CompanyId = a.PollCompanyId,
                                           Id = a.Id,
                                           CompanyName = a.PollCompany.Name,
                                           CompanyShortName = a.PollCompany.ShortName,
                                           Month = a.Month,
                                           Year = a.Year,
                                           Results = a.PollResults.Where(k => k.Active && parties.Contains(k.PartyId)).Select(i => new
                                           {
                                               i.Ratio,
                                               i.PartyId,
                                               PartyName = i.Party.Name,
                                               PartyShortName = i.Party.ShortName,
                                               i.Active,

                                           }).OrderByDescending(l => l.Ratio).ToList()
                                       }).ToListAsync();
                    var partyList = polls.SelectMany(i => i.Results).Select(i => new { i.PartyId, i.PartyShortName }).Distinct().ToList();

                    return Ok(new { data = polls, dates = monthYears, parties = partyList });


                case 2://şirkete göre

                    int[] pollMonths = dates.Select(i => i.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)).Select(i => Convert.ToInt32(i[1])).ToArray();
                    int[] pollYears = dates.Select(i => i.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)).Select(i => Convert.ToInt32(i[0])).ToArray();
                    int companyId = companies[0];

                    var polls2 = await (from a in context.Polls
                                        where dates.Contains(a.Year.ToString() + "-" + a.Month.ToString()) && companies.Contains(a.PollCompanyId)
                                        select new
                                        {
                                            CompanyId = a.PollCompanyId,
                                            Id = a.Id,
                                            CompanyName = a.PollCompany.Name,
                                            CompanyShortName = a.PollCompany.ShortName,
                                            Month = a.Month,
                                            Year = a.Year,
                                            Results = a.PollResults.Where(k => k.Active && parties.Contains(k.PartyId)).Select(i => new
                                            {
                                                i.Ratio,
                                                i.PartyId,
                                                PartyName = i.Party.Name,
                                                PartyShortName = i.Party.ShortName,
                                                i.Active,

                                            }).OrderByDescending(l => l.Ratio).ToList()
                                        }).ToListAsync();
                    return Ok(polls2);


                case 3:// partiye göre


                    var polls3 = await (from a in context.Polls
                                        where dates.Contains(a.Year.ToString() + "-" + a.Month.ToString()) && companies.Contains(a.PollCompanyId)
                                        select new
                                        {
                                            CompanyId = a.PollCompanyId,
                                            Id = a.Id,
                                            CompanyName = a.PollCompany.Name,
                                            CompanyShortName = a.PollCompany.ShortName,
                                            Month = a.Month,
                                            Year = a.Year,
                                            Results = a.PollResults.Where(k => k.Active && parties.Contains(k.PartyId)).Select(i => new
                                            {
                                                i.Ratio,
                                                i.PartyId,
                                                PartyName = i.Party.Name,
                                                PartyShortName = i.Party.ShortName,
                                                i.Active,

                                            }).OrderByDescending(l => l.Ratio).ToList()
                                        }).ToListAsync();
                    return Ok(polls3);

                default:
                    break;
            }

            return Ok();
        }



        public async Task<IActionResult> ElectionComp(int? secim1=11, int? secim2=9)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;



            ViewBag.Iller = await context.SecimIlListesis.OrderBy(i => i.Adi).Select(i => new BaseModel { Id = i.Id, Name = i.Adi }).ToListAsync();

            var secimler = await (from a in context.Secims
                                  join b in context.SecimDetays on a.Id equals b.SecimId
                                  where b.SecimTuruId == 8
                                  orderby a.SecimTarihi descending
                                  select a).ToListAsync();


            ElectionSearchModel searchModel = new ElectionSearchModel();
            searchModel.SecimId = secim1 ?? secimler.FirstOrDefault().Id;


            var secimDetay = await context.SecimDetays.FirstOrDefaultAsync(a => a.SecimId == searchModel.SecimId && a.SecimTuruId == 8);

            var partiler = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  where a.SecimDetayId == secimDetay.Id
                                  group a by new { a.PartiId, b.ShortName, b.Name } into grp

                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = (int)grp.Key.PartiId,
                                      PartyNameShort = grp.Key.ShortName,

                                  }).OrderBy(i => i.Name).ToListAsync();

            var secimIller = await (from a in context.SecimIllers
                                    where a.SecimDetayId == secimDetay.Id
                                    select new ElectionProvinceModel
                                    {
                                        Id = a.IlId.Value,
                                        Name = a.Adi,
                                        RegionId = a.YskSecimCevresiId

                                    }).OrderBy(i => i.Name).ToListAsync();




            var secimSonuclar = await (from a in context.Sonuclars
                                       where a.SecimDetayId == secimDetay.Id
                                       group a by new { a.IlId, a.YskSecimCevresiId, a.PartiId,a.SecimIllerId } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           IlId = grp.Key.IlId,
                                           PartyId = grp.Key.PartiId,
                                           Vote = grp.Sum(i => i.OySayisi),
                                           YskSecimCevresiId = grp.Key.YskSecimCevresiId,
                                           IlListesiId=grp.Key.SecimIllerId,
                                       }).ToListAsync();

            foreach (var item in partiler)
            {
                item.RatioAvg = secimSonuclar.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }

            ViewBag.SecimDetay = secimDetay;
            ViewBag.Parties = partiler.OrderByDescending(i => i.RatioAvg).ToList();
            ViewBag.SecimIller = secimIller;
            ViewBag.SearchModel = searchModel;
            ViewBag.SecimSonuclar = secimSonuclar;

            var secimler2 = await (from a in context.Secims
                                  join b in context.SecimDetays on a.Id equals b.SecimId
                                  where b.SecimTuruId == 8
                                  orderby a.SecimTarihi descending
                                  select a).ToListAsync();


            ElectionSearchModel searchModel2 = new ElectionSearchModel();
            searchModel2.SecimId = secim2 ?? secimler2.FirstOrDefault().Id;


            var secimDetay2 = await context.SecimDetays.FirstOrDefaultAsync(a => a.SecimId == searchModel2.SecimId && a.SecimTuruId == 8);

            var partiler2 = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  where a.SecimDetayId == secimDetay2.Id
                                  group a by new { a.PartiId, b.ShortName, b.Name } into grp

                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = (int)grp.Key.PartiId,
                                      PartyNameShort = grp.Key.ShortName,

                                  }).OrderBy(i => i.Name).ToListAsync();

            var secimIller2 = await (from a in context.SecimIllers
                                    where a.SecimDetayId == secimDetay2.Id
                                    select new ElectionProvinceModel
                                    {
                                        Id = a.IlId.Value,
                                        Name = a.Adi,
                                        RegionId = a.YskSecimCevresiId

                                    }).OrderBy(i => i.Name).ToListAsync();




            var secimSonuclar2 = await (from a in context.Sonuclars
                                       where a.SecimDetayId == secimDetay2.Id
                                       group a by new { a.IlId, a.YskSecimCevresiId, a.PartiId ,a.SecimIllerId} into grp
                                       select new ElectionPartyResultModel
                                       {
                                           IlId = grp.Key.IlId,
                                           PartyId = grp.Key.PartiId,
                                           Vote = grp.Sum(i => i.OySayisi),
                                           YskSecimCevresiId = grp.Key.YskSecimCevresiId,
                                           IlListesiId = grp.Key.SecimIllerId,

                                       }).ToListAsync();

            foreach (var item in partiler2)
            {
                item.RatioAvg = secimSonuclar2.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }

            ViewBag.SecimDetay2 = secimDetay2;
            ViewBag.Parties2 = partiler2.OrderByDescending(i => i.RatioAvg).ToList();
            ViewBag.SecimIller2 = secimIller2;
            ViewBag.SearchModel2 = searchModel2;
            ViewBag.SecimSonuclar2 = secimSonuclar2;



            return View(secimler);
        }

        [Route("secim-il-detaycomp", Name = "secim-il-detaycomp")]
        [Route("election-province-detailcomp", Name = "secim-il-detay2comp")]
        public async Task<IActionResult> ElectionComp1(int secim, int id, int cevreid, int secim1, int id1, int cevreid1)
        {

            var gecerliSecim = await (from a in context.Secims
                                      join b in context.SecimDetays on a.Id equals b.SecimId
                                      where a.Id == secim
                                      orderby a.SecimTarihi descending
                                      select a).FirstOrDefaultAsync();

            var secimDetay = await context.SecimDetays.FirstOrDefaultAsync(a => a.SecimId == secim && a.SecimTuruId == 8);



            var partiler = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  where a.SecimDetayId == secimDetay.Id
                                  group a by new { a.PartiId, b.ShortName, b.Name } into grp

                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = (int)grp.Key.PartiId,
                                      PartyNameShort = grp.Key.ShortName,

                                  }).OrderBy(i => i.Name).ToListAsync();


            var secimIl = await (from a in context.SecimIllers
                                 where a.SecimDetayId == secimDetay.Id && a.IlId == id && a.YskSecimCevresiId == cevreid
                                 select new ElectionProvinceModel
                                 {
                                     Id = a.Id,

                                     Name = a.Adi,
                                     RegionId = a.YskSecimCevresiId

                                 }).FirstOrDefaultAsync();

            var secimIlDetay = await context.SecimMuhtarlikOzets.Where(i => i.SecimDetayId == secimDetay.Id && i.SecimIllerId == (int)secimIl.Id).ToListAsync();

            var secimIlceler = await (from a in context.SecimIlcelers
                                      where a.YskSecimCevresiId == cevreid && a.IlId == secimIl.Id
                                      select new ElectionProvinceModel
                                      {
                                          Id = a.Id,
                                          Name = a.Adi,
                                          RegionId = a.YskSecimCevresiId

                                      }).OrderBy(i => i.Name).ToListAsync();



            var secimSonuclar = await (from a in context.Sonuclars
                                       where a.SecimDetayId == secimDetay.Id && a.SecimIllerId == secimIl.Id && a.YskSecimCevresiId == secimIl.RegionId
                                       group a by new { a.SecimIlcelerId, a.YskSecimCevresiId, a.PartiId, a.SecimIlceListesiId } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           IlceId = grp.Key.SecimIlcelerId,
                                           PartyId = grp.Key.PartiId,
                                           Vote = grp.Sum(i => i.OySayisi),
                                           YskSecimCevresiId = grp.Key.YskSecimCevresiId,
                                           IlceListesiId=grp.Key.SecimIlceListesiId,

                                       }).ToListAsync();

            foreach (var item in partiler)
            {
                item.RatioAvg = secimSonuclar.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }

            ViewBag.SecimDetay = secimDetay;
            ViewBag.SecimilDetay = secimIlDetay;
            ViewBag.Parties = partiler.OrderByDescending(i => i.RatioAvg).ToList();
            ViewBag.Il = secimIl;
            ViewBag.SecimIlceler = secimIlceler;
            ViewBag.SecimSonuclar = secimSonuclar;
            ViewBag.GecerliSecim = gecerliSecim;


            var gecerliSecim2 = await (from a in context.Secims
                                      join b in context.SecimDetays on a.Id equals b.SecimId
                                      where a.Id == secim1
                                      orderby a.SecimTarihi descending
                                      select a).FirstOrDefaultAsync();

            var secimDetay2 = await context.SecimDetays.FirstOrDefaultAsync(a => a.SecimId == secim1 && a.SecimTuruId == 8);



            var partiler2 = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  where a.SecimDetayId == secimDetay2.Id
                                  group a by new { a.PartiId, b.ShortName, b.Name } into grp

                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = (int)grp.Key.PartiId,
                                      PartyNameShort = grp.Key.ShortName,

                                  }).OrderBy(i => i.Name).ToListAsync();


            var secimIl2 = await (from a in context.SecimIllers
                                 where a.SecimDetayId == secimDetay2.Id && a.IlId == id && a.YskSecimCevresiId == cevreid1
                                 select new ElectionProvinceModel
                                 {
                                     Id = a.Id,

                                     Name = a.Adi,
                                     RegionId = a.YskSecimCevresiId

                                 }).FirstOrDefaultAsync();

            var secimIlDetay2 = await context.SecimMuhtarlikOzets.Where(i => i.SecimDetayId == secimDetay2.Id && i.SecimIllerId == (int)secimIl2.Id).ToListAsync();

            var secimIlceler2 = await (from a in context.SecimIlcelers
                                      where a.YskSecimCevresiId == cevreid1 && a.IlId == secimIl2.Id
                                      select new ElectionProvinceModel
                                      {
                                          Id = a.Id,
                                          Name = a.Adi,
                                          RegionId = a.YskSecimCevresiId

                                      }).OrderBy(i => i.Name).ToListAsync();



            var secimSonuclar2 = await (from a in context.Sonuclars
                                       where a.SecimDetayId == secimDetay2.Id && a.SecimIllerId == secimIl2.Id && a.YskSecimCevresiId == secimIl2.RegionId
                                       group a by new { a.SecimIlcelerId, a.YskSecimCevresiId, a.PartiId, a.SecimIlceListesiId } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           IlceId = grp.Key.SecimIlcelerId,
                                           PartyId = grp.Key.PartiId,
                                           Vote = grp.Sum(i => i.OySayisi),
                                           YskSecimCevresiId = grp.Key.YskSecimCevresiId,
                                           IlceListesiId = grp.Key.SecimIlceListesiId,
                                       }).ToListAsync();

            foreach (var item in partiler2)
            {
                item.RatioAvg = secimSonuclar2.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }

            ViewBag.SecimDetay2 = secimDetay2;
            ViewBag.SecimilDetay2 = secimIlDetay2;
            ViewBag.Parties2 = partiler2.OrderByDescending(i => i.RatioAvg).ToList();
            ViewBag.Il2 = secimIl2;
            ViewBag.SecimIlceler2 = secimIlceler2;
            ViewBag.SecimSonuclar2 = secimSonuclar2;
            ViewBag.GecerliSecim2 = gecerliSecim2;


            return View(secimIl);
        }


        [Route("secim-mah-detay", Name = "secim-mah-detay")]
        [Route("election-district2-detail", Name = "secim-mah-detay2")]
        public async Task<IActionResult> ElectionComp2(int secim, int id, int ilId, int cevreid, int secim2, int id2, int ilId2, int cevreid2)
        {

            var gecerliSecim = await (from a in context.Secims
                                      join b in context.SecimDetays on a.Id equals b.SecimId
                                      where a.Id == secim
                                      orderby a.SecimTarihi descending
                                      select a).FirstOrDefaultAsync();

            var secimDetay = await context.SecimDetays.FirstOrDefaultAsync(a => a.SecimId == secim && a.SecimTuruId == 8);

            var partiler = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  where a.SecimDetayId == secimDetay.Id
                                  group a by new { a.PartiId, b.ShortName, b.Name } into grp

                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = (int)grp.Key.PartiId,
                                      PartyNameShort = grp.Key.ShortName,

                                  }).OrderBy(i => i.Name).ToListAsync();


            var secimIl = await (from a in context.SecimIllers
                                 where a.SecimDetayId == secimDetay.Id && a.Id == ilId && a.YskSecimCevresiId == cevreid
                                 select new ElectionProvinceModel
                                 {
                                     Id = a.Id,

                                     Name = a.Adi,
                                     RegionId = a.YskSecimCevresiId

                                 }).FirstOrDefaultAsync();

            var secimIlce = await (from a in context.SecimIlcelers
                                   where a.SecimDetayId == secimDetay.Id && a.IlId == ilId && a.YskSecimCevresiId == cevreid && a.Id == id
                                   select new ElectionProvinceModel
                                   {
                                       Id = a.Id,

                                       Name = a.Adi,
                                       RegionId = a.YskSecimCevresiId

                                   }).FirstOrDefaultAsync();

            var secimIlceDetay = await context.SecimMuhtarlikOzets.Where(i => i.SecimDetayId == secimDetay.Id && i.SecimIlcelerId == (int)secimIlce.Id).ToListAsync();

            var secimMahalleler = await (from a in context.SecimMuhtarliklars
                                         where a.SecimDetayId == secimDetay.Id && a.IlceId == id
                                         select new ElectionProvinceModel
                                         {
                                             Id = a.Id,
                                             Name = a.Adi,
                                             RegionId = a.YskMuhtarlikId,

                                         }).OrderBy(i => i.Name).ToListAsync();



            var secimSonuclar = await (from a in context.Sonuclars
                                       where a.SecimDetayId == secimDetay.Id && a.SecimIllerId == secimIl.Id && a.YskSecimCevresiId == secimIl.RegionId && a.SecimIlcelerId == id
                                       group a by new { a.SecimMuhtarliklarId, a.YskSecimCevresiId, a.PartiId } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           MuhtarlikId = grp.Key.SecimMuhtarliklarId,
                                           PartyId = grp.Key.PartiId,
                                           Vote = grp.Sum(i => i.OySayisi),
                                           YskSecimCevresiId = grp.Key.YskSecimCevresiId

                                       }).ToListAsync();

            foreach (var item in partiler)
            {
                item.RatioAvg = secimSonuclar.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }

            ViewBag.SecimDetay = secimDetay;
            ViewBag.SecimIlceDetay = secimIlceDetay;
            ViewBag.Parties = partiler.OrderByDescending(i => i.RatioAvg).ToList();
            ViewBag.Il = secimIl;
            ViewBag.Ilce = secimIlce;
            ViewBag.SecimMahalleler = secimMahalleler;
            ViewBag.SecimSonuclar = secimSonuclar;
            ViewBag.GecerliSecim = gecerliSecim;


            var gecerliSecim2 = await (from a in context.Secims
                                      join b in context.SecimDetays on a.Id equals b.SecimId
                                      where a.Id == secim2
                                      orderby a.SecimTarihi descending
                                      select a).FirstOrDefaultAsync();

            var secimDetay2 = await context.SecimDetays.FirstOrDefaultAsync(a => a.SecimId == secim2 && a.SecimTuruId == 8);

            var partiler2 = await (from a in context.SecimPartilers
                                  join b in context.Parties on a.PartiId equals b.Id
                                  where a.SecimDetayId == secimDetay2.Id
                                  group a by new { a.PartiId, b.ShortName, b.Name } into grp

                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = (int)grp.Key.PartiId,
                                      PartyNameShort = grp.Key.ShortName,

                                  }).OrderBy(i => i.Name).ToListAsync();


            var secimIl2 = await (from a in context.SecimIllers
                                 where a.SecimDetayId == secimDetay2.Id && a.Id == ilId2 && a.YskSecimCevresiId == cevreid2
                                 select new ElectionProvinceModel
                                 {
                                     Id = a.Id,

                                     Name = a.Adi,
                                     RegionId = a.YskSecimCevresiId

                                 }).FirstOrDefaultAsync();

            var secimIlce2 = await (from a in context.SecimIlcelers
                                   where a.SecimDetayId == secimDetay2.Id && a.IlId == ilId2 && a.YskSecimCevresiId == cevreid2 && a.Id == id2
                                   select new ElectionProvinceModel
                                   {
                                       Id = a.Id,

                                       Name = a.Adi,
                                       RegionId = a.YskSecimCevresiId

                                   }).FirstOrDefaultAsync();

            var secimIlceDetay2 = await context.SecimMuhtarlikOzets.Where(i => i.SecimDetayId == secimDetay2.Id && i.SecimIlcelerId == (int)secimIlce2.Id).ToListAsync();

            var secimMahalleler2 = await (from a in context.SecimMuhtarliklars
                                         where a.SecimDetayId == secimDetay2.Id && a.IlceId == id2
                                         select new ElectionProvinceModel
                                         {
                                             Id = a.Id,
                                             Name = a.Adi,
                                             RegionId=a.YskMuhtarlikId,

                                         }).OrderBy(i => i.Name).ToListAsync();



            var secimSonuclar2 = await (from a in context.Sonuclars
                                       where a.SecimDetayId == secimDetay2.Id && a.SecimIllerId == secimIl2.Id && a.YskSecimCevresiId == secimIl2.RegionId && a.SecimIlcelerId == id2
                                       group a by new { a.SecimMuhtarliklarId, a.YskSecimCevresiId, a.PartiId } into grp
                                       select new ElectionPartyResultModel
                                       {
                                           MuhtarlikId = grp.Key.SecimMuhtarliklarId,
                                           PartyId = grp.Key.PartiId,
                                           Vote = grp.Sum(i => i.OySayisi),
                                           YskSecimCevresiId = grp.Key.YskSecimCevresiId,

                                       }).ToListAsync();

            foreach (var item in partiler2)
            {
                item.RatioAvg = secimSonuclar2.Where(i => i.PartyId == item.Id).Sum(i => i.Vote);
            }

            ViewBag.SecimDetay2 = secimDetay2;
            ViewBag.SecimIlceDetay2 = secimIlceDetay2;
            ViewBag.Parties2 = partiler2.OrderByDescending(i => i.RatioAvg).ToList();
            ViewBag.Il2 = secimIl2;
            ViewBag.Ilce2 = secimIlce2;
            ViewBag.SecimMahalleler2 = secimMahalleler2;
            ViewBag.SecimSonuclar2 = secimSonuclar2;
            ViewBag.GecerliSecim2 = gecerliSecim2;

            return View(secimIl);
        }




    }
}
