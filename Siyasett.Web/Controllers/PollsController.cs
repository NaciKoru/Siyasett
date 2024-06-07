using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siyasett.Core.Party;
using Siyasett.Core.Poll;
using Siyasett.Data.Data;
using System.Globalization;

namespace Siyasett.Web.Controllers
{
    public class PollsController : Controller
    {

        protected readonly AppDbContext context;
        private readonly ILogger<PeopleController> _logger;

        public PollsController(AppDbContext context, ILogger<PeopleController> logger)
        {
            this.context = context;
            _logger = logger;
        }



        [Route("anketler")]
        [Route("polls")]
        public async Task<IActionResult> Index()
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;

            var partiler = await (from a in context.PollResults
                                  join b in context.Parties on a.PartyId equals b.Id
                                  where a.Poll.Active
                                  group a by new { a.PartyId, b.ShortName, b.Name } into grp
                                  
                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = grp.Key.PartyId,
                                      PartyNameShort = grp.Key.ShortName,
                                      RatioAvg = grp.Key.Name == "Kararsızlar" ? -1 : grp.Key.Name == "Diğer" ? -2 : grp.Average(i => i.Ratio)
                                  }).OrderByDescending(i => i.RatioAvg).ToListAsync();
            ViewBag.Parties = partiler;
            var tarihler = await (from a in context.Polls
                                  where a.Active
                                  group a by new { a.Year, a.Month } into grp
                                  select new PollDateModel
                                  {
                                      Year = grp.Key.Year,
                                      Month = grp.Key.Month,
                                      MonthName = cultureInfo.DateTimeFormat.GetMonthName(grp.Key.Month),

                                  }
                          ).OrderByDescending(i=>i.Year).ThenByDescending(i=>i.Month).ToListAsync();

            tarihler.ForEach((i) => i.YearMonth = Convert.ToInt32(i.Year.ToString() + i.Month.ToString("000")));

            ViewBag.Dates = tarihler;
            var polls = await (from a in context.PollResults
                               where a.Poll.Active && a.Active
                               select new PollResultListModel
                               {

                                   PartyId = a.PartyId,
                                   Ratio = a.Ratio,
                                   PollId = a.PollId,
                                   Id = a.Id,
                                   Month = a.Poll.Month,
                                   Year = a.Poll.Year,
                                   CompanyId=a.Poll.PollCompanyId,
                               }).ToListAsync();
            ViewBag.Companies = await context.Companies.Where(i => i.Polls.Any()).OrderBy(i => i.Name).AsNoTracking().ToListAsync();
            return View(polls);
        }


        [Route("anketler-grafik")]
        [Route("polls-charts")]
        public async Task<IActionResult> Grafik()
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;

            var partiler = await (from a in context.PollResults
                                  join b in context.Parties on a.PartyId equals b.Id
                                  group a by new { a.PartyId, b.ShortName, b.Name } into grp

                                  select new PartyListModel
                                  {
                                      Name = grp.Key.Name,
                                      Id = grp.Key.PartyId,
                                      PartyNameShort = grp.Key.ShortName
                                  }).OrderBy(i => i.Name).ToListAsync();
            ViewBag.Parties = partiler;
            var tarihler = await (from a in context.Polls
                                  group a by new { a.Year, a.Month } into grp
                                  select new PollDateModel
                                  {
                                      Year = grp.Key.Year,
                                      Month = grp.Key.Month,
                                      MonthName = cultureInfo.DateTimeFormat.GetMonthName(grp.Key.Month)
                                  }
                          ).OrderByDescending(i => i.Year).ThenByDescending(i => i.Month).ToListAsync();
            ViewBag.Dates = tarihler;
            ViewBag.Companies = await context.Companies.Where(i => i.Polls.Any()).OrderBy(i => i.Name).AsNoTracking().ToListAsync();
            return View();
        }


        public async Task<IActionResult> GetData(int t, string[] dates, int[] parties, int[] companies)
        {
            string[] date;

            switch (t)
            {
                case 1: //tarihe göre
                    date = dates[0].Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

                    int month = Convert.ToInt32(date.Last());
                    int year = Convert.ToInt32(date.First());
                    var polls = await (from a in context.Polls
                                       where a.Year == year && a.Month == month && companies.Contains(a.PollCompanyId)
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
                    return Ok(polls);


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
                                        orderby a.Year,a.Month
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


    }
}
