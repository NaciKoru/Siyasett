using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json;
using Siyasett.Core;
using Siyasett.Core.Elections;
using Siyasett.Core.Party;
using Siyasett.Core.Poll;
using Siyasett.Data.Data;
using Siyasett.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;

namespace Siyasett.Web.Controllers
{
    public class SimulationsController : Controller
    {

        protected readonly AppDbContext context;
        private readonly ILogger<PeopleController> _logger;

        public SimulationsController(AppDbContext context, ILogger<PeopleController> logger)
        {
            this.context = context;
            _logger = logger;
        }


        [Route("simulasyonlar")]
        [Route("simulations")]
        public async Task<IActionResult> Index()
        {

            var secimler = await (from a in context.Secims
                                  join b in context.SecimDetays on a.Id equals b.SecimId
                                  where b.SecimTuruId == 8
                                  orderby a.SecimTarihi descending
                                  select new SecimListModel
                                  {
                                      Id = a.Id,
                                      Adi = a.Adi,
                                      SecimDetayId = b.Id
                                  }).ToListAsync();
            secimler.First().Secili = true;
            var secim = secimler.First();

            ViewBag.AnketFirmalari = await context.Companies.Where(i => i.Polls.Any()).OrderBy(i => i.Name).AsNoTracking().ToListAsync();

            ViewBag.Secimler = secimler;


            var partiler = await (from a in context.Parties
                                  where a.Active && a.Name != "Diğer" && a.ParticipateElection

                                  orderby a.MemberCount descending
                                  select new PartyListModel
                                  {
                                      Name = a.Name,
                                      Id = a.Id,
                                      PartyNameShort = a.ShortName,
                                      PartyName = a.Name
                                  }).ToListAsync();
            ViewBag.Partiler = partiler;

            ViewBag.SecimIller = await (from a in context.SecimIllers
                                        where a.SecimDetayId == secim.SecimDetayId
                                        select new ElectionProvinceModel
                                        {
                                            Id = a.IlId.Value,
                                            Name = a.Adi,
                                            RegionId = a.YskSecimCevresiId

                                        }).OrderBy(i => i.Name).ToListAsync();


            return View();
        }

        /// <summary>
        /// seçimdeki illerin isimlerini geri döner
        /// </summary>
        /// <param name="id">seçim detay id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetStep2(int secimId)
        {
            var secimDetay = await (from a in context.SecimDetays
                                    where a.SecimId == secimId
                                    select a).FirstOrDefaultAsync();
            var iller = await (from a in context.SecimIllers
                               where a.SecimDetayId == secimDetay.Id
                               select new ElectionProvinceModel
                               {
                                   Id = a.IlId.Value,
                                   Name = a.Adi,
                                   RegionId = a.YskSecimCevresiId

                               }).OrderBy(i => i.Name).ToListAsync();
            //var anketler = await (from a in context.PollResults
            //                      where firmalar.Contains(a.Poll.PollCompanyId) && partiler.Contains(a.PartyId)
            //                      group a by new {PartyId=a.PartyId,CompanyId=a.Poll.PollCompanyId, PollYear=a.Poll.Year, PollMonth = a.Poll.Month} into grp
            //                      select new
            //                      {
            //                          PartyId=grp.Key.PartyId,
            //                          Ratio=grp.Last().Ratio,
            //                          CompanyId=grp.Key.CompanyId,
            //                          PollYear=grp.Key.PollYear,
            //                          PollMonth = grp.Key.PollMonth,
            //                      }
            //                   ).ToListAsync();
            return Ok(iller);
        }



        public async Task<IActionResult> GetPartiRatiosElectionWithoutRegion(int secimId, int[] simPartiler, int[] simSirketler)
        {
            object[] veri = new object[4];


            var secimler = await (from a in context.Secims
                                  join b in context.SecimDetays on a.Id equals b.SecimId
                                  where b.SecimTuruId == 8
                                  orderby a.SecimTarihi descending
                                  select a).ToListAsync();


            var secimDetay = await context.SecimDetays.FirstOrDefaultAsync(a => a.SecimId == secimId && a.SecimTuruId == 8);

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


            veri[0] = secimler.ToArray();
            veri[1] = secimDetay;
            veri[2] = partiler.OrderByDescending(i => i.RatioAvg).ToList().ToArray();
            veri[3] = secimSonuclar.ToArray();

            return Ok(veri);
        }

        public async Task<IActionResult> GetPartiRatiosFromElection(int secimId, int[] simPartiler, int[] simIller)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("tr-TR");
            object[] veri = new object[5];

