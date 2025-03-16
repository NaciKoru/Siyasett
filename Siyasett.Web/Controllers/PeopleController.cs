using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.GeometriesGraph;
using NetTopologySuite.Index.HPRtree;
using Siyasett.Core;
using Siyasett.Core.Abstracts;
using Siyasett.Core.Addresses;
using Siyasett.Core.Emails;
using Siyasett.Core.Extensions;
using Siyasett.Core.Party;
using Siyasett.Core.People;
using Siyasett.Core.Phones;
using Siyasett.Core.SocialMedia;
using Siyasett.Data.Data;
using Siyasett.Models;
using Siyasett.Web.Models;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using Position = Siyasett.Data.Data.Position;

namespace Siyasett.Web.Controllers
{
    public class PeopleController : Controller
    {
        protected readonly AppDbContext context;
        private readonly ILogger<PeopleController> _logger;

        public PeopleController(AppDbContext context, ILogger<PeopleController> logger)
        {
            this.context = context;
            _logger = logger;
        }

        [Route("kim-kimdir")]
        [Route("who-is")]
        public async Task<IActionResult> Index(int page = 1, byte sort = 1, byte desc = 0, int pagesize = 30, string firstName = "", string lastName = "", byte gender = 0, int education = 0, int partyId = 0, int instituiton = 0, int positionId = 0, string period = "", int isactivepolitics = 1, int sector = 0, int province = 0, int district = 0, int job = 0, string birthplace = "", int agegap = 0, int isalive = 0)
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;
            string[] orderFields = new string[] { "Views", "FirstName", "LastName", "InstitutionName", "PartyName", "PositionName", "Province", "GenderId", "EducationId" };
            pager.SetQuery(Request.Query);
            PeopleSearchModel searchModel = new PeopleSearchModel();
            searchModel.FirstName = firstName;
            searchModel.LastName = lastName;
            searchModel.GenderId = gender;
            searchModel.EducationId = education;
            searchModel.PartyId = partyId;
            searchModel.InstitutionTypeId = instituiton;
            searchModel.PositionId = positionId;
            //searchModel.OrderBy = sort;
            searchModel.OrderByDesc = desc;
            searchModel.Period = period;
            searchModel.IsActivePolitics = isactivepolitics;
            searchModel.Sectors = sector;
            searchModel.Province = province;
            searchModel.District = district;
            searchModel.Job = job;
            searchModel.Birtplace = birthplace;
            searchModel.Agegap = agegap;
            searchModel.IsAlive = isalive;
            pager.SortFieldIndex = sort;

            var minage = 0;
            var maxage = 150;

            if (agegap == 1)
            {
                minage = 16;
                maxage = 21;
            }
            else if (agegap == 2)
            {
                minage = 22;
                maxage = 35;
            }
            else if (agegap == 3)
            {
                minage = 36;
                maxage = 45;
            }
            else if (agegap == 4)
            {
                minage = 46;
                maxage = 55;
            }
            else if (agegap == 5)
            {
                minage = 56;
                maxage = 64;
            }
            else if (agegap == 6)
            {
                minage = 65;
                maxage = 150;
            }
            else if (agegap == 7)
            {
                minage = 1;
            }


            var liste = await (from a in context.People
                               let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id && ((!i.EndDate.HasValue && isactivepolitics == 1) || (isactivepolitics == 2)) && ((!string.IsNullOrEmpty(period) && i.Period == period) || string.IsNullOrEmpty(period)) && (searchModel.PartyId == 0 || (i.PartyId == searchModel.PartyId && searchModel.PartyId != 0)) && (searchModel.PositionId == 0 || (searchModel.PositionId == i.PositionId && searchModel.PositionId != 0)) && (searchModel.InstitutionTypeId == 0 || (searchModel.InstitutionTypeId == i.InstitutionTypeId && searchModel.InstitutionTypeId != 0)) && ((searchModel.Province != 0 && i.ProvinceId == searchModel.Province) || searchModel.Province == 0) && (searchModel.Sectors == 0 || (searchModel.Sectors == i.SectorId && searchModel.Sectors != 0)) && (searchModel.District == 0 || (searchModel.District == i.DistrictId && searchModel.District != 0))).Select(i => new
                               {
                                   PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                   PartyNameEn = i.PartyId.HasValue ? i.Party.NameEn : "",
                                   PositionName = i.Position.NameTr,
                                   PositionNameEn = i.Position.NameEn,
                                   PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                   InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                   PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                   Period = i.Period,
                                   StartDate = i.StartDate,
                                   InstitutionType = i.InstitutionType.NameTr,
                                   InstitutionTypeEn = i.InstitutionType.NameEn,
                                   Institution = i.InstitutionType.NameTr,
                                   InstitutionEn = i.InstitutionType.NameEn,
                                   PosProvinceId = i.ProvinceId,
                                   PosDistrictId = i.DistrictId,
                                   PositionId = i.Id,
                                   ProvinceName = context.Provinces.FirstOrDefault(d => d.Id == i.ProvinceId).Name,
                                   SectorId = i.SectorId,
                                   SectorName = i.Sector == null ? "" : i.Sector.NameTr,
                                   SectorNameEn = i.Sector == null ? "" : i.Sector.NameEn,

                               }).OrderByDescending(f => f.StartDate).FirstOrDefault()

