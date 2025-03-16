using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.GeometriesGraph;
using Siyasett.Core;
using Siyasett.Core.Addresses;
using Siyasett.Core.Attachments;
using Siyasett.Core.Emails;
using Siyasett.Core.Extensions;
using Siyasett.Core.Party;
using Siyasett.Core.People;
using Siyasett.Core.Phones;
using Siyasett.Core.Poll;
using Siyasett.Core.SocialMedia;
using Siyasett.Data.Data;
using Siyasett.Models;
using Siyasett.Web.Models;
using System.Security.AccessControl;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Siyasett.Web.Controllers
{
    public class PartiesController : Controller
    {
        protected readonly AppDbContext context;
        private readonly ILogger<PeopleController> _logger;

        public PartiesController(AppDbContext context, ILogger<PeopleController> logger)
        {
            this.context = context;
            _logger = logger;
        }


        [Route("siyasi-partiler")]
        [Route("political-parties")]
        public async Task<IActionResult> Index(int page = 1, int sort = -4, int pagesize = 30, string query = "", string name = "", string shortname = "", string leadername = "", int iselection = 0, int isactive = 0)
        {

            string[] orderFields = new string[] { "Name", "LeaderName", "Parliamenteries", "MemberCount", "Dof" };

            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;
            pager.SortFieldIndex = sort;
            pager.SearchKeyword = query;

            PartySearchModel searchModel = new PartySearchModel();
            searchModel.PartyName = name;
            searchModel.PartyShortName = shortname;
            searchModel.PartyLeader = leadername;
            searchModel.Enjoyable = iselection;
            searchModel.Active = isactive;

            var liste = await (from a in context.Parties
                               join l in context.People on a.LeaderPeopleId equals l.Id into l2
                               from leader in l2.DefaultIfEmpty()
                               let pc = context.PeoplePositions.Where(i => i.PartyId == a.Id && !i.EndDate.HasValue && i.InstitutionTypeId == 2 && i.PositionId == 59).Count()//milletvekili aktif
                               where (string.IsNullOrEmpty(name) || (a.Name.ToUpper()).Contains(Helpers.Normalize(name)))
                               && (string.IsNullOrEmpty(shortname) || (a.ShortName.ToUpper()).Contains(Helpers.Normalize(shortname)))
                               && (string.IsNullOrEmpty(leadername) || (a.LeaderPeople.FirstNameNormalize).Contains(Helpers.Normalize(leadername)) || (a.LeaderPeople.LastNameNormalize).Contains(Helpers.Normalize(leadername)))
                               && (searchModel.Enjoyable == 0 || (searchModel.Enjoyable == 1 && a.ParticipateElection == true) || (searchModel.Enjoyable == 2 && a.ParticipateElection == false))
                               && (searchModel.Active == 0 || (searchModel.Active == 1 && a.Active == true) || (searchModel.Active == 2 && a.Active == false))
                               select new PartyListModel
                               {
                                   LeaderPeopleId = a.LeaderPeopleId,
                                   Name = a.Name,
                                   NameEn=a.NameEn,
                                   PartyNameShort = a.ShortName,
                                   Id = a.Id,
                                   LeaderName = leader == null ? "" : leader.FirstName + " " + leader.LastName,
                                   Active = a.Active,
                                   Dof = a.Dof,
                                   ParticipateElection = a.ParticipateElection,
                                   WebSite = a.WebSite,
                                   MemberCount = a.MemberCount,
                                   Parliamenteries = pc,
                                   Logo = a.Logo,
                                   SocialMedias = a.PartySocialParties.Select(i => new SocialMediaModel
                                   {
                                       Id = i.SocialMediaId,
                                       ParentId = i.PartyId,
                                       SocialAddress = i.SocialMedia.SocialAddress,
                                       SocialTypeId = i.SocialMedia.SocialTypeId,
                                       SocialTypeNameEn = i.SocialMedia.SocialType.NameEn,
                                       SocialTypeNameTr = i.SocialMedia.SocialType.NameTr,
                                       Url = i.SocialMedia.SocialType.UrlPattern.Replace("{value}", i.SocialMedia.SocialAddress)
                                   }).ToList()
                               }
                            ).OrderByField(orderFields[Math.Abs(pager.SortFieldIndex) - 1], pager.SortFieldIndex > 0).AsNoTracking().Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).ToListAsync();

            pager.Total = await (from a in context.Parties
                                 where (string.IsNullOrEmpty(name) || a.Name.Contains(name))
                                 && (string.IsNullOrEmpty(shortname) || a.ShortName.Contains(shortname))
                                 && (string.IsNullOrEmpty(leadername) || (a.LeaderPeople.FirstNameNormalize).Contains(Helpers.Normalize(leadername)) || (a.LeaderPeople.LastNameNormalize).Contains(Helpers.Normalize(leadername)))
                                 && (searchModel.Enjoyable == 0 || (searchModel.Enjoyable == 1 && a.ParticipateElection == true) || (searchModel.Enjoyable == 2 && a.ParticipateElection == false))
                                 && (searchModel.Active == 0 || (searchModel.Active == 1 && a.Active == true) || (searchModel.Active == 2 && a.Active == false))
                                 select new { a.Id }).AsNoTracking().CountAsync();


            ViewBag.SearchModel = searchModel;
            ViewBag.Pager = pager;

            return View(liste);
        }


        [Route("siyasi-partiler/{id:int}/{adi}")]
        [Route("political-parties/{id:int}/{adi}")]
        public async Task<IActionResult> Detail(int id)
        {
            var model = await (from a in context.Parties

                               where a.Id == id
                               select new PartyModel
                               {
                                   Id = a.Id,
                                   Name = a.Name,
                                   ShortName = a.ShortName,
                                   LeaderName = a.LeaderPeople.FirstName + " " + a.LeaderPeople.LastName,
                                   Active = a.Active,
                                   LeaderPeopleId = a.LeaderPeopleId,
                                   Dof = a.Dof,
                                   Parliamenteries = a.Parliamenteries,
                                   MemberCount = a.MemberCount,
                                   ParticipateElection = a.ParticipateElection,
                                   Logo = a.Logo,
                                   WebSite = a.WebSite,
                                   UpdatedDate = a.UpdatedDate,
                                   CreatedDate = a.CreatedDate

                               }).FirstOrDefaultAsync();


            var culture=Thread.CurrentThread.CurrentCulture;
            var cultureName = culture.Name;


            int[] teskilatPositions = new int[] { 27, 28, 29, 30, 26, 25 };
            ViewBag.Teskilat = await (from a in context.People
                                      let position = (from i in context.PeoplePositions
                                                      join p in context.Provinces on i.ProvinceId equals p.Id into p1
                                                      from province in p1.DefaultIfEmpty()
                                                      join d in context.Districts on i.DistrictId equals d.Id into d1
                                                      from district in d1.DefaultIfEmpty()
                                                      orderby i.StartDate descending
                                                      where i.PeopleId == a.Id
                                                      select new
                                                      {
                                                          PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                                          PositionName = cultureName == "tr-TR" ? i.Position.NameTr : i.Position.NameEn,
                                                          Institution = i.InstitutionName,
                                                          PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                                          InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                                          PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                                          Period = i.Period,
                                                          StartDate = i.StartDate,
                                                          InstitutionType = i.InstitutionType.NameTr,
                                                          Weightiness = i.Position.Weightiness,
                                                          ProvinceName = province != null ? province.Name : "",
                                                          DistrictName = district != null ? district.Name : ""
                                                      }).FirstOrDefault()
                                      where a.PeoplePositions.Any(i => teskilatPositions.Contains(i.PositionId) && i.PartyId == id)
                                      orderby a.FirstName, a.LastName
                                      select new PeopleListModel
                                      {
                                          LastName = a.LastName,
                                          CreatedDate = a.CreatedDate,
                                          EducationId = a.EducationId,
                                          EducationName = cultureName=="tr-TR"? a.Education.NameTr:a.Education.NameEn,
                                          FirstName = a.FirstName,
                                          GenderId = a.GenderId,
                                          GenderName = a.Gender.NameTr,
                                          Id = a.Id,
                                          PlaceOfBirth = a.PlaceOfBirth,
                                          DateOfBirth = a.DateOfBirth,
                                          Photo = a.Photo,
                                          UpdatedDate = a.UpdatedDate,
                                          IsActive = a.IsActive,
                                          PartyName = position != null ? (String.IsNullOrEmpty(position.PartyName) ? position.PositionName : position.PartyName) : "",
                                          PositionName = position != null ? position.PositionName : "",
                                          InstitutionName = position != null ? position.Institution : "",
                                          Period = position != null ? position.Period : "",
                                          InstitutionTypeName = position != null ? position.InstitutionType : "",
                                          Views = a.Views,
                                          PositionWeight = position != null ? position.Weightiness : 0,
                                          ProvinceName = position != null ? position.ProvinceName : "",
                                          DistrictName = position != null ? position.DistrictName : ""

                                      }).OrderBy(k => k.PositionWeight).ThenBy(i => i.FirstName).ThenBy(i => i.LastName).Take(30).ToListAsync();

            int[] ustduzeyPositions = new int[] { 16, 17, 67, 68, 18, 19, 60, 71, 74, 76, 77 };

            ViewBag.UstDuzey = await (from a in context.People
                                      let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id).Select(i => new
                                      {
                                          PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                          PositionName = cultureName == "tr-TR"? i.Position.NameTr:i.Position.NameEn,
                                          Institution = i.InstitutionName,
                                          PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                          InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                          PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                          Period = i.Period,
                                          StartDate = i.StartDate,
                                          InstitutionType = cultureName == "tr-TR" ? i.InstitutionType.NameTr: i.InstitutionType.NameEn,
                                          Weight = i.Position.Weightiness,
                                          possector = i.SectorId,
                                          positionSectorName = cultureName == "tr-TR" ? i.Sector.NameTr:i.Sector.NameEn
                                      }).OrderByDescending(f => f.StartDate).FirstOrDefault()
                                      where a.PeoplePositions.Any(i => ustduzeyPositions.Contains(i.PositionId) && i.PartyId == id && !i.EndDate.HasValue)
                                      select new PeopleListModel
                                      {
                                          LastName = a.LastName,
                                          CreatedDate = a.CreatedDate,
                                          EducationId = a.EducationId,
                                          EducationName = cultureName == "tr-TR" ? a.Education.NameTr : a.Education.NameEn,
                                          FirstName = a.FirstName,
                                          GenderId = a.GenderId,
                                          GenderName = cultureName == "tr-TR"? a.Gender.NameTr:a.Gender.NameEn,
                                          Id = a.Id,
                                          PlaceOfBirth = a.PlaceOfBirth,
                                          DateOfBirth = a.DateOfBirth,
                                          Photo = a.Photo,
                                          Name = a.FirstName + " " + a.LastName,
                                          UpdatedDate = a.UpdatedDate,
                                          IsActive = a.IsActive,
                                          PartyName = position != null ? (String.IsNullOrEmpty(position.PartyName) ? position.PositionName : position.PartyName) : "",
                                          PositionName = position != null ? position.PositionName : "",
                                          InstitutionName = position != null ? position.Institution : "",
                                          Period = position != null ? position.Period : "",
                                          InstitutionTypeName = position != null ? position.InstitutionType : "",
                                          Views = a.Views,
                                          Weight = position != null ? position.Weight.HasValue ? position.Weight.Value : 1 : 1,
                                          PositionSector = position != null? position.possector:0,
                                          PositionSectorName=position!=null? position.positionSectorName:""

                                      }).OrderBy(i => i.Weight).ThenBy(i => i.FirstName).ThenBy(i => i.LastName).ToListAsync();



            ViewBag.Secilmis = await (from a in context.People
                                      let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id).Select(i => new
                                      {
                                          PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                          PositionName = cultureName == "tr-TR"? i.Position.NameTr:i.Position.NameEn,
                                          Institution = i.InstitutionName,
                                          PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                          InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                          PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                          Period = i.Period,
                                          StartDate = i.StartDate,
                                          InstitutionType = cultureName == "tr-TR"? i.InstitutionType.NameTr:i.InstitutionType.NameEn,
                                          Weightiness = context.Positions.FirstOrDefault(p => p.Id == i.PositionId).Weightiness,
                                      }).OrderByDescending(f => f.StartDate).FirstOrDefault()
                                      where a.PeoplePositions.Any(i => (i.InstitutionTypeId == 2 || i.InstitutionTypeId == 6) && i.PartyId == id && !i.EndDate.HasValue)
                                      orderby a.FirstName, a.LastName
                                      select new PeopleListModel
                                      {
                                          LastName = a.LastName,
                                          CreatedDate = a.CreatedDate,
                                          EducationId = a.EducationId,
                                          EducationName = cultureName == "tr-TR"? a.Education.NameTr:a.Education.NameEn,
                                          FirstName = a.FirstName,
                                          GenderId = a.GenderId,
                                          GenderName = cultureName == "tr-TR" ? a.Gender.NameTr : a.Gender.NameEn,
                                          Id = a.Id,
                                          PlaceOfBirth = a.PlaceOfBirth,
                                          DateOfBirth = a.DateOfBirth,
                                          Photo = a.Photo,
                                          UpdatedDate = a.UpdatedDate,
                                          IsActive = a.IsActive,
                                          PartyName = position != null ? (String.IsNullOrEmpty(position.PartyName) ? position.PositionName : position.PartyName) : "",
                                          PositionName = position != null ? position.PositionName : "",
                                          InstitutionName = position != null ? position.Institution : "",
                                          Period = position != null ? position.Period : "",
                                          InstitutionTypeName = position != null ? position.InstitutionType : "",
                                          Views = a.Views,
                                          PositionWeight = position != null ? position.Weightiness : 0

                                      }).OrderBy(k => k.PositionWeight).Take(30).ToListAsync();


            PagerInfo pagerSecilmis = new PagerInfo();
            pagerSecilmis.PageSize = 30;
            pagerSecilmis.CurrentPage = 1;

            pagerSecilmis.Total = await context.People.Where(a => a.PeoplePositions.Any(i => (i.InstitutionTypeId == 2 || i.InstitutionTypeId == 6) && i.PartyId == id && !i.EndDate.HasValue)).CountAsync();
            ViewBag.PagerSecilmis = pagerSecilmis;

            var programSectors = context.PartyTexts.Where(a => a.Partyid == id)
                .Select(i => new Sector
                {
                    Id=i.SectorId,
                    NameTr= cultureName=="tr-TR"? i.Sector.NameTr:i.Sector.NameEn,
                    Weightiness=i.Sector.Weightiness
                }).OrderBy(i=>i.Weightiness).ToList();

            var partyTexts = context.PartyTexts.Where(a => a.Partyid == id ).ToList();

            ViewBag.ProgramSectors = programSectors;
            ViewBag.PartyTexts = partyTexts;


            ViewBag.Phones = await (from a in context.PartyPhones
                                    where a.PartyId == id
                                    select new PhoneModel
                                    {
                                        PhoneTypeNameTr = cultureName == "tr-TR"? a.Phone.PhoneType.NameTr:a.Phone.PhoneType.NameEn,
                                        CommunicationTypeId = a.Phone.CommunicationTypeId,
                                        CommunicationTypeNameTr = cultureName == "tr-TR" ? a.Phone.CommunicationType.NameTr : a.Phone.CommunicationType.NameEn,
                                        PhoneId = a.Phone.Id,
                                        Id = a.Id,
                                        PhoneTypeId = a.Phone.PhoneTypeId,
                                        CountryCode = a.Phone.CountryCode,
                                        PrefixCode = a.Phone.PrefixCode,
                                        PhoneNumber = a.Phone.PhoneNumber,

                                    }).ToListAsync();

            ViewBag.Emails = await context.PartyEmails.Where(a => a.PartyId == id)
                .Select(i => new EmailModel
                {
                    CommunicationTypeId = i.Email.CommunicationTypeId,
                    CommunicationTypeNameTr = cultureName == "tr-TR" ? i.Email.CommunicationType.NameTr : i.Email.CommunicationType.NameEn,
                    Id = i.EmailId,
                    ParentId = i.PartyId,
                    EmailAddress = i.Email.EmailAddress,

                }).ToListAsync();



            ViewBag.SocialAdress = context.PartySocialParties.Where(a => a.PartyId == id)
                .Select(i => new SocialMediaModel
                {
                    Id = i.SocialMediaId,
                    ParentId = i.PartyId,
                    SocialAddress = i.SocialMedia.SocialAddress,
                    SocialTypeId = i.SocialMedia.SocialTypeId,
                    SocialTypeNameEn = i.SocialMedia.SocialType.NameEn,
                    SocialTypeNameTr = i.SocialMedia.SocialType.NameTr,
                    Url = i.SocialMedia.SocialType.UrlPattern.Replace("{value}", i.SocialMedia.SocialAddress)

                }).ToList();

            ViewBag.Attachments = await context.PartyAttachments.Where(a => a.PartyId == id)
                .Select(i => new AttachmentModel
                {
                    OriginalFileName = i.Attachment.OriginalFileName,
                    FileName = i.Attachment.FileName,
                    AttachmentId = i.AttachmentId,
                    ContentType = i.Attachment.ContentType,
                    CreatedDate = i.Attachment.CreatedDate,
                    FileSize = i.Attachment.FileSize,
                    Id = i.Id,
                    NameEn = i.Attachment.NameEn,
                    NameTr = i.Attachment.NameTr,
                    ParentId = i.PartyId

                }).ToListAsync();

            ViewBag.Addresses = await (from a in context.PartyAddresses
                                       where a.PartyId == id
                                       select new AddressModel
                                       {
                                           Address1 = a.Address.Address1,
                                           Address2 = a.Address.Address2,
                                           AddressTypeId = a.Address.AddressTypeId,
                                           AddressTypeNameEn = a.Address.AddressType.NameEn,
                                           AddressTypeNameTr = a.Address.AddressType.NameTr,
                                           District = a.Address.District,
                                           Id = a.Address.Id,
                                           CountryId = a.Address.CountryId,
                                           CountryNameTr = a.Address.Country.NameTr,
                                           CreatedDate = a.Address.CreatedDate,
                                           NameEn = a.Address.NameEn,
                                           NameTr = a.Address.NameTr,
                                           PostalCode = a.Address.PostalCode,
                                           Province = a.Address.Province,
                                           UpdatedDate = a.Address.UpdatedDate,
                                           DistrictId = a.Address.DistrictId,
                                           ProvinceId = a.Address.ProvinceId
                                       }).ToListAsync();





            return View(model);
        }

        public async Task<IActionResult> GetSecimler(int id)
        {
            var partiSecimler = await (from a in context.SecimPartilers
                                       where a.PartiId == id
                                       orderby a.SecimDetay.Secim.SecimTarihi
                                       select new PartyElectionModel
                                       {
                                           SecimTarihi = a.SecimDetay.Secim.SecimTarihi.Value,
                                           Adi = a.SecimDetay.Secim.Adi,
                                           PartyId = id,
                                           SecimId = a.SecimDetay.SecimId.Value,
                                           SecimTuruAdi = a.SecimDetay.SecimTuru.Adi,
                                           YskSecimId = a.SecimDetay.YskSecimId.Value,
                                           AldigiOySayisi = a.YurtIciToplamOy.Value + a.YurtDisiToplamOy.Value,
                                           ToplamOySayisi = a.SecimDetay.GecerliOy.Value,

                                       }).ToListAsync();
            return Ok(partiSecimler);
        }


        public async Task<IActionResult> Detail2(int id, int katsayi, string tablo)
        {

            if (tablo.Contains("ustDuzey"))
            {

                int[] ustduzeyPositions = new int[] { 16, 17, 67, 68, 18, 19, 60, 71, 74, 76, 77 };

                var sonuc = await (from a in context.People
                                   let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id).Select(i => new
                                   {
                                       PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                       PositionName =  i.Position.NameTr,
                                       Institution = i.InstitutionName,
                                       PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                       InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                       PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                       Period = i.Period,
                                       StartDate = i.StartDate,
                                       InstitutionType = i.InstitutionType.NameTr,
                                       Weight = i.Position.Weightiness,
                                       possector = context.PeoplePositionSectors.FirstOrDefault(f => f.PeoplePositionId == i.Id).SectorId
                                   }).OrderByDescending(f => f.StartDate).FirstOrDefault()
                                   where a.PeoplePositions.Any(i => ustduzeyPositions.Contains(i.PositionId) && i.PartyId == id && !i.EndDate.HasValue)
                                   select new PeopleListModel
                                   {
                                       LastName = a.LastName,
                                       CreatedDate = a.CreatedDate,
                                       EducationId = a.EducationId,
                                       EducationName = a.Education.NameTr,
                                       FirstName = a.FirstName,
                                       GenderId = a.GenderId,
                                       GenderName = a.Gender.NameTr,
                                       Id = a.Id,
                                       PlaceOfBirth = a.PlaceOfBirth,
                                       DateOfBirth = a.DateOfBirth,
                                       Photo = Helpers.GetPersonPhoto(a.Photo),
                                       Name = a.FirstName + " " + a.LastName,
                                       UpdatedDate = a.UpdatedDate,
                                       IsActive = a.IsActive,
                                       PartyName = position != null ? (String.IsNullOrEmpty(position.PartyName) ? position.PositionName : position.PartyName) : "",
                                       PositionName = position != null ? position.PositionName : "",
                                       InstitutionName = position != null ? position.Institution : "",
                                       Period = position != null ? position.Period : "",
                                       InstitutionTypeName = position != null ? position.InstitutionType : "",
                                       Views = a.Views,
                                       Weight = position != null ? position.Weight.HasValue ? position.Weight.Value : 1 : 1,
                                       PositionSector = position.possector

                                   }).OrderBy(i => i.Weight).ThenBy(i => i.FirstName).ThenBy(i => i.LastName).Take(30 * katsayi).ToListAsync();



                return Ok(sonuc);
            }
            else if (tablo.Contains("teskilat"))
            {

                int[] teskilatPositions = new int[] { 27, 28, 29, 30, 26, 25 };
                var sonuc = await (from a in context.People
                                   let position = (from i in context.PeoplePositions
                                                   join p in context.Provinces on i.ProvinceId equals p.Id into p1
                                                   from province in p1.DefaultIfEmpty()
                                                   join d in context.Districts on i.DistrictId equals d.Id into d1
                                                   from district in d1.DefaultIfEmpty()
                                                   orderby i.StartDate descending
                                                   where i.PeopleId == a.Id
                                                   select new
                                                   {
                                                       PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                                       PositionName = i.Position.NameTr,
                                                       Institution = i.InstitutionName,
                                                       PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                                       InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                                       PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                                       Period = i.Period,
                                                       StartDate = i.StartDate,
                                                       InstitutionType = i.InstitutionType.NameTr,
                                                       Weightiness = i.Position.Weightiness,
                                                       ProvinceName = province != null ? province.Name : "",
                                                       DistrictName = district != null ? district.Name : ""
                                                   }).FirstOrDefault()
                                   where a.PeoplePositions.Any(i => teskilatPositions.Contains(i.PositionId) && i.PartyId == id)
                                   orderby a.FirstName, a.LastName
                                   select new PeopleListModel
                                   {
                                       LastName = a.LastName,
                                       CreatedDate = a.CreatedDate,
                                       EducationId = a.EducationId,
                                       EducationName = a.Education.NameTr,
                                       FirstName = a.FirstName,
                                       GenderId = a.GenderId,
                                       GenderName = a.Gender.NameTr,
                                       Id = a.Id,
                                       PlaceOfBirth = a.PlaceOfBirth,
                                       DateOfBirth = a.DateOfBirth,
                                       Photo = Helpers.GetPersonPhoto(a.Photo),
                                       UpdatedDate = a.UpdatedDate,
                                       IsActive = a.IsActive,
                                       PartyName = position != null ? (String.IsNullOrEmpty(position.PartyName) ? position.PositionName : position.PartyName) : "",
                                       PositionName = position != null ? position.PositionName : "",
                                       InstitutionName = position != null ? position.Institution : "",
                                       Period = position != null ? position.Period : "",
                                       InstitutionTypeName = position != null ? position.InstitutionType : "",
                                       Views = a.Views,
                                       PositionWeight = position != null ? position.Weightiness : 0,
                                       ProvinceName = position != null ? position.ProvinceName : "",
                                       DistrictName = position != null ? position.DistrictName : ""

                                   }).OrderBy(k => k.PositionWeight).ThenBy(i => i.FirstName).ThenBy(i => i.LastName).Take(30 * katsayi).ToListAsync();


                return Ok(sonuc);
            }
            else if (tablo.Contains("secilmis"))
            {

                var sonuc = await (from a in context.People
                                   let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id).Select(i => new
                                   {
                                       PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                       PositionName = i.Position.NameTr,
                                       Institution = i.InstitutionName,
                                       PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                       InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                       PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                       Period = i.Period,
                                       StartDate = i.StartDate,
                                       InstitutionType = i.InstitutionType.NameTr,
                                       Weightiness = context.Positions.FirstOrDefault(p => p.Id == i.PositionId).Weightiness,
                                   }).OrderByDescending(f => f.StartDate).FirstOrDefault()
                                   where a.PeoplePositions.Any(i => (i.InstitutionTypeId == 2 || i.InstitutionTypeId == 6) && i.PartyId == id && !i.EndDate.HasValue)
                                   orderby a.FirstName, a.LastName
                                   select new PeopleListModel
                                   {
                                       LastName = a.LastName,
                                       CreatedDate = a.CreatedDate,
                                       EducationId = a.EducationId,
                                       EducationName = a.Education.NameTr,
                                       FirstName = a.FirstName,
                                       GenderId = a.GenderId,
                                       GenderName = a.Gender.NameTr,
                                       Id = a.Id,
                                       PlaceOfBirth = a.PlaceOfBirth,
                                       DateOfBirth = a.DateOfBirth,
                                       Photo = Helpers.GetPersonPhoto(a.Photo),
                                       UpdatedDate = a.UpdatedDate,
                                       IsActive = a.IsActive,
                                       PartyName = position != null ? (String.IsNullOrEmpty(position.PartyName) ? position.PositionName : position.PartyName) : "",
                                       PositionName = position != null ? position.PositionName : "",
                                       InstitutionName = position != null ? position.Institution : "",
                                       Period = position != null ? position.Period : "",
                                       InstitutionTypeName = position != null ? position.InstitutionType : "",
                                       Views = a.Views,
                                       PositionWeight = position != null ? position.Weightiness : 0

                                   }).OrderBy(k => k.PositionWeight).Take(30 * katsayi).ToListAsync();
                return Ok(sonuc);
            }
            else
            {
                return Ok();
            }


        }

        public async Task<IActionResult> GetPartyText(int sectorid, int partyid)
        {
            var list = context.PartyTextsSectors.Where(a => a.Sectorid == sectorid).ToList();
            List<string> textlist = new();
            foreach (var item in list)
            {
                var text = context.PartyTexts.Where(a => a.Id == item.PartyTextid && a.Partyid == partyid).ToList();
                foreach (var te in text)
                {
                    textlist.Add(te.Text);
                }
            }


            return Ok(textlist);
        }

        public async Task<IActionResult> GetFeedBack(FeedBackModel obj)
        {
            var feedback = new Feedback();
            feedback.Name=obj.Name;
            feedback.Lastname = obj.LastName;
            feedback.Email=obj.Email;
            feedback.Topic = obj.Topic;
            feedback.Message = obj.Text;
            feedback.PageLink = obj.PageUrl;
            feedback.SendTime = DateTime.Now;
            feedback.Title = obj.Title;
            context.Feedbacks.Add(feedback);
            context.SaveChanges();

            return Ok();
        }

        public async Task<IActionResult> SaveFeedbackEdit(FeedBackModel obj)
        {
            var feedback = await context.Feedbacks.FirstOrDefaultAsync(a => a.Id == obj.Id);
            feedback.Answer = obj.Answer;
            feedback.AnswerTime = DateTime.Now;
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
