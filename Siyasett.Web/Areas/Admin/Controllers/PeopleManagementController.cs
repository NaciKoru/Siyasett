using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siyasett.Core.Addresses;
using Siyasett.Core.Phones;
using Siyasett.Core.People;
using Siyasett.Data;
using Siyasett.Data.Data;
using Siyasett.Web.Models;
using System.Globalization;
using Siyasett.Core.Emails;
using Siyasett.Core.SocialMedia;

using Siyasett.Models;
using Siyasett.Core.Party;
using Siyasett.Core;
using Siyasett.Core.Extensions;



namespace Siyasett.Web.Areas.Admin.Controllers
{

    public class PeopleManagementController : AdminController
    {
        private readonly IWebHostEnvironment _host;
        public PeopleManagementController(ILogger<AdminController> logger, IWebHostEnvironment env, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {
            _host = env;
        }

        public async Task<IActionResult> Index(int page = 1, byte sort = 0, byte desc = 0, int pagesize = 30, string firstName = "", string lastName = "", byte gender = 0, int education = 0, int partyId = 0, int instituiton = 0, int positionId = 0, string period = "", int isactivepolitics = 1)
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;
            string[] orderFields = new string[] { "FirstName", "LastName", "GenderId", "EducationId", "PartyName", "PositionId", "Period" };
            pager.SetQuery(Request.Query);
            PeopleSearchModel searchModel = new PeopleSearchModel();
            searchModel.FirstName = firstName;
            searchModel.LastName = lastName;
            searchModel.GenderId = gender;
            searchModel.EducationId = education;
            searchModel.PartyId = partyId;
            searchModel.InstitutionTypeId = instituiton;
            searchModel.PositionId = positionId;
            searchModel.OrderBy = sort;
            searchModel.OrderByDesc = desc;
            searchModel.Period = period;
            searchModel.IsActivePolitics = isactivepolitics;


            var liste = await (from a in context.People
                               let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id && ((!i.EndDate.HasValue && isactivepolitics == 1) || (isactivepolitics == 2))).Select(i => new
                               {
                                   PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                   PositionName = i.Position.NameTr,
                                   Institution = context.InstitutionTypes.FirstOrDefault(z => z.Id == i.InstitutionTypeId).NameTr == null ? "" : context.InstitutionTypes.FirstOrDefault(z => z.Id == i.InstitutionTypeId).NameTr,
                                   PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                   InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                   PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                   Period = i.Period,
                                   StartDate = i.StartDate,
                               }).Where(instituiton != 0 ? c => c.InsIdPos == instituiton : c => c.InsIdPos != 0 && string.IsNullOrEmpty(period) ? (c.Period == "" || c.Period != "") : c.Period.ToUpper().Contains(Helpers.Normalize(period))).OrderByDescending(f => f.StartDate).FirstOrDefault()
                               where (string.IsNullOrEmpty(firstName) || (a.FirstNameNormalize).Contains(Helpers.Normalize(firstName)))
                               && (string.IsNullOrEmpty(lastName) || (a.LastNameNormalize).Contains(Helpers.Normalize(lastName)))
                               && (searchModel.GenderId == 0 || (searchModel.GenderId == a.GenderId))
                               && (searchModel.EducationId == 0 || (searchModel.EducationId == a.EducationId))
                               && (searchModel.PartyId == 0 || (searchModel.PartyId == position.PartyIdPos))
                               && (searchModel.PositionId == 0 || (searchModel.PositionId == position.PosPosId))
                               && (searchModel.InstitutionTypeId == 0 || (searchModel.InstitutionTypeId == position.InsIdPos))
                               && (string.IsNullOrEmpty(period) || (position.Period.ToUpper().Contains(Helpers.Normalize(period))))
                               && ((position != null && isactivepolitics == 1) || (isactivepolitics == 2 ))
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
                                   Photo = a.Photo,
                                   UpdatedDate = a.UpdatedDate,
                                   IsActive = a.IsActive,
                                   PartyName = position != null ? (String.IsNullOrEmpty(position.PartyName) ? position.PositionName : position.PartyName) : "",
                                   PositionName = position != null ? position.PositionName : "",
                                   InstitutionName = position != null ? position.Institution : "",
                                   Period = position != null ? position.Period : "",
                               }
                       ).OrderByField(orderFields[searchModel.OrderBy], searchModel.OrderByDesc == 0).Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).AsNoTracking().ToListAsync();

            pager.Total = await (from a in context.People
                                 let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id && ((!i.EndDate.HasValue && isactivepolitics == 1) || (isactivepolitics == 2))).Select(i => new
                                 {
                                     PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                     PositionName = i.Position.NameTr,
                                     Institution = context.InstitutionTypes.FirstOrDefault(z => z.Id == i.InstitutionTypeId).NameTr == null ? "" : context.InstitutionTypes.FirstOrDefault(z => z.Id == i.InstitutionTypeId).NameTr,
                                     PartyIdPos = i.PartyId.HasValue ? i.PartyId : 0,
                                     InsIdPos = i.InstitutionTypeId > 0 ? i.InstitutionTypeId : 0,
                                     PosPosId = i.PositionId > 0 ? i.PositionId : 0,
                                     Period = i.Period,
                                 }).Where(instituiton != 0 ? c => c.InsIdPos == instituiton : c => c.InsIdPos != 0 && string.IsNullOrEmpty(period) ? (c.Period == "" || c.Period != "") : c.Period.ToUpper().Contains(Helpers.Normalize(period))).FirstOrDefault()
                                 where (string.IsNullOrEmpty(firstName) || (a.FirstNameNormalize).Contains(Helpers.Normalize(firstName)))
                               && (string.IsNullOrEmpty(lastName) || (a.LastNameNormalize).Contains(Helpers.Normalize(lastName)))
                               && (searchModel.GenderId == 0 || (searchModel.GenderId == a.GenderId))
                               && (searchModel.EducationId == 0 || (searchModel.EducationId == a.EducationId))
                               && (searchModel.PartyId == 0 || (searchModel.PartyId == position.PartyIdPos))
                               && (searchModel.PositionId == 0 || (searchModel.PositionId == position.PosPosId))
                               && (searchModel.InstitutionTypeId == 0 || (searchModel.InstitutionTypeId == position.InsIdPos))
                               && (string.IsNullOrEmpty(period) || (position.Period.ToUpper().Contains(Helpers.Normalize(period))))
                               && ((position != null && isactivepolitics == 1) || (isactivepolitics == 2))
                                 select new { a.Id }).AsNoTracking().CountAsync();

            ViewBag.Educations = await context.Educations.Select(i => new Education { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.Id).AsNoTracking().ToListAsync();

            ViewBag.SearchModel = searchModel;
            ViewBag.Pager = pager;

            ViewBag.InstitutionTypes = await context.InstitutionTypes.Select(i => new InstitutionType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Positions = await context.Positions.Select(i => new Position { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();

            ViewBag.Parties = await (from a in context.Parties
                                     where a.Active
                                     orderby a.ShortName
                                     select new PartyListModel
                                     {
                                         Id = a.Id,
                                         PartyName = a.ShortName,

                                     }).ToListAsync();

            return View(liste);
        }


        public async Task<IActionResult> New()
        {
            PersonCreateModel model = new PersonCreateModel();


            ViewBag.Genders = await context.Genders.Select(i => new Gender { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.InstitutionTypes = await context.InstitutionTypes.Select(i => new InstitutionType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            // ViewBag.Positions = await context.Positions.Select(i => new Position { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.PositionFields = await context.PositionFields.Select(i => new PositionField { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Educations = await context.Educations.Select(i => new Education { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.Id).AsNoTracking().ToListAsync();

            ViewBag.Jobs = await context.Jobs.Select(i => new Job { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();



            //ViewBag.Parties = await (from a in context.Parties
            //                         where a.Active
            //                         select new PartyListModel
            //                         {
            //                             Id = a.Id,
            //                             PartyName = a.Name,

            //                         }).ToListAsync();

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> New(PersonCreateModel model, int durum)
        {

            if (ModelState.IsValid)
            {
                var person = new Person();
                person.FirstName = model.FirstName;
                person.LastName = model.LastName;
                //person.PartyId = model.PartyId;
                person.InstitutionTypeId = model.InstitutionTypeId;
                person.FirstNameNormalize = Helpers.Normalize(model.FirstName);
                person.LastNameNormalize = Helpers.Normalize(model.LastName);

                person.GenderId = model.GenderId;
                person.PositionFieldId = model.PositionFieldId;
                person.CreatedUserId = this.appUser.Id;
                person.UpdatedUserId = this.appUser.Id;
                person.IsActive = model.IsActive;
                person.EducationId = model.EducationId;
                person.CvEn = model.CvEn;
                person.CvTr = model.CvTr;
                person.DateOfBirth = model.DateOfBirth;
                person.PlaceOfBirth = model.PlaceOfBirth;
                //if (!string.IsNullOrEmpty(model.DateOfBirthStr))
                //{
                //    CultureInfo provider = CultureInfo.InvariantCulture;
                //    try
                //    {
                //        model.DateOfBirth = DateTime.ParseExact(model.DateOfBirthStr, "dd.MM.yyyy", provider, DateTimeStyles.None);
                //    }
                //    catch
                //    { 
                //        model.DateOfBirth = null;
                //    }
                //}
                context.People.Add(person);
                await context.SaveChangesAsync();
                foreach (var item in model.JobIds)
                {

                    context.PeopleJobs.Add(new PeopleJob { PeopleId = person.Id, JobId = item });
                }
                //foreach (var item in model.PositionIds)
                //{

                //    context.PeoplePositions.Add(new PeoplePosition { PeopleId = person.Id, PositionId = item });
                //}
                if (!string.IsNullOrEmpty(model.UploadPhoto))
                {

                    byte[] bytes = Convert.FromBase64String(model.UploadPhoto.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[1]);
                    var extension = model.UploadPhoto.Contains("png") ? ".png" : ".jpg";

                    string path = _host.WebRootPath;

                    string filename = Guid.NewGuid().ToString("N") + extension;
                    if (!Directory.Exists(Path.Combine(path, "images", "person")))
                        Directory.CreateDirectory(Path.Combine(path, "images", "person"));

                    string savefile = Path.Combine(path, "images", "person", filename);
                    using (var imageFile = new FileStream(savefile, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();


                    }
                    person.Photo = filename;
                }


                await context.SaveChangesAsync();
                if (durum == 1)
                {
                    return RedirectToAction("Edit", new { id = person.Id });
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



            //ViewBag.Parties = await (from a in context.Parties
            //                         where a.Active
            //                         select new PartyListModel
            //                         {
            //                             Id = a.Id,
            //                             PartyName = a.Name,

            //                         }).ToListAsync();

            ViewBag.Genders = await context.Genders.Select(i => new Gender { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.InstitutionTypes = await context.InstitutionTypes.Select(i => new InstitutionType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            //ViewBag.Positions = await context.Positions.Select(i => new Position { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.PositionFields = await context.PositionFields.Select(i => new PositionField { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Educations = await context.Educations.Select(i => new Education { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.Id).AsNoTracking().ToListAsync();

            ViewBag.Jobs = await context.Jobs.Select(i => new Job { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();


            ViewBag.Parties = await (from a in context.Parties
                                     where a.Active
                                     select new PartyListModel
                                     {
                                         Id = a.Id,
                                         PartyName = a.Name,

                                     }).ToListAsync();

            return View(model);

        }


        public async Task<IActionResult> Edit(int id)
        {

            PersonCreateModel? model = await (from a in context.People
                                              where a.Id == id
                                              select new PersonCreateModel
                                              {
                                                  Id = a.Id,
                                                  CreatedDate = a.CreatedDate,
                                                  CvEn = a.CvEn,
                                                  CvTr = a.CvTr,
                                                  DateOfBirth = a.DateOfBirth,
                                                  EducationId = a.EducationId,
                                                  FirstName = a.FirstName,
                                                  GenderId = a.GenderId,
                                                  InstitutionTypeId = a.InstitutionTypeId,
                                                  LastName = a.LastName,
                                                  Photo = a.Photo,
                                                  PlaceOfBirth = a.PlaceOfBirth,
                                                  PositionFieldId = a.PositionFieldId,
                                                  JobIds = a.PeopleJobs.Select(i => i.JobId).ToArray(),
                                                  UpdatedDate = a.UpdatedDate,
                                                  IsActive = a.IsActive,
                                                  PartyId = a.PartyId
                                              }
                                       ).FirstOrDefaultAsync();

            BaseModel? prev = await context.People.Where(i => i.Id < id).OrderByDescending(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.FirstName + " " + i.LastName }).FirstOrDefaultAsync();
            BaseModel? next = await context.People.Where(i => i.Id > id).OrderBy(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.FirstName + " " + i.LastName }).FirstOrDefaultAsync();
            ViewBag.Prev = prev;
            ViewBag.Next = next;

            ViewBag.PeoplePositions = await (from a in context.PeoplePositions
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
                                                 InstitutionTypeId = a.InstitutionTypeId,
                                                 InstitutionName = a.InstitutionName,
                                                 PartyId = a.PartyId,
                                                 PartyName = a.PartyId.HasValue ? a.Party.ShortName : "",
                                                 Period = a.Period,
                                                 PositionId = a.PositionId,
                                                 PositionName = a.Position.NameTr,
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
                                                 SectorId = a.SectorId,
                                                 SectorName = a.Sector == null ? "" : a.Sector.NameTr
                                             }).ToListAsync();

            ViewBag.Phones = await (from a in context.PeoplePhones
                                    where a.PeopleId == id
                                    select new PhoneModel
                                    {
                                        PhoneTypeNameTr = a.Phone.PhoneType.NameTr,
                                        CommunicationTypeId = a.Phone.CommunicationTypeId,
                                        CommunicationTypeNameTr = a.Phone.CommunicationType.NameTr,
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
                                           CreatedDate = a.Address.CreatedDate,
                                           NameEn = a.Address.NameEn,
                                           NameTr = a.Address.NameTr,
                                           PostalCode = a.Address.PostalCode,
                                           Province = a.Address.Province,
                                           UpdatedDate = a.Address.UpdatedDate,
                                           DistrictId = a.Address.DistrictId,
                                           ProvinceId = a.Address.ProvinceId
                                       }).AsNoTracking().ToListAsync();

            ViewBag.Genders = await context.Genders.Select(i => new Gender { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.InstitutionTypes = await context.InstitutionTypes.Select(i => new InstitutionType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Positions = await context.Positions.Select(i => new Position { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();
            ViewBag.PositionFields = await context.PositionFields.Select(i => new PositionField { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Educations = await context.Educations.Select(i => new Education { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.Id).AsNoTracking().ToListAsync();
            ViewBag.AddressTypes = await context.AddressTypes.Select(i => new AddressType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.PhoneTypes = await context.PhoneTypes.Select(i => new PhoneType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Jobs = await context.Jobs.Select(i => new Job { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Countries = await context.Countries.OrderBy(i => i.Sira).ThenBy(i => i.NameTr).Select(i => new Country { Id = i.Id, NameTr = i.NameTr, Iso2 = i.Iso2, PhoneCode = i.PhoneCode }).AsNoTracking().ToListAsync();
            ViewBag.Provinces = await context.Provinces.Select(i => new Province { Name = i.Name, Id = i.Id }).OrderBy(i => i.Name).AsNoTracking().ToListAsync();

            ViewBag.CommunicationTypes = await context.CommunicationTypes.OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();
            ViewBag.SocialTypes = await context.SocialTypes.Select(i => new SocialType { NameTr = i.NameTr, Id = i.Id }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();
            ViewBag.Sectors = await context.Sectors.Select(i => new Sector { NameTr = i.NameTr, Id = i.Id }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();

            ViewBag.Parties = await (from a in context.Parties
                                     where a.Active
                                     orderby a.ShortName
                                     select new PartyListModel
                                     {
                                         Id = a.Id,
                                         PartyName = a.ShortName,

                                     }).ToListAsync();

            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> AddPosition(PeoplePositionModel model)
        {

            if (!model.StartDate.HasValue)
                return Ok(new { success = false, msg = "Başlangıç tarihi hatalı." });
            if (model.EndYear.HasValue && !model.EndDate.HasValue)
                return Ok(new { success = false, msg = "Bitiş tarihi hatalı." });


            try
            {
                var p = new PeoplePosition
                {
                    CreatedDate = DateTime.Now,

                    EndDate = model.EndDate,
                    InstitutionName = model.InstitutionName,
                    InstitutionTypeId = model.InstitutionTypeId,
                    PartyId = model.PartyId,
                    PeopleId = model.PeopleId,
                    PositionId = model.PositionId,
                    Period = model.Period,
                    StartDate = model.StartDate,
                    UpdatedDate = DateTime.Now,
                    ProvinceId = model.ProvinceId,
                    DistrictId = model.DistrictId,
                    EndDay = model.EndDay,
                    EndMonth = model.EndMonth,
                    EndYear = model.EndYear,
                    StartYear = model.StartYear,
                    StartMonth = model.StartMonth,
                    StartDay = model.StartDay,
                    SectorId = model.SectorId,


                };

                context.PeoplePositions.Add(p);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return Ok(new { success = false, msg = "İşlem sırasında bir hata oluştu.", err = e.ToString() });

            }
            return Ok(new { success = true });
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePosition(PeoplePositionModel model)
        {

            try
            {
                var p = await context.PeoplePositions.FirstOrDefaultAsync(i => i.Id == model.Id);




                p.EndDate = model.EndDate;
                p.InstitutionName = model.InstitutionName;
                p.InstitutionTypeId = model.InstitutionTypeId;
                p.PartyId = model.PartyId;
                p.PeopleId = model.PeopleId;
                p.PositionId = model.PositionId;
                p.Period = model.Period;
                p.StartDate = model.StartDate;
                p.UpdatedDate = DateTime.Now;
                p.ProvinceId = model.ProvinceId;
                p.DistrictId = model.DistrictId;
                p.EndDay = model.EndDay;
                p.EndMonth = model.EndMonth;
                p.EndYear = model.EndYear;
                p.StartYear = model.StartYear;
                p.StartMonth = model.StartMonth;
                p.StartDay = model.StartDay;
                p.SectorId = model.SectorId;


                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return Ok(new { success = false, msg = "İşlem sırasında bir hata oluştu.", err = e.ToString() });

            }
            return Ok(new { success = true });
        }


        [HttpGet]
        public async Task<IActionResult> GetPostionById(int id)
        {

            var model = await context.PeoplePositions.Where(i => i.Id == id)
                .Select(a => new
                {
                    PeopleId = a.PeopleId,
                    Description = a.Description,

                    Id = a.Id,

                    InstitutionTypeId = a.InstitutionTypeId,
                    InstitutionName = a.InstitutionName,
                    PartyId = a.PartyId,

                    Period = a.Period,
                    PositionId = a.PositionId,
                    PositionName = a.Position.NameTr,
                    StartDay = a.StartDay,
                    StartMonth = a.StartMonth,
                    StartYear = a.StartYear,
                    EndYear = a.EndYear,
                    EndMonth = a.EndMonth,
                    EndDay = a.EndDay,
                    DistrictId = a.DistrictId,
                    ProvinceId = a.ProvinceId,
                    SectorId = a.SectorId,

                }).FirstOrDefaultAsync();


            return Ok(new { success = true, data = model });
        }


        [HttpPost]
        public async Task<IActionResult> DeletePosition(int id)
        {

            var model = await context.PeoplePositions.FirstOrDefaultAsync(i => i.Id == id);
            if (model == null)
                return Ok(new { success = false });

            context.PeoplePositions.Remove(model);
            await context.SaveChangesAsync();

            return Ok(new { success = true });
        }
        public async Task<IActionResult> AddAddress(AddressModel model)
        {
            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    Address address = new Address
                    {
                        Address1 = model.Address1,
                        Address2 = model.Address2,
                        AddressTypeId = model.AddressTypeId,
                        CountryId = model.CountryId,
                        CreatedUserId = this.appUser.Id,
                        NameEn = model.NameEn,
                        NameTr = model.NameTr,
                        District = model.District,
                        PostalCode = model.PostalCode,
                        Province = model.Province,
                        UpdatedUserId = this.appUser.Id,
                        DistrictId = model.DistrictId,
                        ProvinceId = model.ProvinceId,
                    };
                    context.Addresses.Add(address);
                    await context.SaveChangesAsync();
                    PeopleAddress pa = new PeopleAddress { PeopleId = model.peopleId, AddressId = address.Id };
                    await context.PeopleAddresses.AddAsync(pa);
                    await context.SaveChangesAsync();
                    await trans.CommitAsync();
                    return Ok(new { success = true });

                }
                catch (Exception e)
                {
                    return Ok(new { success = false, msg = e.ToString() });
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddPhone(PhoneModel model)
        {
            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {

                    Phone phone = new Phone
                    {
                        PhoneNumber = model.PhoneNumber,
                        CommunicationTypeId = model.CommunicationTypeId,
                        PhoneTypeId = model.PhoneTypeId,
                        CreatedDate = DateTime.Now,
                        CreatedUserId = this.appUser.Id,
                        CountryCode = model.CountryCode,
                        PrefixCode = model.PrefixCode,
                        CountryId = model.CountryId
                    };
                    context.Phones.Add(phone);
                    await context.SaveChangesAsync();
                    PeoplePhone pa = new PeoplePhone { PeopleId = model.PeopleId, PhoneId = phone.Id };
                    await context.PeoplePhones.AddAsync(pa);
                    await context.SaveChangesAsync();

                    await trans.CommitAsync();

                    return Ok(new { success = true });
                }

                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    return Ok(new { success = false, msg = e.ToString() });
                }
            }

        }

        public async Task<IActionResult> AddEmail(Siyasett.Core.Emails.EmailModel model)
        {
            if (!Helpers.IsValidEmail(model.EmailAddress))
                return Ok(new { success = false, msg = "E-posta adresi geçerli değildir." });

            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    Email email = new Email
                    {

                        EmailAddress = model.EmailAddress,
                        CommunicationTypeId = model.CommunicationTypeId,

                    };
                    context.Emails.Add(email);
                    await context.SaveChangesAsync();
                    PeopleEmail pa = new PeopleEmail { PeopleId = model.ParentId, EmailId = email.Id };
                    await context.PeopleEmails.AddAsync(pa);
                    await context.SaveChangesAsync();
                    await trans.CommitAsync();
                    return Ok(new { success = true });

                }
                catch (Exception e)
                {
                    return Ok(new { success = false, msg = e.ToString() });
                }
            }

        }

        public async Task<IActionResult> AddSocialMedia(Core.SocialMedia.SocialMediaModel model)
        {
            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    SocialMedium social = new SocialMedium
                    {
                        SocialAddress = model.SocialAddress,
                        SocialTypeId = model.SocialTypeId,
                    };
                    context.SocialMedia.Add(social);
                    await context.SaveChangesAsync();
                    PeopleSocialMedium pa = new PeopleSocialMedium { PeopleId = model.ParentId, SocialMediaId = social.Id };
                    await context.PeopleSocialMedia.AddAsync(pa);
                    await context.SaveChangesAsync();
                    await trans.CommitAsync();
                    return Ok(new { success = true });

                }
                catch (Exception e)
                {
                    return Ok(new { success = false, msg = e.ToString() });
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PersonCreateModel model, int durum)
        {

            if (ModelState.IsValid)
            {
                using (var trans = await context.Database.BeginTransactionAsync())
                {
                    var person = await context.People.FirstOrDefaultAsync(i => i.Id == id);
                    if (person == null)
                        return RedirectToAction("Index");

                    person.FirstName = model.FirstName;
                    person.LastName = model.LastName;
                    person.PartyId = model.PartyId;
                    person.InstitutionTypeId = model.InstitutionTypeId;
                    person.FirstNameNormalize = Helpers.Normalize(model.FirstName);
                    person.LastNameNormalize = Helpers.Normalize(model.LastName);

                    person.GenderId = model.GenderId;
                    person.PositionFieldId = model.PositionFieldId;
                    person.UpdatedDate = DateTime.Now;
                    person.UpdatedUserId = this.appUser.Id;
                    person.EducationId = model.EducationId;
                    person.CvEn = model.CvEn;
                    person.CvTr = model.CvTr;
                    person.IsActive = model.IsActive;
                    person.DateOfBirth = model.DateOfBirth;
                    person.PlaceOfBirth = model.PlaceOfBirth;
                    //positions
                    //var positions = await context.PeoplePositions.Where(i => i.PeopleId == id).ToListAsync();
                    //if (positions.Count > 0)
                    //{
                    //    context.PeoplePositions.RemoveRange(positions.Where(i => !model.PositionIds.Contains(i.PositionId)).ToList());
                    //}

                    //if (model.PositionIds.Length > 0)
                    //    foreach (var item in model.PositionIds)
                    //    {
                    //        if (!positions.Any(i => i.PositionId == item))
                    //            context.PeoplePositions.Add(new PeoplePosition { PeopleId = id, PositionId = item });
                    //    }

                    //jobs
                    var jobs = await context.PeopleJobs.Where(i => i.PeopleId == id).ToListAsync();
                    if (jobs.Count > 0)
                    {
                        context.PeopleJobs.RemoveRange(jobs.Where(i => !model.JobIds.Contains(i.JobId)).ToList());
                    }

                    if (model.JobIds.Length > 0)
                        foreach (var item in model.JobIds)
                        {
                            if (!jobs.Any(i => i.JobId == item))
                                context.PeopleJobs.Add(new PeopleJob { PeopleId = id, JobId = item });
                        }

                    if (!string.IsNullOrEmpty(model.UploadPhoto))
                    {

                        byte[] bytes = Convert.FromBase64String(model.UploadPhoto.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[1]);
                        var extension = model.UploadPhoto.Contains("png") ? ".png" : ".jpg";

                        string path = _host.WebRootPath;

                        string filename = Guid.NewGuid().ToString("N") + extension;
                        if (!Directory.Exists(Path.Combine(path, "images", "person")))
                            Directory.CreateDirectory(Path.Combine(path, "images", "person"));

                        string savefile = Path.Combine(path, "images", "person", filename);
                        using (var imageFile = new FileStream(savefile, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();


                        }
                        person.Photo = filename;
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


            ViewBag.PeoplePositions = await (from a in context.PeoplePositions
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
                                                 InstitutionTypeId = a.InstitutionTypeId,
                                                 InstitutionName = a.InstitutionName,
                                                 PartyId = a.PartyId,
                                                 PartyName = a.PartyId.HasValue ? a.Party.ShortName : "",
                                                 Period = a.Period,
                                                 PositionId = a.PositionId,
                                                 PositionName = a.Position.NameTr,
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
                                             }).ToListAsync();

            ViewBag.Phones = await (from a in context.PeoplePhones
                                    where a.PeopleId == id
                                    select new PhoneModel
                                    {
                                        PhoneTypeNameTr = a.Phone.PhoneType.NameTr,
                                        CommunicationTypeId = a.Phone.CommunicationTypeId,
                                        CommunicationTypeNameTr = a.Phone.CommunicationType.NameTr,
                                        PhoneId = a.Phone.Id,
                                        Id = a.Id,
                                        PhoneTypeId = a.Phone.PhoneTypeId,
                                        CountryCode = a.Phone.CountryCode,
                                        PrefixCode = a.Phone.PrefixCode,
                                        PhoneNumber = a.Phone.PhoneNumber,

                                    }).ToListAsync();

            ViewBag.Emails = await context.PeopleEmails.Where(a => a.PeopleId == id)
                .Select(i => new EmailModel
                {
                    CommunicationTypeId = i.Email.CommunicationTypeId,
                    CommunicationTypeNameTr = i.Email.CommunicationType.NameTr,
                    Id = i.EmailId,
                    ParentId = i.PeopleId,
                    EmailAddress = i.Email.EmailAddress,

                }).ToListAsync();



            ViewBag.SocialAdress = context.PeopleSocialMedia.Where(a => a.PeopleId == id)
                .Select(i => new SocialMediaModel
                {
                    Id = i.SocialMediaId,
                    ParentId = i.PeopleId,
                    SocialAddress = i.SocialMedia.SocialAddress,
                    SocialTypeId = i.SocialMedia.SocialTypeId,
                    SocialTypeNameEn = i.SocialMedia.SocialType.NameEn,
                    SocialTypeNameTr = i.SocialMedia.SocialType.NameTr,

                }).ToList();


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
                                           CreatedDate = a.Address.CreatedDate,
                                           NameEn = a.Address.NameEn,
                                           NameTr = a.Address.NameTr,
                                           PostalCode = a.Address.PostalCode,
                                           Province = a.Address.Province,
                                           UpdatedDate = a.Address.UpdatedDate,
                                           DistrictId = a.Address.DistrictId,
                                           ProvinceId = a.Address.ProvinceId
                                       }).ToListAsync();


            ViewBag.Genders = await context.Genders.Select(i => new Gender { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.InstitutionTypes = await context.InstitutionTypes.Select(i => new InstitutionType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Positions = await context.Positions.Select(i => new Position { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();

            ViewBag.PositionFields = await context.PositionFields.Select(i => new PositionField { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Educations = await context.Educations.Select(i => new Education { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.Id).AsNoTracking().ToListAsync();
            ViewBag.AddressTypes = await context.AddressTypes.Select(i => new AddressType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.PhoneTypes = await context.PhoneTypes.Select(i => new PhoneType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Jobs = await context.Jobs.Select(i => new Job { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Countries = await context.Countries.OrderBy(i => i.Sira).ThenBy(i => i.NameTr).Select(i => new Country { Id = i.Id, NameTr = i.NameTr, Iso2 = i.Iso2, PhoneCode = i.PhoneCode }).AsNoTracking().ToListAsync();
            ViewBag.Provinces = await context.Provinces.Select(i => new Province { Name = i.Name, Id = i.Id }).OrderBy(i => i.Name).AsNoTracking().ToListAsync();

            ViewBag.CommunicationTypes = await context.CommunicationTypes.OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();
            ViewBag.SocialTypes = await context.SocialTypes.Select(i => new SocialType { NameTr = i.NameTr, Id = i.Id }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();
            ViewBag.Sectors = await context.Sectors.Select(i => new Sector { NameTr = i.NameTr, Id = i.Id }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();

            ViewBag.Parties = await (from a in context.Parties
                                     where a.Active
                                     orderby a.ShortName
                                     select new PartyListModel
                                     {
                                         Id = a.Id,
                                         PartyName = a.ShortName,

                                     }).ToListAsync();

            return View(model);

        }


        [HttpPost]
        public async Task<IActionResult> UploadAttachment([FromForm] int peopleId, [FromForm] string nameTr, [FromForm] string nameEn)
        {

            string path = _host.WebRootPath;
            string saveFilename = Guid.NewGuid().ToString("N");
            if (Request.Form.Files.Count == 0)
                return Ok(new { success = false, msg = "Dosya bulunmuyor" });

            var formFile = Request.Form.Files[0];
            string extension = System.IO.Path.GetExtension(formFile.FileName);

            string fname = System.IO.Path.Combine(path, "attachments", saveFilename + extension);
            using (var stream = System.IO.File.Create(fname))
            {
                await formFile.CopyToAsync(stream);
            }

            using (var trans = await context.Database.BeginTransactionAsync())
            {
                Attachment a = new Attachment
                {
                    ContentType = formFile.ContentType,
                    CreatedDate = DateTime.Now,
                    FileName = saveFilename + extension,
                    FileSize = formFile.Length,
                    OriginalFileName = formFile.FileName,
                    UpdatedDate = DateTime.Now,
                    NameTr = nameTr,
                    NameEn = nameEn
                };
                await context.Attachments.AddAsync(a);
                await context.SaveChangesAsync();

                PeopleAttachment pa = new PeopleAttachment
                {
                    AttachmentId = a.Id,
                    PeopleId = peopleId
                };
                await context.PeopleAttachments.AddAsync(pa);
                await context.SaveChangesAsync();

                await trans.CommitAsync();


            }

            return Ok(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAttachment(int id)
        {
            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var pa = await context.PeopleAttachments.FirstOrDefaultAsync(i => i.AttachmentId == id);
                    var a = await context.Attachments.FirstOrDefaultAsync(i => i.Id == id);
                    if (a == null || pa == null)
                        return Ok(new { success = false, msg = "Belirtilen ekler bulunamadı" });

                    context.PeopleAttachments.Remove(pa);
                    context.Attachments.Remove(a);

                    string path = _host.WebRootPath;
                    string fname = System.IO.Path.Combine(path, "attachments", a.FileName);
                    if (System.IO.File.Exists(fname))
                        System.IO.File.Delete(fname);

                    await context.SaveChangesAsync();

                    await trans.CommitAsync();


                    return Ok(new { success = true });

                }
                catch (Exception e)
                {
                    return Ok(new { success = false, msg = e.ToString() });
                }
            }

        }



        public async Task<IActionResult> DeleteAdress(int id)
        {
            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var peopleadress = context.PeopleAddresses.FirstOrDefault(a => a.AddressId == id);
                    context.PeopleAddresses.Remove(peopleadress);
                    var adress = context.Addresses.FirstOrDefault(a => a.Id == id);
                    context.Addresses.Remove(adress);
                    await context.SaveChangesAsync();
                    await trans.CommitAsync();
                    return Ok(new { success = true });

                }
                catch (Exception e)
                {
                    return Ok(new { success = false, msg = e.ToString() });
                }
            }

        }

        public async Task<IActionResult> DeleteEmail(int id)
        {
            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {

                    var peopleEmail = context.PeopleEmails.FirstOrDefault(a => a.EmailId == id);
                    context.PeopleEmails.Remove(peopleEmail);
                    var email = context.Emails.FirstOrDefault(a => a.Id == id);
                    context.Emails.Remove(email);

                    await context.SaveChangesAsync();
                    await trans.CommitAsync();
                    return Ok(new { success = true });

                }
                catch (Exception e)
                {
                    return Ok(new { success = false, msg = e.ToString() });
                }
            }

        }
        public async Task<IActionResult> DeletePhone(int id)
        {
            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var peoplephone = context.PeoplePhones.FirstOrDefault(a => a.Id == id);
                    if (peoplephone == null)
                        return Ok(new { success = false, msg = "İlgili telefon numarası bulunamdı" });

                    var phone = context.Phones.FirstOrDefault(a => a.Id == peoplephone.PhoneId);
                    context.PeoplePhones.Remove(peoplephone);
                    context.Phones.Remove(phone);
                    await context.SaveChangesAsync();
                    await trans.CommitAsync();
                    return Ok(new { success = true });

                }
                catch (Exception e)
                {
                    return Ok(new { success = false, msg = e.ToString() });
                }
            }

        }



        public async Task<IActionResult> DeleteSocialMedia(int id)
        {

            try
            {
                var peoplesocial = context.PeopleSocialMedia.FirstOrDefault(a => a.SocialMediaId == id);
                context.PeopleSocialMedia.Remove(peoplesocial);
                var social = context.SocialMedia.FirstOrDefault(a => a.Id == id);
                context.SocialMedia.Remove(social);
                await context.SaveChangesAsync();

                return Ok(new { success = true });

            }
            catch (Exception e)
            {
                return Ok(new { success = false, msg = e.ToString() });
            }


        }
        public async Task<IActionResult> EditAdressGetRecord(int id)
        {

            try
            {
                var adress = context.Addresses.FirstOrDefault(a => a.Id == id);
                return Ok(adress);

            }
            catch (Exception e)
            {
                return Ok(new { success = false, msg = e.ToString() });
            }


        }

        public async Task<IActionResult> UpdateAddress(AddressModel model)
        {

            try
            {
                Address address = context.Addresses.FirstOrDefault(a => a.Id == model.Id);

                address.Address1 = model.Address1;
                address.Address2 = model.Address2;
                address.AddressTypeId = model.AddressTypeId;
                address.CountryId = model.CountryId;
                address.NameEn = model.NameEn;
                address.NameTr = model.NameTr;
                address.District = model.District;
                address.PostalCode = model.PostalCode;
                address.Province = model.Province;
                address.UpdatedUserId = this.appUser.Id;
                address.ProvinceId = model.ProvinceId;
                address.DistrictId = model.DistrictId;
                context.Addresses.Update(address);
                await context.SaveChangesAsync();

                return Ok(new { success = true });

            }
            catch (Exception e)
            {
                return Ok(new { success = false, msg = e.ToString() });
            }


        }

        [HttpGet]
        public async Task<IActionResult> GetEmail(int id)
        {


            var email = await context.PeopleEmails.Where(i => i.EmailId == id)
                .Select(i => new EmailModel
                {
                    CommunicationTypeId = i.Email.CommunicationTypeId,
                    CommunicationTypeNameEn = i.Email.CommunicationType.NameEn,
                    CommunicationTypeNameTr = i.Email.CommunicationType.NameTr,
                    ParentId = i.PeopleId,
                    Id = i.EmailId,
                    EmailAddress = i.Email.EmailAddress
                }).FirstOrDefaultAsync();


            return Ok(email);




        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmail(Siyasett.Core.Emails.EmailModel model)
        {
            if (!Helpers.IsValidEmail(model.EmailAddress))
                return Ok(new { success = false, msg = "E-posta adresi geçerli değildir." });


            try
            {
                Email email = context.Emails.FirstOrDefault(a => a.Id == model.Id);
                email.EmailAddress = model.EmailAddress;
                email.CommunicationTypeId = model.CommunicationTypeId;


                context.Emails.Update(email);
                await context.SaveChangesAsync();

                return Ok(new { success = true });

            }
            catch (Exception e)
            {
                return Ok(new { success = false, msg = e.ToString() });
            }


        }

        [HttpGet]
        public async Task<IActionResult> EditPhoneGetRecord(int id)
        {

            try
            {
                var phone = context.Phones.FirstOrDefault(a => a.Id == id);
                return Ok(phone);

            }
            catch (Exception e)
            {
                return Ok(new { success = false, msg = e.ToString() });
            }


        }

        [HttpPost]
        public async Task<IActionResult> UpdatePhone(PhoneModel model)
        {

            try
            {
                Phone phone = context.Phones.FirstOrDefault(a => a.Id == model.Id);

                phone.UpdatedDate = DateTime.Now;
                phone.UpdatedUserId = this.appUser.Id;
                phone.CountryCode = model.CountryCode;
                phone.PrefixCode = model.PrefixCode;
                phone.PhoneNumber = model.PhoneNumber;
                phone.PhoneTypeId = model.PhoneTypeId;
                phone.CommunicationTypeId = model.CommunicationTypeId;

                context.Phones.Update(phone);
                await context.SaveChangesAsync();

                return Ok(new { success = true });

            }
            catch (Exception e)
            {
                return Ok(new { success = false, msg = e.ToString() });
            }


        }


        [HttpGet]
        public async Task<IActionResult> EditSocialMediaGetRecord(int id)
        {

            try
            {
                var social = context.SocialMedia.FirstOrDefault(a => a.Id == id);
                return Ok(social);

            }
            catch (Exception e)
            {
                return Ok(new { success = false, msg = e.ToString() });
            }


        }

        [HttpPost]
        public async Task<IActionResult> UpdateSocialMedia(SocialMediaModel model)
        {

            try
            {
                SocialMedium social = context.SocialMedia.FirstOrDefault(a => a.Id == model.Id);
                social.SocialAddress = model.SocialAddress;
                social.SocialTypeId = model.SocialTypeId;
                context.SocialMedia.Update(social);

                await context.SaveChangesAsync();

                return Ok(new { success = true });

            }
            catch (Exception e)
            {
                return Ok(new { success = false, msg = e.ToString() });
            }


        }
    }
}