            var secimDetay = await context.SecimDetays.FirstOrDefaultAsync(a => a.SecimId == secimId && a.SecimTuruId == 8);

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
                                        RegionId = a.YskSecimCevresiId,
                                        SecimIllerId = a.Id,

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

            veri[0] = secimDetay;
            veri[1] = partiler.ToArray();
            veri[2] = secimIller.ToArray();
            veri[3] = secimSonuclar.ToArray();
            veri[4] = context.Parties.ToArray();

            return Ok(veri);
        }

        public async Task<IActionResult> GetPartiRatiosFromPolls(int secimId, int[] simPartiler, int[] simSirketler)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("tr-TR");
            object[] veri = new object[5];

            var secimDetay = await context.SecimDetays.FirstOrDefaultAsync(a => a.SecimId == secimId && a.SecimTuruId == 8);

            var partiler = await (from a in context.Parties
                                  select new PartyListModel
                                  {
                                      Name = a.Name,
                                      Id = a.Id,
                                      PartyNameShort = a.ShortName,
                                  }).OrderByDescending(i => i.Id).ToListAsync();
            var tarihler = await (from a in context.Polls
                                  where a.Active
                                  group a by new { a.Year, a.Month } into grp
                                  select new PollDateModel
                                  {
                                      Year = grp.Key.Year,
                                      Month = grp.Key.Month,
                                      MonthName = cultureInfo.DateTimeFormat.GetMonthName(grp.Key.Month),

                                  }
                          ).ToListAsync();

            tarihler.ForEach((i) => i.YearMonth = Convert.ToInt32(i.Year.ToString() + i.Month.ToString("000")));

            var polls = await (from a in context.PollResults
                               where a.Poll.Active && a.Active && simPartiler.Contains(a.PartyId) && simSirketler.Contains(a.Poll.PollCompanyId)
                               select new PollResultListModel
                               {

                                   PartyId = a.PartyId,
                                   Ratio = a.Ratio,
                                   PollId = a.PollId,
                                   Id = a.Id,
                                   Month = a.Poll.Month,
                                   Year = a.Poll.Year,
                                   CompanyId = a.Poll.PollCompanyId,
                               }).ToListAsync();


            veri[0] = secimDetay;
            veri[1] = partiler.ToArray();
            veri[2] = tarihler.ToArray();
            veri[3] = polls.ToArray();
            veri[4] = await context.Companies.Where(i => i.Polls.Any()).OrderBy(i => i.Name).AsNoTracking().ToArrayAsync();