                               where (string.IsNullOrEmpty(firstName) || (a.FirstNameNormalize).Contains(Helpers.Normalize(firstName)))
                               && (string.IsNullOrEmpty(lastName) || (a.LastNameNormalize).Contains(Helpers.Normalize(lastName)))
                               && (searchModel.GenderId == 0 || (searchModel.GenderId == a.GenderId))
                               && (searchModel.EducationId == 0 || (searchModel.EducationId == a.EducationId))
                                && ((position != null))
                                && (searchModel.Job == 0 || (context.PeopleJobs.Where(c => c.PeopleId == a.Id).Count() > 0 && context.PeopleJobs.Where(d => d.PeopleId == a.Id).FirstOrDefault(e => e.JobId == searchModel.Job) != null))
                                && (string.IsNullOrEmpty(searchModel.Birtplace) || (a.PlaceOfBirth.ToUpper().Contains(birthplace.ToUpper())))
                                && (searchModel.Agegap == 0 || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) >= minage && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) <= maxage && minage != 1) || (minage == 1 && a.DateOfBirth == null))
                                && ((searchModel.IsAlive == 0 && a.PositionFieldId == 1) || ((searchModel.IsAlive == 1 && a.IsActive == true && a.PositionFieldId == 1) || (searchModel.IsAlive == 2 && a.IsActive == false && a.PositionFieldId==1)))
                               select new PeopleListModel
                               {
                                   LastName = a.LastName,
                                   CreatedDate = a.CreatedDate,
                                   EducationId = a.EducationId,
                                   EducationName = a.Education.NameTr,
                                   EducationNameEn = a.Education.NameEn,
                                   FirstName = a.FirstName,
                                   GenderId = a.GenderId,
                                   GenderName = a.Gender.NameTr,
                                   GenderNameEn = a.Gender.NameEn,
                                   Id = a.Id,
                                   PlaceOfBirth = a.PlaceOfBirth,
                                   DateOfBirth = a.DateOfBirth,
                                   Photo = a.Photo,
                                   UpdatedDate = a.UpdatedDate,
                                   IsActive = a.IsActive,
                                   PartyName = position != null ? (String.IsNullOrEmpty(position.PartyName) ? position.PositionName : position.PartyName) : "",
                                   PartyNameEn = position != null ? (String.IsNullOrEmpty(position.PartyNameEn) ? position.PositionNameEn : position.PartyNameEn) : "",
                                   PositionName = position != null ? position.PositionName : "",
                                   PositionNameEn = position != null ? position.PositionNameEn : "",
                                   InstitutionName = position != null ? position.Institution : "",
                                   InstitutionNameEn = position != null ? position.InstitutionEn : "",
                                   Period = position != null ? position.Period : "",
                                   InstitutionTypeName = position != null ? position.InstitutionType : "",
                                   InstitutionTypeNameEn = position != null ? position.InstitutionTypeEn : "",
                                   Views = a.Views,
                                   ProvinceName = position != null ? position.ProvinceName : "",
                                   SectorId = position.SectorId != null ? position.SectorId : 0,
                                   PhotoOrder = string.IsNullOrEmpty(a.Photo) ? 0 : 1,
                                   //SocialMedias = a.PeopleSocialMedia.Select(i => new SocialMediaModel
                                   //{
                                   //    Id = i.SocialMediaId,
                                   //    ParentId = i.PeopleId,
                                   //    SocialAddress = i.SocialMedia.SocialAddress,
                                   //    SocialTypeId = i.SocialMedia.SocialTypeId,
                                   //    SocialTypeNameEn = i.SocialMedia.SocialType.NameEn,
                                   //    SocialTypeNameTr = i.SocialMedia.SocialType.NameTr,
                                   //    Url = i.SocialMedia.SocialType.UrlPattern.Replace("{value}", i.SocialMedia.SocialAddress)
                                   //}).ToList(),
                                   PositionId = position.PositionId != null ? position.PositionId : 0,
                               }
                       ).OrderByDescending(a => a.PhotoOrder).ThenBy(a => Guid.NewGuid()).Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).AsNoTracking().ToListAsync();




            pager.Total = await (from a in context.People
                                 let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id && ((!i.EndDate.HasValue && isactivepolitics == 1) || (isactivepolitics == 2)) && ((!string.IsNullOrEmpty(period) && i.Period == period) || string.IsNullOrEmpty(period)) && (searchModel.PartyId == 0 || (i.PartyId == searchModel.PartyId && searchModel.PartyId != 0)) && (searchModel.PositionId == 0 || (searchModel.PositionId == i.PositionId && searchModel.PositionId != 0)) && (searchModel.InstitutionTypeId == 0 || (searchModel.InstitutionTypeId == i.InstitutionTypeId && searchModel.InstitutionTypeId != 0)) && ((searchModel.Province != 0 && i.ProvinceId == searchModel.Province) || searchModel.Province == 0) && (searchModel.Sectors == 0 || (searchModel.Sectors == i.SectorId && searchModel.Sectors != 0)) && (searchModel.District == 0 || (searchModel.District == i.DistrictId && searchModel.District != 0))).Select(i => new
                                 {
                                     PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                     PositionName = i.Position.NameTr,
                                     PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                     InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                     PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                     Period = i.Period,
                                     StartDate = i.StartDate,
                                     InstitutionType = i.InstitutionType.NameTr,
                                     Institution = i.InstitutionType.NameTr,
                                     PosProvinceId = i.ProvinceId,
                                     PosDistrictId = i.DistrictId,
                                     PositionId = i.Id,
                                     ProvinceName = context.Provinces.FirstOrDefault(d => d.Id == i.ProvinceId).Name,
                                     SectorId = i.SectorId,
                                     SectorName = i.Sector == null ? "" : i.Sector.NameTr,
                                 }).OrderByDescending(f => f.StartDate).FirstOrDefault()
                                 where (string.IsNullOrEmpty(firstName) || (a.FirstNameNormalize).Contains(Helpers.Normalize(firstName)))
                                 && (string.IsNullOrEmpty(lastName) || (a.LastNameNormalize).Contains(Helpers.Normalize(lastName)))
                                 && (searchModel.GenderId == 0 || (searchModel.GenderId == a.GenderId))
                                 && (searchModel.EducationId == 0 || (searchModel.EducationId == a.EducationId))
                                  && ((position != null))
                                  && (searchModel.Job == 0 || (context.PeopleJobs.Where(c => c.PeopleId == a.Id).Count() > 0 && context.PeopleJobs.Where(d => d.PeopleId == a.Id).FirstOrDefault(e => e.JobId == searchModel.Job) != null))
                                  && (string.IsNullOrEmpty(searchModel.Birtplace) || (a.PlaceOfBirth.ToUpper().Contains(birthplace.ToUpper())))
                                  && (searchModel.Agegap == 0 || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) >= minage && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) <= maxage && minage != 1) || (minage == 1 && a.DateOfBirth == null))
                                  && (searchModel.IsAlive == 0 || ((searchModel.IsAlive == 1 && a.IsActive == true) || (searchModel.IsAlive == 2 && a.IsActive == false && a.PositionFieldId == 1)))
                                 select new { a.Id }).AsNoTracking().CountAsync();


            ViewBag.Educations = await context.Educations.Select(i => new Education { Id = i.Id, NameTr = i.NameTr, NameEn = i.NameEn }).OrderBy(i => i.Id).AsNoTracking().ToListAsync();

            ViewBag.SearchModel = searchModel;
            ViewBag.Pager = pager;

            ViewBag.InstitutionTypes = await context.InstitutionTypes.Select(i => new InstitutionType { Id = i.Id, NameTr = i.NameTr, NameEn = i.NameEn }).AsNoTracking().ToListAsync();


            ViewBag.Positions = await context.Positions.Select(i => new Position { Id = i.Id, NameTr = i.NameTr, NameEn = i.NameEn, Weightiness = i.Weightiness }).OrderBy(i => i.Weightiness).AsNoTracking().ToListAsync();

            ViewBag.Parties = await (from a in context.Parties
                                     where a.Active
                                     orderby a.ShortName
                                     select new PartyListModel
                                     {
                                         Id = a.Id,
                                         PartyName = a.ShortName,
                                         NameEn = a.NameEn,

                                     }).ToListAsync();

            ViewBag.Sectors = await context.Sectors.Select(i => new Sector { Id = i.Id, NameTr = i.NameTr, Weightiness = i.Weightiness, NameEn = i.NameEn }).OrderBy(a => a.Weightiness).AsNoTracking().ToListAsync();
            ViewBag.Provinces = await context.Provinces.Select(i => new Province { Id = i.Id, Name = i.Name }).OrderBy(a => a.Name).AsNoTracking().ToListAsync();
            ViewBag.District = await context.Districts.Select(i => new District { Id = i.Id, Name = i.Name }).OrderBy(a => a.Name).AsNoTracking().ToListAsync();

            var peopleJobs = context.PeopleJobs.Select(i => i.JobId).Distinct();

            ViewBag.Jobs = await context.Jobs.Where(a => peopleJobs.Contains(a.Id)).Select(i => new Job { Id = i.Id, NameTr = i.NameTr, NameEn = i.NameEn }).OrderBy(a => a.NameTr).AsNoTracking().ToListAsync();
            return View(liste);

        }


        [Route("kim-kimdir/{id:int}/{adi}")]
        public async Task<IActionResult> Detail(int id)
        {
            var model = await (from a in context.People
                               where a.Id == id  && a.PositionFieldId == 1
                               select new PersonDetailModel
                               {
                                   Id = a.Id,
                                   CreatedDate = a.CreatedDate,
                                   CvEn = a.CvEn,
                                   CvTr = a.CvTr,
                                   DateOfBirth = a.DateOfBirth,
                                   EducationId = a.EducationId,
                                   FirstName = a.FirstName,
                                   GenderId = a.GenderId,
                                   GenderName = a.Gender.NameTr,
                                   GenderNameEn = a.Gender.NameEn,
                                   LastName = a.LastName,
                                   Photo = a.Photo,
                                   PlaceOfBirth = a.PlaceOfBirth,
                                   PositionFieldId = a.PositionFieldId,
                                   PositionFieldName = a.PositionField.NameTr,
                                   PositionFieldNameEn = a.PositionField.NameEn,
                                   JobNames = String.Join(", ", a.PeopleJobs.Select(i => i.Job.NameTr)),
                                   JobNamesEn = String.Join(", ", a.PeopleJobs.Select(i => i.Job.NameEn)),
                                   EducationName = a.Education.NameTr,
                                   EducationNameEn = a.Education.NameEn,
                                   UpdatedDate = a.UpdatedDate,
                                   IsActive = a.IsActive,
                                   PartyId = a.PartyId

                               }
                                       ).FirstOrDefaultAsync();

            if (model == null)
                return RedirectToAction("Index");

            var person = await context.People.FirstOrDefaultAsync(i => i.Id == id);
            person.Views++;
            await context.SaveChangesAsync();

            BaseModel? prev = await context.People.Where(i => i.Id < id).OrderByDescending(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.FirstName + " " + i.LastName }).FirstOrDefaultAsync();
            BaseModel? next = await context.People.Where(i => i.Id > id).OrderBy(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.FirstName + " " + i.LastName }).FirstOrDefaultAsync();
            ViewBag.Prev = prev;
            ViewBag.Next = next;

            var personPositions = await (from a in context.PeoplePositions
                                         join p in context.Provinces on a.ProvinceId equals p.Id into p1
                                         from province in p1.DefaultIfEmpty()
                                         join d in context.Districts on a.DistrictId equals d.Id into d1
                                         from district in d1.DefaultIfEmpty()
                                         where a.PeopleId == id
                                         orderby a.StartDate descending
                                         select new PeoplePositionModel
                                         {
                                             PeopleId = a.PeopleId,
                                             Description = a.Description,

                                             Id = a.Id,
                                             InstitutionTypeName = a.InstitutionType.NameTr,
                                             InstitutionTypeNameEn = a.InstitutionType.NameEn,
                                             InstitutionTypeId = a.InstitutionTypeId,
                                             InstitutionName = a.InstitutionName,
                                             PartyId = a.PartyId,
                                             PartyName = a.PartyId.HasValue ? a.Party.ShortName : "",
                                             PartyNameEn = a.PartyId.HasValue ? a.Party.NameEn : "",
                                             Period = a.Period,
                                             PositionId = a.PositionId,
                                             PositionName = a.Position.NameTr,
                                             PositionNameEn = a.Position.NameEn,
                                             StartDay = a.StartDay,
                                             StartMonth = a.StartMonth,
                                             StartYear = a.StartYear,
                                             EndYear = a.EndYear,
                                             EndMonth = a.EndMonth,
                                             EndDay = a.EndDay,
                                             DistrictId = a.DistrictId,
                                             ProvinceId = a.ProvinceId,
                                             ProvinceName = province != null ? province.Name : "",
                                             DistrictName = district != null ? district.Name : "",
                                             SectorName = String.IsNullOrEmpty(a.Sector.NameTr) ? "" : a.Sector.NameTr,
                                             SectorNameEn = String.IsNullOrEmpty(a.Sector.NameEn) ? "" : a.Sector.NameEn,

                                         }).ToListAsync();

            ViewBag.PersonPositions = personPositions;
            //similar people
            var lastPos = personPositions.Where(i => !i.EndDate.HasValue).LastOrDefault();


            ViewBag.SimilarPeople = await (from a in context.People
                                           let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id /*&& !i.EndDate.HasValue*/).Select(i => new
                                           {
                                               PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                               PositionName = i.Position.NameTr,
                                               PositionNameEn = i.Position.NameEn,
                                               Institution = i.InstitutionName,
                                               PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                               InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                               PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                               Period = i.Period,
                                               StartDate = i.StartDate,
                                               SectorId = i.SectorId,
                                               SectorName = i.Sector.NameTr,
                                               SectorNameEn = i.Sector.NameEn,
                                               InstitutionType = i.InstitutionType.NameTr,
                                               InstitutionTypeEn = i.InstitutionType.NameEn,

                                           }).OrderByDescending(f => f.StartDate).FirstOrDefault()

                                           where a.Id != id && (lastPos != null && a.PeoplePositions.Any(p => p.InstitutionTypeId == lastPos.InstitutionTypeId
                                           && p.PositionId == lastPos.PositionId && !p.EndDate.HasValue && ((lastPos.InstitutionTypeId == 2 && p.ProvinceId == lastPos.ProvinceId) || lastPos.InstitutionTypeId != 2)))
                                           orderby Guid.NewGuid()
                                           select new PeopleListModel
                                           {


                                               LastName = a.LastName,
                                               CreatedDate = a.CreatedDate,
                                               EducationId = a.EducationId,
                                               EducationName = a.Education.NameTr,
                                               EducationNameEn = a.Education.NameEn,
                                               FirstName = a.FirstName,
                                               GenderId = a.GenderId,
                                               GenderName = a.Gender.NameTr,
                                               GenderNameEn = a.Gender.NameEn,
                                               Id = a.Id,
                                               PlaceOfBirth = a.PlaceOfBirth,
                                               DateOfBirth = a.DateOfBirth,
                                               Photo = a.Photo,
                                               UpdatedDate = a.UpdatedDate,
                                               IsActive = a.IsActive,
                                               PartyName = position != null ? (String.IsNullOrEmpty(position.PartyName) ? position.PositionName : position.PartyName) : "",
                                               PartyNameEn = position != null ? (String.IsNullOrEmpty(position.PartyName) ? position.PositionNameEn : position.PartyName) : "",
                                               PositionName = position != null ? position.PositionName : "",
                                               PositionNameEn = position != null ? position.PositionNameEn : "",
                                               InstitutionName = position != null ? position.Institution : "",
                                               SectorName = position != null ? position.SectorName : "",
                                               SectorNameEn = position != null ? position.SectorNameEn : "",
                                               Period = position != null ? position.Period : "",
                                               InstitutionTypeName = position != null ? position.InstitutionType : "",
                                               InstitutionTypeNameEn = position != null ? position.InstitutionTypeEn : "",
                                               //SocialMedias = a.PeopleSocialMedia.Select(i => new SocialMediaModel
                                               //{
                                               //    Id = i.SocialMediaId,
                                               //    ParentId = i.PeopleId,
                                               //    SocialAddress = i.SocialMedia.SocialAddress,
                                               //    SocialTypeId = i.SocialMedia.SocialTypeId,
                                               //    SocialTypeNameEn = i.SocialMedia.SocialType.NameEn,
                                               //    SocialTypeNameTr = i.SocialMedia.SocialType.NameTr,
                                               //    Url = i.SocialMedia.SocialType.UrlPattern.Replace("{value}", i.SocialMedia.SocialAddress)
                                               //}).ToList()
                                           }).Take(6).ToListAsync();

            ViewBag.Phones = await (from a in context.PeoplePhones
                                    where a.PeopleId == id
                                    select new PhoneModel
                                    {
                                        PhoneTypeNameTr = a.Phone.PhoneType.NameTr,
                                        PhoneTypeNameEn = a.Phone.PhoneType.NameEn,
                                        CommunicationTypeId = a.Phone.CommunicationTypeId,
                                        CommunicationTypeNameTr = a.Phone.CommunicationType.NameTr,
                                        CommunicationTypeNameEn = a.Phone.CommunicationType.NameEn,
                                        PhoneId = a.Phone.Id,
                                        Id = a.Id,
                                        PhoneTypeId = a.Phone.PhoneTypeId,
                                        CountryCode = a.Phone.CountryCode,
                                        CountryId = a.Phone.CountryId,
                                        PrefixCode = a.Phone.PrefixCode,
                                        PhoneNumber = a.Phone.PhoneNumber,

                                    }).AsNoTracking().ToListAsync();

            ViewBag.Emails = await context.PeopleEmails.Where(a => a.PeopleId == id)
                .Select(i => new EmailModel
                {
                    CommunicationTypeId = i.Email.CommunicationTypeId,
                    CommunicationTypeNameTr = i.Email.CommunicationType.NameTr,
                    CommunicationTypeNameEn = i.Email.CommunicationType.NameEn,
                    Id = i.EmailId,
                    ParentId = i.PeopleId,
                    EmailAddress = i.Email.EmailAddress,

                }).AsNoTracking().ToListAsync();




            ViewBag.SocialAdress = await context.PeopleSocialMedia.Where(a => a.PeopleId == id)
                .Select(i => new SocialMediaModel
                {
                    Id = i.SocialMediaId,
                    ParentId = i.PeopleId,
                    SocialAddress = i.SocialMedia.SocialAddress,
                    SocialTypeId = i.SocialMedia.SocialTypeId,
                    SocialTypeNameEn = i.SocialMedia.SocialType.NameEn,
                    SocialTypeNameTr = i.SocialMedia.SocialType.NameTr,
                    Url = i.SocialMedia.SocialType.UrlPattern.Replace("{value}", i.SocialMedia.SocialAddress)
                }).AsNoTracking().ToListAsync();


            ViewBag.Addresses = await (from a in context.PeopleAddresses
                                       where a.PeopleId == id
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
                                           CountryNameEn = a.Address.Country.NameEn,
                                           CreatedDate = a.Address.CreatedDate,
                                           NameEn = a.Address.NameEn,
                                           NameTr = a.Address.NameTr,
                                           PostalCode = a.Address.PostalCode,
                                           Province = a.Address.Province,
                                           UpdatedDate = a.Address.UpdatedDate,
                                           DistrictId = a.Address.DistrictId,
                                           ProvinceId = a.Address.ProvinceId
                                       }).AsNoTracking().ToListAsync();

            return View(model);
        }




        public async Task<IActionResult> Charts()
        {
            ViewBag.Parties = context.Parties.OrderBy(a => a.ShortName).ToList();
            ViewBag.Regions = context.Regions.OrderBy(a => a.Id).ToList();
            var peopleJobs = context.PeopleJobs.Select(i => i.JobId).Distinct();
            ViewBag.Jobs = await context.Jobs.Where(a => peopleJobs.Contains(a.Id)).Select(i => new Job { Id = i.Id, NameTr = i.NameTr, NameEn = i.NameEn }).OrderBy(a => a.NameTr).AsNoTracking().ToListAsync();
            var positionsDist = context.PeoplePositions.Select(x => x.PositionId).Distinct();
            ViewBag.Positions = await context.Positions.Where(a => positionsDist.Contains(a.Id)).Select(i => new Position { Id = i.Id, NameTr = i.NameTr, NameEn = i.NameEn, Weightiness = i.Weightiness }).OrderBy(i => i.Weightiness).AsNoTracking().ToListAsync();

            int[] selparti = { 1, 2, 3, 4, 7, 8 };
            ViewBag.DefaultSelectedParties = selparti;

            return View();
        }

    
        public async Task<IActionResult> GetData(int t, string partiesstr, string regionsstr, string titlesstr, string agesstr, string genderstr, string jobsstr, int isactive)
        {
            string[] date;
            int[] akdeniz1 = { 1,7,15,31,32,33,46,80 };
            int[] doguAnadolu5 = { 4,12,13,23,24,25,30,36,44,49,62,65,75,76 };
            int[] karadeniz3 = { 5,8,14,19,28,29,37,52,53,55,57,60,61,67,69,74,78,81 };
            int[] guneydoguAnadolu6 = { 2,21,27,47,56,63,72,73,79 };
            int[] ege2 = { 3,9,20,35,43,45,48,64 };
            int[] marmara7 = { 10,11,16,17,22,34,39,41,54,59,77 };
            int[] icAnadolu4 = { 6,18,26,38,40,42,50,51,58,66,68,70,71 };
            int[] educations = { 1, 2, 3, 4, 5, 6, 7, 24 };
            int[] partisizUnvan = { 64, 65, 66, 68, 72, 73, 74,5,7,6,8,9,10,61,64,13,14,15,20,21,22,23,24,32,33,34,35,36,37,38,39,40,41,43,44,45,46,49,52,53,54,55,56,11,12,25,47,50,51 };
            int[] yersizUnvan = { 64, 65, 66, 68, 72, 73, 74, 5, 7, 6, 75, 65, 8, 9, 10, 61, 13, 14, 15, 60, 16, 67, 17, 68, 18, 19, 77, 20, 21, 22, 23, 24, 76, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 71, 43, 44, 45, 46, 48, 49, 52, 53, 54, 55, 56, 11, 12, 42, 26, 25, 47, 50, 51 };


            List<int> parties=new List<int>();
            List<int> regions = new List<int>();
            List<int> titles = new List<int>();
            List<int> ages = new List<int>();
            List<int> gender = new List<int>();
            List<int> jobs = new List<int>();

            if (partiesstr.Contains(";"))
            {
                parties= partiesstr.Split(';').Select(n => Convert.ToInt32(n)).ToList();
            } else
            {
                parties.Add(Convert.ToInt32(partiesstr));
            }

            if (regionsstr.Contains(";"))
            {
                regions = regionsstr.Split(';').Select(n => Convert.ToInt32(n)).ToList();
            } else
            {
                regions.Add(Convert.ToInt32(regionsstr));
            }

            if (titlesstr.Contains(";"))
            {
                titles = titlesstr.Split(';').Select(n => Convert.ToInt32(n)).ToList();
            } else
            {
                titles.Add(Convert.ToInt32(titlesstr));
            }

            if (agesstr.Contains(";"))
            {
                ages = agesstr.Split(';').Select(n => Convert.ToInt32(n)).ToList();
            } else
            {
                ages.Add(Convert.ToInt32(agesstr));
            }

            if (genderstr.Contains(";"))
            {
                gender = genderstr.Split(';').Select(n => Convert.ToInt32(n)).ToList();
            }
            else
            {
                gender.Add(Convert.ToInt32(genderstr));
            }

            if (jobsstr.Contains(";"))
            {
                jobs = jobsstr.Split(';').Select(n => Convert.ToInt32(n)).ToList();
            }
            else
            {
                jobs.Add(Convert.ToInt32(jobsstr));
            }

            List<string> partiNames = new List<string>();
            foreach (var item in parties)
            {
                var adi = context.Parties.FirstOrDefault(a => a.Id == item).ShortName;
                partiNames.Add(adi);
            }
            List<string> regionNames = new List<string>();
            foreach (var item in regions)
            {
                var adi = context.Regions.FirstOrDefault(a => a.Id == item).NameTr;
                regionNames.Add(adi);
            }
            List<string> titleNames = new List<string>();
            foreach (var item in titles)
            {
                var adi = context.Positions.FirstOrDefault(a => a.Id == item).NameTr;
                titleNames.Add(adi);
            }

            List<string> jobNames = new List<string>();
            foreach (var item in jobs)
            {
                var adi = context.Jobs.FirstOrDefault(a => a.Id == item).NameTr;
                jobNames.Add(adi);
            }



            //var sonuc = new Array[16];

            //sonuc[0] = regionNames.ToArray();
            //sonuc[1] = partiNames.ToArray();
            //sonuc[2] = titleNames.ToArray();
            //sonuc[3] = jobNames.ToArray();
            //sonuc[4] = regions;
            //sonuc[5] = parties;
            //sonuc[6] = titles;
            //sonuc[7] = jobs;


            switch (t)
            {
                case 1: //bölgelere göre
                    var defRegions= context.Regions.OrderBy(a => a.Id).ToList();
                    var sonuc = new Array[2];
                    foreach (var item in regions)
                    {
                        var bolgeAdi = context.Regions.FirstOrDefault(a => a.Id == item).NameTr;

                    }

                    List<int> counts = new List<int>();

                    

                    foreach (var item in defRegions)
                    {
                        var liste = await (from a in context.People
                                           let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id && ((!i.EndDate.HasValue && isactive == 1) || (isactive == 2)) && ((parties.Contains((int)i.PartyId) && parties.Count() > 0) ) && (titles.Contains(i.PositionId) && titles.Count() > 0) && ((item.Id==1 && akdeniz1.Contains((int)i.ProvinceId)) || (item.Id == 2 && ege2.Contains((int)i.ProvinceId)) || (item.Id == 3 && karadeniz3.Contains((int)i.ProvinceId)) || (item.Id == 4 && icAnadolu4.Contains((int)i.ProvinceId)) || (item.Id == 5 && doguAnadolu5.Contains((int)i.ProvinceId)) || (item.Id == 6 && guneydoguAnadolu6.Contains((int)i.ProvinceId)) || (item.Id == 7 && marmara7.Contains((int)i.ProvinceId)))).Select(i => new
                                           {
                                               PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                               PositionName = i.Position.NameTr,

                                               PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                               InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                               PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                               Period = i.Period,
                                               StartDate = i.StartDate,
                                               InstitutionType = i.InstitutionType.NameTr,
                                               Institution = i.InstitutionType.NameTr,
                                               province = i.ProvinceId

                                           }).OrderByDescending(f => f.StartDate).FirstOrDefault()

                                           where ((gender.Contains(a.GenderId) && gender.Count() > 0))
                                           && ((educations.Contains(a.EducationId)))
                                            && ((position != null))
                                            && ((context.PeopleJobs.Where(c => c.PeopleId == a.Id).Count() > 0 && context.PeopleJobs.Where(d => d.PeopleId == a.Id).FirstOrDefault(e => jobs.Contains(e.JobId)) != null))
                                            && ((((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 15 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 22 && ages.Contains(1)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 21 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 36 && ages.Contains(2)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 35 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 46 && ages.Contains(3)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 45 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 56 && ages.Contains(4)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 55 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 65 && ages.Contains(5)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 64 && ages.Contains(6)) || (ages.Contains(7) && a.DateOfBirth == null))) 
                                           select new PeopleListModel
                                           {
                                               LastName = a.LastName,

                                           }
                       ).AsNoTracking().ToListAsync();

                        counts.Add(liste.Count());
                    }
                    sonuc[0] = context.Regions.OrderBy(a=>a.Id).Select(i=>i.NameTr).ToArray();
                    sonuc[1] = counts.ToArray();


                    return Ok(sonuc);
                case 2://ünvanlarına göre

                    var positionsDist = context.PeoplePositions.Select(x => x.PositionId).Distinct();
                    var positions = await context.Positions.Where(a => positionsDist.Contains(a.Id)).Select(i => new Position { Id = i.Id, NameTr = i.NameTr, NameEn = i.NameEn, Weightiness = i.Weightiness }).OrderBy(i => i.Weightiness).AsNoTracking().ToListAsync();


                    var sonuc1 = new Array[2];


                    List<int> counts2 = new List<int>();

                    


                    foreach (var item in positions)
                    {


                        var liste = await (from a in context.People
                                           let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id && ((!i.EndDate.HasValue && isactive == 1) || (isactive == 2)) && ((parties.Contains((int)i.PartyId) && parties.Count() > 0) || partisizUnvan.Contains(item.Id)) && (i.PositionId==item.Id ) && ((regions.Contains(1) && akdeniz1.Contains((int)i.ProvinceId)) || (regions.Contains(2) && ege2.Contains((int)i.ProvinceId)) || (regions.Contains(3) && karadeniz3.Contains((int)i.ProvinceId)) || (regions.Contains(4) && icAnadolu4.Contains((int)i.ProvinceId)) || (regions.Contains(5) && doguAnadolu5.Contains((int)i.ProvinceId)) || (regions.Contains(6) && guneydoguAnadolu6.Contains((int)i.ProvinceId)) || (regions.Contains(7) && marmara7.Contains((int)i.ProvinceId)) || yersizUnvan.Contains(item.Id))).Select(i => new
                                           {
                                               PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                               PositionName = i.Position.NameTr,

                                               PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                               InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                               PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                               Period = i.Period,
                                               StartDate = i.StartDate,
                                               InstitutionType = i.InstitutionType.NameTr,
                                               Institution = i.InstitutionType.NameTr,
                                               province = i.ProvinceId

                                           }).OrderByDescending(f => f.StartDate).FirstOrDefault()

                                           where ((gender.Contains(a.GenderId)))
                                           && ((educations.Contains(a.EducationId)))
                                            && ((position != null))
                                            && ((context.PeopleJobs.Where(c => c.PeopleId == a.Id).Count() > 0 && context.PeopleJobs.Where(d => d.PeopleId == a.Id).FirstOrDefault(e => jobs.Contains(e.JobId)) != null))
                                            && (((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 15 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 22 && ages.Contains(1)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 21 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 36 && ages.Contains(2)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 35 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 46 && ages.Contains(3)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 45 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 56 && ages.Contains(4)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 55 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 65 && ages.Contains(5)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 64 && ages.Contains(6)) || (ages.Contains(7) && a.DateOfBirth == null))
                                           select new PeopleListModel
                                           {
                                               LastName = a.LastName,

                                           }
                       ).AsNoTracking().ToListAsync();

                        counts2.Add(liste.Count);
                    }

                    sonuc1[0] = positions.Select(a=>a.NameTr).ToArray();
                    sonuc1[1] = counts2.ToArray();

                    return Ok(sonuc1);
                case 3://mesleğine  göre
                    var jobdist = context.PeopleJobs.Select(x => x.JobId).Distinct();
                    var jobslist = await context.Jobs.Where(a => jobdist.Contains(a.Id)).Select(i => new Job { Id = i.Id, NameTr = i.NameTr, NameEn = i.NameEn }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();


                    var defRegions2 = context.Regions.OrderBy(a => a.Id).ToList();
                    var sonuc2 = new Array[2];
                    foreach (var item in regions)
                    {
                        var bolgeAdi = context.Regions.FirstOrDefault(a => a.Id == item).NameTr;

                    }

                    List<int> counts3 = new List<int>();


                    foreach (var item in jobslist)
                    {
                        var liste = await (from a in context.People
                                           let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id && ((!i.EndDate.HasValue && isactive == 1) || (isactive == 2)) && ((parties.Contains((int)i.PartyId) && parties.Count() > 0) || partisizUnvan.Contains(i.PositionId)) && ((titles.Contains(i.PositionId) && titles.Count() > 0)) && ((regions.Contains(1) && akdeniz1.Contains((int)i.ProvinceId)) || (regions.Contains(2) && ege2.Contains((int)i.ProvinceId)) || (regions.Contains(3) && karadeniz3.Contains((int)i.ProvinceId)) || (regions.Contains(4) && icAnadolu4.Contains((int)i.ProvinceId)) || (regions.Contains(5) && doguAnadolu5.Contains((int)i.ProvinceId)) || (regions.Contains(6) && guneydoguAnadolu6.Contains((int)i.ProvinceId)) || (regions.Contains(7) && marmara7.Contains((int)i.ProvinceId)) || yersizUnvan.Contains(i.PositionId))).Select(i => new
                                           {
                                               PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                               PositionName = i.Position.NameTr,

                                               PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                               InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                               PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                               Period = i.Period,
                                               StartDate = i.StartDate,
                                               InstitutionType = i.InstitutionType.NameTr,
                                               Institution = i.InstitutionType.NameTr,
                                               province = i.ProvinceId

                                           }).OrderByDescending(f => f.StartDate).FirstOrDefault()

                                           where ((gender.Contains(a.GenderId) && gender.Count() > 0))
                                           && ((educations.Contains(a.EducationId)))
                                            && ((position != null))
                                            && ((context.PeopleJobs.Where(c => c.PeopleId == a.Id).Count() > 0 && context.PeopleJobs.Where(d => d.PeopleId == a.Id).FirstOrDefault(e => e.JobId==item.Id) != null))
                                            && ((((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 15 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 22 && ages.Contains(1)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 21 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 36 && ages.Contains(2)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 35 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 46 && ages.Contains(3)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 45 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 56 && ages.Contains(4)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 55 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 65 && ages.Contains(5)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 64 && ages.Contains(6)) || (ages.Contains(7) && a.DateOfBirth == null))) 
                                           select new PeopleListModel
                                           {
                                               LastName = a.LastName,

                                           }
                       ).AsNoTracking().ToListAsync();

                        counts3.Add(liste.Count);
                    }

                    sonuc2[0] = jobslist.Select(a => a.NameTr).ToArray();
                    sonuc2[1] = counts3.ToArray();

                    return Ok(sonuc2);
                case 4://eğitim durumuna  göre

                    var defRegions3 = context.Regions.OrderBy(a => a.Id).ToList();
                    var sonuc3 = new Array[2];
                    foreach (var item in regions)
                    {
                        var bolgeAdi = context.Regions.FirstOrDefault(a => a.Id == item).NameTr;

                    }

                    List<int> counts1 = new List<int>();


                    foreach (var item in educations)
                    {
                        var liste = await (from a in context.People
                                           let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id && ((!i.EndDate.HasValue && isactive == 1) || (isactive == 2)) && ((parties.Contains((int)i.PartyId) && parties.Count() > 0) || partisizUnvan.Contains(i.PositionId)) && (titles.Contains(i.PositionId) && titles.Count() > 0) && ((regions.Contains(1) && akdeniz1.Contains((int)i.ProvinceId)) || (regions.Contains(2) && ege2.Contains((int)i.ProvinceId)) || (regions.Contains(3) && karadeniz3.Contains((int)i.ProvinceId)) || (regions.Contains(4) && icAnadolu4.Contains((int)i.ProvinceId)) || (regions.Contains(5) && doguAnadolu5.Contains((int)i.ProvinceId)) || (regions.Contains(6) && guneydoguAnadolu6.Contains((int)i.ProvinceId)) || (regions.Contains(7) && marmara7.Contains((int)i.ProvinceId)) || yersizUnvan.Contains(i.PositionId))).Select(i => new
                                           {
                                               PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                               PositionName = i.Position.NameTr,

                                               PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                               InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                               PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                               Period = i.Period,
                                               StartDate = i.StartDate,
                                               InstitutionType = i.InstitutionType.NameTr,
                                               Institution = i.InstitutionType.NameTr,
                                               province = i.ProvinceId

                                           }).OrderByDescending(f => f.StartDate).FirstOrDefault()

                                           where (gender.Contains(a.GenderId) )
                                           && (a.EducationId==item)
                                            && ((position != null))
                                            && ((context.PeopleJobs.Where(c => c.PeopleId == a.Id).Count() > 0 && context.PeopleJobs.Where(d => d.PeopleId == a.Id).FirstOrDefault(e => jobs.Contains(e.JobId)) != null))
                                            && (((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 15 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 22 && ages.Contains(1)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 21 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 36 && ages.Contains(2)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 35 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 46 && ages.Contains(3)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 45 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 56 && ages.Contains(4)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 55 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 65 && ages.Contains(5)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 64 && ages.Contains(6)) || (ages.Contains(7) && a.DateOfBirth == null)) 
                                           select new PeopleListModel
                                           {
                                               LastName = a.LastName,

                                           }
                       ).AsNoTracking().ToListAsync();

                        counts1.Add(liste.Count);
                    }

                    sonuc3[0] = context.Educations.OrderBy(a=>a.Id).Select(a => a.NameTr).ToArray();
                    sonuc3[1] = counts1.ToArray();

                    return Ok(sonuc3);
                case 5://yaş grubuna göre
                    var jobdist2 = context.PeopleJobs.Select(x => x.JobId).Distinct();
                    var jobslist2 = context.Educations.Select(a => a.Id).ToList();

                    var defRegions4 = context.Regions.OrderBy(a => a.Id).ToList();
                    var sonuc4 = new Array[2];
                    foreach (var item in regions)
                    {
                        var bolgeAdi = context.Regions.FirstOrDefault(a => a.Id == item).NameTr;

                    }
                    ages =new List<int> { 1, 2, 3, 4, 5, 6, 7 };
                    List<int> counts4 = new List<int>();


                    foreach (var item in ages)
                    {


                        var liste = await (from a in context.People
                                           let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id && ((!i.EndDate.HasValue && isactive == 1) || (isactive == 2)) && (( parties.Contains((int)i.PartyId) && parties.Count() > 0) || partisizUnvan.Contains(i.PositionId)) && (titles.Contains(i.PositionId)  && titles.Count()> 0) && ((regions.Contains(1) && akdeniz1.Contains((int)i.ProvinceId)) || (regions.Contains(2) && ege2.Contains((int)i.ProvinceId)) || (regions.Contains(3) && karadeniz3.Contains((int)i.ProvinceId)) || (regions.Contains(4) && icAnadolu4.Contains((int)i.ProvinceId)) || (regions.Contains(5) && doguAnadolu5.Contains((int)i.ProvinceId)) || (regions.Contains(6) && guneydoguAnadolu6.Contains((int)i.ProvinceId)) || (regions.Contains(7) && marmara7.Contains((int)i.ProvinceId)) || yersizUnvan.Contains(i.PositionId))).Select(i => new
                                           {
                                               PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                               PositionName = i.Position.NameTr,

                                               PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                               InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                               PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                               Period = i.Period,
                                               StartDate = i.StartDate,
                                               InstitutionType = i.InstitutionType.NameTr,
                                               Institution = i.InstitutionType.NameTr,
                                               province = i.ProvinceId

                                           }).OrderByDescending(f => f.StartDate).FirstOrDefault()

                                           where  ((gender.Contains(a.GenderId) && gender.Count()>0))
                                           && ((educations.Contains(a.EducationId)))
                                            && ((position != null))
                                            && ((context.PeopleJobs.Where(c => c.PeopleId == a.Id).Count() > 0 && context.PeopleJobs.Where(d => d.PeopleId == a.Id).FirstOrDefault(e => jobs.Contains(e.JobId)) != null))
                                            && (((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 15 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 22 && item==1) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 21 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 36 && item==2) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 35 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 46 && item==3) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 45 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 56 && item==4) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 55 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 65 && item==5) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 64 && item==6) || (item==7 && a.DateOfBirth == null)) 
                                           select new PeopleListModel
                                           {
                                               LastName = a.LastName,

                                           }
                       ).AsNoTracking().ToListAsync();



                        counts4.Add(liste.Count);
                    }

                    sonuc4[0] = new string[] { "16-21", "22-35", "36-45", "46-55", "56-64", "65+","Bilinmiyor" };
                    sonuc4[1] = counts4.ToArray();

                    return Ok(sonuc4);

                case 6://cinsiyetine göre

                    var jobdist3 = context.PeopleJobs.Select(x => x.JobId).Distinct();
                    var jobslist3 = context.Educations.Select(a => a.Id).ToList();

                    var defRegions5 = context.Regions.OrderBy(a => a.Id).ToList();
                    var sonuc5 = new Array[2];
                    foreach (var item in regions)
                    {
                        var bolgeAdi = context.Regions.FirstOrDefault(a => a.Id == item).NameTr;

                    }
                    var genderrr = new int[] { 1, 2, };
                    List<int> counts5 = new List<int>();


                    foreach (var item in genderrr)
                    {
                        var liste = await (from a in context.People
                                           let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id && ((!i.EndDate.HasValue && isactive == 1) || (isactive == 2)) && ((parties.Contains((int)i.PartyId) && parties.Count() > 0) || partisizUnvan.Contains(i.PositionId)) && (titles.Contains(i.PositionId) && titles.Count() > 0) && ((regions.Contains(1) && akdeniz1.Contains((int)i.ProvinceId)) || (regions.Contains(2) && ege2.Contains((int)i.ProvinceId)) || (regions.Contains(3) && karadeniz3.Contains((int)i.ProvinceId)) || (regions.Contains(4) && icAnadolu4.Contains((int)i.ProvinceId)) || (regions.Contains(5) && doguAnadolu5.Contains((int)i.ProvinceId)) || (regions.Contains(6) && guneydoguAnadolu6.Contains((int)i.ProvinceId)) || (regions.Contains(7) && marmara7.Contains((int)i.ProvinceId)) || yersizUnvan.Contains(i.PositionId))).Select(i => new
                                           {
                                               PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                               PositionName = i.Position.NameTr,

                                               PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                               InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                               PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                               Period = i.Period,
                                               StartDate = i.StartDate,
                                               InstitutionType = i.InstitutionType.NameTr,
                                               Institution = i.InstitutionType.NameTr,
                                               province = i.ProvinceId

                                           }).OrderByDescending(f => f.StartDate).FirstOrDefault()

                                           where ((a.GenderId==item && gender.Count() > 0))
                                           && ((educations.Contains(a.EducationId)))
                                            && ((position != null))
                                            && ((context.PeopleJobs.Where(c => c.PeopleId == a.Id).Count() > 0 && context.PeopleJobs.Where(d => d.PeopleId == a.Id).FirstOrDefault(e => jobs.Contains(e.JobId)) != null))
                                            && ((((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 15 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 22 && ages.Contains(1)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 21 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 36 && ages.Contains(2)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 35 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 46 && ages.Contains(3)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 45 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 56 && ages.Contains(4)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 55 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 65 && ages.Contains(5)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 64 && ages.Contains(6)) || (ages.Contains(7) && a.DateOfBirth == null))) 
                                           select new PeopleListModel
                                           {
                                               LastName = a.LastName,

                                           }
                       ).AsNoTracking().ToListAsync();

                        counts5.Add(liste.Count);
                    }

                    sonuc5[0] = new string[] { "Erkek", "Kadın" };
                    sonuc5[1] = counts5.ToArray();

                    return Ok(sonuc5);
                case 7://partilere göre

                    var sonuc6 = new Array[2];

                    var partiesfix = context.Parties.Select(a => a.Id).ToList();
                    List<int> counts6 = new List<int>();


                    foreach (var item in partiesfix)
                    {
                        var liste = await (from a in context.People
                                           let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id && ((!i.EndDate.HasValue && isactive == 1) || (isactive == 2)) && ((i.PartyId==item && parties.Count() > 0) || partisizUnvan.Contains(i.PositionId)) && (titles.Contains(i.PositionId) && titles.Count() > 0) && ((regions.Contains(1) && akdeniz1.Contains((int)i.ProvinceId)) || (regions.Contains(2) && ege2.Contains((int)i.ProvinceId)) || (regions.Contains(3) && karadeniz3.Contains((int)i.ProvinceId)) || (regions.Contains(4) && icAnadolu4.Contains((int)i.ProvinceId)) || (regions.Contains(5) && doguAnadolu5.Contains((int)i.ProvinceId)) || (regions.Contains(6) && guneydoguAnadolu6.Contains((int)i.ProvinceId)) || (regions.Contains(7) && marmara7.Contains((int)i.ProvinceId)) || yersizUnvan.Contains(i.PositionId))).Select(i => new
                                           {
                                               PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                               PositionName = i.Position.NameTr,

                                               PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                               InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                               PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                               Period = i.Period,
                                               StartDate = i.StartDate,
                                               InstitutionType = i.InstitutionType.NameTr,
                                               Institution = i.InstitutionType.NameTr,
                                               province = i.ProvinceId

                                           }).OrderByDescending(f => f.StartDate).FirstOrDefault()

                                           where ((gender.Contains(a.GenderId) && gender.Count() > 0))
                                           && ((educations.Contains(a.EducationId)))
                                            && ((position != null))
                                            && ((context.PeopleJobs.Where(c => c.PeopleId == a.Id).Count() > 0 && context.PeopleJobs.Where(d => d.PeopleId == a.Id).FirstOrDefault(e => jobs.Contains(e.JobId)) != null))
                                            && ((((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 15 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 22 && ages.Contains(1)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 21 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 36 && ages.Contains(2)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 35 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 46 && ages.Contains(3)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 45 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 56 && ages.Contains(4)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 55 && (DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) < 65 && ages.Contains(5)) || ((DateTime.Now.Year - Convert.ToInt32(a.DateOfBirth)) > 64 && ages.Contains(6)) || (ages.Contains(7) && a.DateOfBirth == null))) 
                                           select new PeopleListModel
                                           {
                                               LastName = a.LastName,

                                           }
                       ).AsNoTracking().ToListAsync();

                        counts6.Add(liste.Count);
                    }
                    sonuc6[0] = context.Parties.Select(a => a.ShortName).ToArray();
                    sonuc6[1] = counts6.ToArray();

                    return Ok(sonuc6);


                default:
                    break;
            }
            return Ok();
        }

        public List<District> GetDistricts(int id)
        {
            return context.Districts.Where(i => i.ProvinceId == id).OrderBy(i => i.Name).Select(i => new District { Id = i.Id, Name = i.Name, ProvinceId = i.ProvinceId }).ToList();
        }


        public async Task<IActionResult> GetFeedBack(FeedBackModel obj)
        {
            var feedback = new Feedback();
            feedback.Name = obj.Name;
            feedback.Lastname = obj.LastName;
            feedback.Email = obj.Email;
            feedback.Topic = obj.Topic;
            feedback.Message = obj.Text;
            feedback.PageLink = obj.PageUrl;
            feedback.SendTime = DateTime.Now;
            feedback.Title = obj.Title;
            context.Feedbacks.Add(feedback);
            context.SaveChanges();

            return Ok(new { success=true});
        }
    }
}