            return Ok(veri);
        }


        public async Task<IActionResult> GecimSecimdeniller(int secimId)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("tr-TR");

            var secimDetay = await context.SecimDetays.FirstOrDefaultAsync(a => a.SecimId == secimId && a.SecimTuruId == 8);



            var secimIller = await (from a in context.SecimIllers
                                    where a.SecimDetayId == secimDetay.Id
                                    select new ElectionProvinceModel
                                    {
                                        Id = a.IlId.Value,
                                        Name = a.Adi,
                                        RegionId = a.YskSecimCevresiId

                                    }).OrderBy(i => i.Name).ToListAsync();

            return Ok(secimIller);
        }


        public async Task<IActionResult> GetPartyIdeolojies(int[] partiler)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("tr-TR");

            var partyIdeologies = await (from a in context.PartyIdeologies
                                         where partiler.Contains(a.PartyId)
                                         select new PartyIdeology
                                         {
                                             Id = a.Id,
                                             PartyId = a.PartyId,
                                             PoliticialIdeologyId = a.PoliticialIdeologyId,
                                             PoliticialIdeology = a.PoliticialIdeology,
                                             Value = a.Value,
                                             Party = a.Party,
                                         }).OrderBy(i => i.PartyId).ToListAsync();

            return Ok(partyIdeologies);
        }


        public async Task<IActionResult> GetParties()
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("tr-TR");

            var partiler = await (from a in context.Parties
                                  where a.Active && a.Name != "Diğer" && a.ParticipateElection
                                  orderby a.MemberCount descending
                                  select new PartyListModel
                                  {
                                      Name = a.Name,
                                      Id = a.Id,
                                      PartyNameShort = a.ShortName,
                                      PartyName = a.Name
                                  }).ToListAsync();

            return Ok(partiler);
        }


        public IActionResult GetMilVekSayisi(int id)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("tr-TR");

            var sayi = context.SecimIllers.FirstOrDefault(a => a.YskSecimCevresiId == id).AdaySayisi;

            return Ok(sayi);
        }


        public async Task<IActionResult> IlDetayForSimulasyon(int secim, int id, int cevreid)
        {

            object[] veri = new object[5];

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


            veri[3] = secimSonuclar;

            //ViewBag.SecimDetay = secimDetay;
            veri[0] = secimIlDetay;
            //ViewBag.Parties = partiler.OrderByDescending(i => i.RatioAvg).ToList();
            //ViewBag.Il = secimIl;
            //ViewBag.SecimIlceler = secimIlceler;
            //ViewBag.SecimSonuclar = secimSonuclar;
            //ViewBag.GecerliSecim = gecerliSecim;
            return Ok(veri);
        }



        public ActionResult SimulationResult(string datasForSimGraphic, string ittifaklars)
        {

            List<Ittifak> myDeserializedClassittifak = JsonConvert.DeserializeObject<List<Ittifak>>(ittifaklars);
            List<Root> myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(datasForSimGraphic);
            var partiler = context.Parties.ToList();
            //var result = new { data = datasForSimGraphic, itt = ittifaklars };
            ViewBag.DeserializedClass = myDeserializedClass;
            ViewBag.myDeserializedClassittifak = myDeserializedClassittifak;
            ViewBag.Partiler = partiler;
            return View();
        }


        public class Data
        {
            public List<DataSonucPartyi> dataSonucPartyi { get; set; }
            public List<string> iller { get; set; }
            public string ilAdi { get; set; }
            public int gecOySayi { get; set; }
            public List<HesapMil> hesapMil { get; set; }
            public int topMilSay { get; set; }
            public List<GenelOylarSecim> genelOylarSecim { get; set; }
            public List<GenelOylarAnket> genelOylarAnket { get; set; }
            public List<PartyTahminOylar> partyTahminOylar { get; set; }

        }
        public class PartyTahminOylar
        {
            public string partiId { get; set; }
            public string oran { get; set; }
        }
        public class GenelOylarAnket
        {
            public string partiId { get; set; }
            public string oran { get; set; }
        }


        public class GenelOylarSecim
        {
            public int partiId { get; set; }
            public string oran { get; set; }
        }

        public class DataSonucPartyi
        {
            public string ekranaYazilcakPartyId { get; set; }
            public string ekranPartyOran { get; set; }
            public int ekranilId { get; set; }
        }

        public class HesapMil
        {
            public string partiId { get; set; }
            public string oyOran { get; set; }
            public int milSay { get; set; }
        }

        public class Root
        {
            public Data data { get; set; }
        }

        public class Ittifak
        {
            public string ittifakno { get; set; }
            public string ittifakparti { get; set; }
            public string ittifakname { get; set; }
        }

        public ActionResult SimulationResult2(string datasForSimGraphic, string ittifaklars)
        {

            List<IttifakParti2> myDeserializedClassittifak = JsonConvert.DeserializeObject<List<IttifakParti2>>(ittifaklars);
            List<Result2> myDeserializedClass = JsonConvert.DeserializeObject<List<Result2>>(datasForSimGraphic);
            var partiler = context.Parties.ToList();
            //var result = new { data = datasForSimGraphic, itt = ittifaklars };
            ViewBag.DeserializedClass = myDeserializedClass;
            ViewBag.myDeserializedClassittifak = myDeserializedClassittifak;
            ViewBag.Partiler = partiler;
            return View();
        }

        public class GecerliOy
        {
            public string ilTopGecOy { get; set; }
        }

        public class Iladi
        {
            public string ilAdi { get; set; }
        }


        public class PartiOran
        {
            public string partiId { get; set; }
            public string oran { get; set; }
            public string milvelSayisi { get; set; }
        }

        public class Result2
        {
            public Iladi iladi { get; set; }
            public GecerliOy gecerliOy { get; set; }
            public List<PartiOran> partiOran { get; set; }
            public string toplamMilSayisi { get; set; }
            public List<OncekiOranlar> oncekiOranlar { get; set; }
            public List<PartyTahminOylar> partyTahminOylar { get; set; }
        }

        public class IttifakParti2
        {
            public string ittifakAdi { get; set; }
            public string partiId { get; set; }
            public bool @checked { get; set; }
            public string ittifakName { get; set; }
        }

        public ActionResult SimulationResult3(string datasForSimGraphic, string ittifaklars)
        {

            List<IttifakParti2> myDeserializedClassittifak = JsonConvert.DeserializeObject<List<IttifakParti2>>(ittifaklars);
            List<Result2> myDeserializedClass = JsonConvert.DeserializeObject<List<Result2>>(datasForSimGraphic);
            var partiler = context.Parties.ToList();
            //var result = new { data = datasForSimGraphic, itt = ittifaklars };
            ViewBag.DeserializedClass = myDeserializedClass;
            ViewBag.myDeserializedClassittifak = myDeserializedClassittifak;
            ViewBag.Partiler = partiler;
            return View();
        }

        [RequestSizeLimit(100_000_000)]
        
        public async Task<IActionResult> TumIllerSonuclar(string ilVeriler)
        {

            List<TumIllerResultModel> ilGelenVeriler = JsonConvert.DeserializeObject<List<TumIllerResultModel>>(ilVeriler);
            object[] genelVeri = new object[ilGelenVeriler.Count()];
            var milletvekiliSayilari = await context.SecimIllers.Select(i => new
            {
                YskSecimCevresiId = i.YskSecimCevresiId,
                AdaySayisi = i.AdaySayisi
            }).ToListAsync(); // FirstOrDefault(a => a.YskSecimCevresiId == Convert.ToInt32(item.cevreId)).AdaySayisi;

            var secimDetaylari = await context.SecimDetays.Where(a => a.SecimTuruId == 8).Select(i=>new {i.Id,i.SecimId}).ToListAsync() ;

            var secimIller = await (from a in context.SecimIllers
                                 //where a.SecimDetayId == secimDetay.Id && a.IlId == item.klasikIlId && a.YskSecimCevresiId == Convert.ToInt32(item.cevreId)
                                 select new 
                                 {
                                     Id = a.Id,
                                     Name = a.Adi,
                                     RegionId = a.YskSecimCevresiId,
                                     a.SecimDetayId,
                                     a.IlId,
                                     a.YskSecimCevresiId

                                 }).ToListAsync();


            var secimPartiler = await (from a in context.SecimPartilers
                                   join b in context.Parties on a.PartiId equals b.Id
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
                                       SecimDetayId=a.SecimDetayId,
                                   }).OrderBy(i => i.Name).ToListAsync();

            //var secimdetaygrup = ilGelenVeriler.GroupBy(i => new { i.secimturuId, i.secimdetay }).Select(i => new { i.Key.secimturuId, i.Key.secimdetay }).ToList();
            List<int> secimTurleri= ilGelenVeriler.GroupBy(i=>i.secimturuId).Select(i=> Convert.ToInt32( i.Key)).ToList();
            List<int> secimDetayIds = secimDetaylari.Where(i => secimTurleri.Contains(i.SecimId.Value)).Select(i=>i.Id).ToList();
            List<int> secimDetayGruplari = ilGelenVeriler.GroupBy(i => i.secimdetay).Select(i => Convert.ToInt32(i.Key)).ToList();



            for (int i = 0; i < genelVeri.Length; i++)
            {
                var item = ilGelenVeriler[i];

                object[] veri = new object[5];

                var milvelsayi = milletvekiliSayilari.FirstOrDefault(i => i.YskSecimCevresiId == Convert.ToInt32(item.cevreId)).AdaySayisi;  //context.SecimIllers.FirstOrDefault(a => a.YskSecimCevresiId == Convert.ToInt32(item.cevreId)).AdaySayisi;
                veri[0] = milvelsayi;
                // ildetayforsimulasyon



                //var gecerliSecim = await (from a in context.Secims
                //                          join b in context.SecimDetays on a.Id equals b.SecimId
                //                          where a.Id == Convert.ToInt32(item.secimturuId)
                //                          orderby a.SecimTarihi descending
                //                          select a).FirstOrDefaultAsync();

                var secimDetay = secimDetaylari.FirstOrDefault(a => a.SecimId == Convert.ToInt32(item.secimturuId));  //await context.SecimDetays.FirstOrDefaultAsync(a => a.SecimId == Convert.ToInt32(item.secimturuId) && a.SecimTuruId == 8);



                //var partiler = await (from a in context.SecimPartilers
                //                      join b in context.Parties on a.PartiId equals b.Id
                //                      where a.SecimDetayId == secimDetay.Id
                //                      group a by new { a.PartiId, b.ShortName, b.Name } into grp

                //                      select new PartyListModel
                //                      {
                //                          Name = grp.Key.Name,
                //                          Id = (int)grp.Key.PartiId,
                //                          PartyNameShort = grp.Key.ShortName,

                //                      }).OrderBy(i => i.Name).ToListAsync();

                var partiler = secimPartiler.Where(a => a.SecimDetayId == secimDetay.Id).ToList();

                //var secimIl = await (from a in context.SecimIllers
                //                     where a.SecimDetayId == secimDetay.Id && a.IlId == item.klasikIlId && a.YskSecimCevresiId == Convert.ToInt32(item.cevreId)
                //                     select new ElectionProvinceModel
                //                     {
                //                         Id = a.Id,
                //                         Name = a.Adi,
                //                         RegionId = a.YskSecimCevresiId

                //                     }).FirstOrDefaultAsync();

                var secimIl = secimIller.FirstOrDefault(a => a.SecimDetayId == secimDetay.Id && a.IlId == item.klasikIlId && a.YskSecimCevresiId == Convert.ToInt32(item.cevreId));

                var secimIlDetay = await context.SecimMuhtarlikOzets.Where(i => i.SecimDetayId == secimDetay.Id && i.SecimIllerId == (int)secimIl.Id).ToListAsync();

                //var secimIlceler = await (from a in context.SecimIlcelers
                //                          where a.YskSecimCevresiId == Convert.ToInt32(item.cevreId) && a.IlId == secimIl.Id
                //                          select new ElectionProvinceModel
                //                          {
                //                              Id = a.Id,
                //                              Name = a.Adi,
                //                              RegionId = a.YskSecimCevresiId

                //                          }).OrderBy(i => i.Name).ToListAsync();



                var secimSonuclar = await (from a in context.Sonuclars
                                           where a.SecimDetayId == secimDetay.Id && a.SecimIllerId == secimIl.Id && a.YskSecimCevresiId == secimIl.RegionId
                                           group a by new { a.SecimIlcelerId, a.YskSecimCevresiId, a.PartiId } into grp
                                           select new ElectionPartyResultModel
                                           {
                                               IlceId = grp.Key.SecimIlcelerId,
                                               PartyId = grp.Key.PartiId,
                                               Vote = grp.Sum(i => i.OySayisi),
                                               YskSecimCevresiId = grp.Key.YskSecimCevresiId,

                                           }).ToListAsync();

                foreach (var item2 in partiler)
                {
                    item2.RatioAvg = secimSonuclar.Where(i => i.PartyId == item2.Id).Sum(i => i.Vote);
                }


                veri[1] = secimSonuclar;
                veri[2] = secimIlDetay;

                ////GetSecimIlIdeologyChartData

                //var partiler2 = await (from a in context.SecimPartilers
                //                       join b in context.Parties on a.PartiId equals b.Id

                //                       where a.SecimDetayId == item.secimdetay

                //                       select new PartyListModel
                //                       {
                //                           Name = b.Name,
                //                           Id = b.Id,
                //                           PartyNameShort = b.ShortName,
                //                           Ideoloies = b.PartyIdeologies.Select(i => new ComboBaseModel
                //                           {
                //                               Id = i.PoliticialIdeologyId,
                //                               Weightiness = i.Value
                //                           }).ToList(),
                //                       }).OrderBy(i => i.Name).ToListAsync();

                var partiler2 = secimPartiler.Where(a => a.SecimDetayId == item.secimdetay).ToList();

                var secimSonuclar2 = await (from a in context.Sonuclars
                                            where a.SecimDetayId == item.secimdetay && a.SecimIllerId == item.secimIlId
                                            group a by new { a.SecimDetayId, a.PartiId } into grp
                                            select new ElectionPartyResultModel
                                            {
                                                PartyId = grp.Key.PartiId,
                                                Vote = grp.Sum(i => i.OySayisi),
                                            }).ToListAsync();

                veri[3] = secimSonuclar2;

                foreach (var item2 in partiler2)
                {
                    item2.RatioAvg = secimSonuclar.Where(i => i.PartyId == item2.Id).Sum(i => i.Vote);
                }

                

                var ideologies = await (context.PoliticalIdeologies.Select(i => new PartyListModel
                {
                    Id = i.Id,
                    Name = i.NameTr,
                    PartyNameShort = i.NameTr,
                    RatioAvg = 0
                }).ToListAsync());

                foreach (var item2 in ideologies)
                {
                    foreach (var party in partiler2)
                    {
                        var oran = party.Ideoloies.Where(i => i.Id == item2.Id).Select(i => i.Weightiness).FirstOrDefault();

                        if (oran != null)
                        {
                            item2.RatioAvg += party.RatioAvg * oran / 100;
                        }
                    }
                    item2.RatioAvg = (double)Math.Round((decimal)item2.RatioAvg);
                }
                veri[4] = new { data = ideologies.OrderByDescending(i => i.RatioAvg).ToList() };

                genelVeri[i] = veri;

            }
            
            return Ok(genelVeri);
        }



        public class TumIllerResultModel
        {
            public int secimIlId { get; set; }
            public string cevreId { get; set; }
            public string secimturuId { get; set; }
            public int secimdetay { get; set; }
            public int klasikIlId { get; set; }
        }

        public class OncekiOranlar
        {
            public string partiAdi { get; set; }
            public string oran { get; set; }
        }

    }
}
