using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using NetTopologySuite.GeometriesGraph;
using NuGet.Versioning;
using Siyasett.Core;
using Siyasett.Core.Addresses;
using Siyasett.Core.Attachments;
using Siyasett.Core.Emails;
using Siyasett.Core.Extensions;
using Siyasett.Core.Party;
using Siyasett.Core.People;
using Siyasett.Core.Phones;
using Siyasett.Core.SocialMedia;
using Siyasett.Data;
using Siyasett.Data.Data;
using Siyasett.Models;
using Siyasett.Web.Models;
using System;
using System.Drawing.Printing;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Siyasett.Web.Areas.Admin.Controllers
{
 
    public class PartyManagementController : AdminController
    {

        private readonly IWebHostEnvironment _host;
        public PartyManagementController(ILogger<AdminController> logger, IWebHostEnvironment env, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {

            _host = env;

        }

        public async Task<IActionResult> Index(int page = 1, byte sort = 0, byte desc = 0, int pagesize = 30, string query = "", string shortname = "", string name = "", string leaderName = "", string dof = "", int parliamentery = 0, int member = 0, string website = "", int active = 0, int enjoyable = 0)
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;
            string[] orderFields = new string[] { "PartyName", "PartyNameShort", "LeaderName", "Dof", "Parliamenteries", "MemberCount", "WebSite", "Active", "ParticipateElection" };
            pager.SetQuery(Request.Query);
            PartySearchModel searchModel = new PartySearchModel();
            searchModel.PartyShortName = shortname;
            searchModel.PartyName = name;
            searchModel.PartyLeader = leaderName;
            searchModel.PartyDof = dof;
            searchModel.PartyParliamentery = parliamentery;
            searchModel.PartyMember = member;
            searchModel.WebSite = website;
            searchModel.OrderBy = sort;
            searchModel.OrderByDesc = desc;
            searchModel.Active = active;
            searchModel.Enjoyable = enjoyable;




            var liste = await (from a in context.Parties
                               join l in context.People on a.LeaderPeopleId equals l.Id into l2
                               from leader in l2.DefaultIfEmpty()
                               where (string.IsNullOrEmpty(name) || (a.Name.ToUpper()).Contains(Helpers.Normalize(name)))
                               && (string.IsNullOrEmpty(shortname) || (a.ShortName.ToUpper()).Contains(Helpers.Normalize(shortname)))
                               && (string.IsNullOrEmpty(leaderName) || (a.LeaderPeople.FirstNameNormalize).Contains(Helpers.Normalize(leaderName)) || (a.LeaderPeople.LastNameNormalize).Contains(Helpers.Normalize(leaderName)))
                               && (string.IsNullOrEmpty(dof) || (a.Dof).Contains(Helpers.Normalize(dof)))
                                && (searchModel.Active == 0 || (searchModel.Active == 1 ? (a.Active == true) : (a.Active == false)))
                                && (searchModel.Enjoyable == 0 || (searchModel.Enjoyable == 1 ? (a.ParticipateElection == true) : (a.ParticipateElection == false)))
                               orderby a.Name
                               select new PartyListModel
                               {
                                   LeaderPeopleId = a.LeaderPeopleId,
                                   PartyName = a.Name,
                                   PartyNameShort = a.ShortName,
                                   Id = a.Id,
                                   LeaderName = leader == null ? "" : leader.FirstName + " " + leader.LastName,
                                   Active = a.Active,
                                   Dof = a.Dof,
                                   ParticipateElection = a.ParticipateElection,
                                   WebSite = a.WebSite,
                                   MemberCount = a.MemberCount != null ? a.MemberCount : 0,
                                   Parliamenteries = a.Parliamenteries != null ? a.Parliamenteries : 0,
                                   NameEn = a.NameEn != null ? a.NameEn : "",
                       
                               }
                      ).OrderByField(orderFields[searchModel.OrderBy], searchModel.OrderByDesc == 0).Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).AsNoTracking().ToListAsync();

            pager.Total = await (from a in context.Parties
                                 join l in context.People on a.LeaderPeopleId equals l.Id into l2
                                 from leader in l2.DefaultIfEmpty()
                                 where (string.IsNullOrEmpty(name) || (a.Name.ToUpper()).Contains(Helpers.Normalize(name)))
                                 && (string.IsNullOrEmpty(shortname) || (a.ShortName.ToUpper()).Contains(Helpers.Normalize(shortname)))
                                 && (string.IsNullOrEmpty(leaderName) || (a.LeaderPeople.FirstNameNormalize).Contains(Helpers.Normalize(leaderName)) || (a.LeaderPeople.LastNameNormalize).Contains(Helpers.Normalize(leaderName)))
                                 && (string.IsNullOrEmpty(dof) || (a.Dof).Contains(Helpers.Normalize(dof)))
                                  && (searchModel.Active == 0 || (searchModel.Active == 1 ? (a.Active == true) : (a.Active == false)))
                                  && (searchModel.Enjoyable == 0 || (searchModel.Enjoyable == 1 ? (a.ParticipateElection == true) : (a.ParticipateElection == false)))
                                 select new { a.Id }).AsNoTracking().CountAsync();
            ViewBag.SearchModel = searchModel;

            ViewBag.Pager = pager;
            return View(liste);
        }

        public async Task<IActionResult> New()
        {
            PartyCreateModel model = new PartyCreateModel();
            model.IsActive = true;

            ViewBag.People = await (from a in context.People
                                    where a.IsActive
                                    orderby a.FirstName, a.LastName
                                    select new PeopleListModel
                                    {
                                        Id = a.Id,
                                        LastName = a.LastName,
                                        FirstName = a.FirstName
                                    }).ToListAsync();

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> New(PartyCreateModel model, int durum)
        {
            Party party = new Party();
            party.Name = model.Name;
            party.LeaderPeopleId = model.LeaderPeopleId;
            party.Active = model.IsActive;
            party.Dof = model.Dof;
            party.ParticipateElection = model.IsCanJoinElection;
            party.ShortName = model.ShortName;
            party.CreatedUserId = this.appUser.Id;
            party.UpdatedUserId = this.appUser.Id;
            party.MemberCount = model.MemberCount == null ? 0 : (int)model.MemberCount;
            party.Parliamenteries = model.Parliamentarian;
            party.Closed = model.IsClosed;
            party.NameEn = model.NameEn;


            if (!string.IsNullOrEmpty(model.UploadPhoto))
            {

                byte[] bytes = Convert.FromBase64String(model.UploadPhoto.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[1]);
                var extension = model.UploadPhoto.Contains("png") ? ".png" : ".jpg";

                string path = _host.WebRootPath;

                string filename = Guid.NewGuid().ToString("N") + extension;
                if (!Directory.Exists(Path.Combine(path, "images", "parties")))
                    Directory.CreateDirectory(Path.Combine(path, "images", "parties"));

                string savefile = Path.Combine(path, "images", "parties", filename);
                using (var imageFile = new FileStream(savefile, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();


                }
                party.Logo = filename;
            }


            context.Parties.Add(party);
            context.SaveChanges();

            if (durum == 1)
            {
                return RedirectToAction("Edit", new { id = party.Id });
            }
            else if (durum == 2)
            {

                return RedirectToAction("Index");
            }
            else if (durum == 3)
            {
                return RedirectToAction("New");

            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            PartyCreateModel? model = await (from a in context.Parties
                                             where a.Id == id
                                             select new PartyCreateModel
                                             {
                                                 Id = a.Id,
                                                 CreatedDate = a.CreatedDate,
                                                 IsCanJoinElection = (bool)a.ParticipateElection,
                                                 Dof = a.Dof,
                                                 IsActive = (bool)a.Active,
                                                 Name = a.Name,
                                                 ShortName = a.ShortName,
                                                 LeaderPeopleId = a.LeaderPeopleId,
                                                 MemberCount = a.MemberCount,
                                                 Parliamentarian = a.Parliamenteries,
                                                 Photo = a.Logo,
                                                 NameEn = a.NameEn
                                             }
                                        ).FirstOrDefaultAsync();

            ViewBag.Phones = await (from a in context.PartyPhones
                                    where a.PartyId == id
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

            ViewBag.Emails = await context.PartyEmails.Where(a => a.PartyId == id)
                .Select(i => new EmailModel
                {
                    CommunicationTypeId = i.Email.CommunicationTypeId,
                    CommunicationTypeNameTr = i.Email.CommunicationType.NameTr,
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

            ViewBag.Texts = await (from a in context.PartyTexts
                                   where a.Partyid == id
                                   select new PartyTextListModel
                                   {
                                       Id = a.Id,
                                       PartyId = a.Partyid,
                                       SectorId = a.SectorId,
                                       SectorName=a.Sector.NameTr,
                                       Text = a.Text,
                                   }).ToListAsync();


            ViewBag.Prev = await context.Parties.Where(i => i.Id < id).OrderByDescending(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.Name }).FirstOrDefaultAsync();
            ViewBag.Next = await context.Parties.Where(i => i.Id > id).OrderBy(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.Name }).FirstOrDefaultAsync();

            ViewBag.AddressTypes = await context.AddressTypes.Select(i => new AddressType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.PhoneTypes = await context.PhoneTypes.Select(i => new PhoneType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Countries = await context.Countries.OrderBy(i => i.Sira).ThenBy(i => i.NameTr).Select(i => new Country { Id = i.Id, NameTr = i.NameTr, Iso2 = i.Iso2, PhoneCode = i.PhoneCode }).AsNoTracking().ToListAsync();
            ViewBag.Provinces = await context.Provinces.Select(i => new Province { Name = i.Name, Id = i.Id }).OrderBy(i => i.Name).AsNoTracking().ToListAsync();

            ViewBag.CommunicationTypes = await context.CommunicationTypes.Select(i => new CommunicationType { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();
            ViewBag.SocialTypes = await context.SocialTypes.Select(i => new SocialType { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();
            ViewBag.SectorsCombo = await context.Sectors.OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();


            ViewBag.People = await (from a in context.People
                                    where a.IsActive
                                    orderby a.FirstName, a.LastName
                                    select new PeopleListModel
                                    {
                                        Id = a.Id,
                                        LastName = a.LastName,
                                        FirstName = a.FirstName
                                    }).ToListAsync();

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PartyCreateModel model, int durum)
        {

            if (ModelState.IsValid)
            {

                var party = await context.Parties.FirstOrDefaultAsync(i => i.Id == id);
                if (party == null)
                    return RedirectToAction("Index");

                party.Name = model.Name;
                party.LeaderPeopleId = model.LeaderPeopleId;
                party.Active = model.IsActive;
                party.Dof = model.Dof;
                party.ParticipateElection = model.IsCanJoinElection;
                party.ShortName = model.ShortName;
                party.UpdatedUserId = this.appUser.Id;
                party.UpdatedDate = DateTime.Now;
                party.MemberCount = model.MemberCount == null ? 0 : (int)model.MemberCount;
                party.Parliamenteries = model.Parliamentarian;
                party.Closed = model.IsClosed;
                party.NameEn = model.NameEn;

                if (!string.IsNullOrEmpty(model.UploadPhoto))
                {

                    byte[] bytes = Convert.FromBase64String(model.UploadPhoto.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[1]);
                    var extension = model.UploadPhoto.Contains("png") ? ".png" : ".jpg";

                    string path = _host.WebRootPath;

                    string filename = Guid.NewGuid().ToString("N") + extension;
                    if (!Directory.Exists(Path.Combine(path, "images", "parties")))
                        Directory.CreateDirectory(Path.Combine(path, "images", "parties"));

                    string savefile = Path.Combine(path, "images", "parties", filename);
                    using (var imageFile = new FileStream(savefile, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();


                    }
                    party.Logo = filename;
                }


                await context.SaveChangesAsync();

                if (durum == 3)
                {
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                else if (durum == 2)
                {

                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }


            ViewBag.Prev = await context.Parties.Where(i => i.Id < id).OrderByDescending(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.Name }).FirstOrDefaultAsync();
            ViewBag.Next = await context.Parties.Where(i => i.Id > id).OrderBy(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.Name }).FirstOrDefaultAsync();


            ViewBag.Phones = await (from a in context.PartyPhones
                                    where a.PartyId == id
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

            ViewBag.Emails = await context.PartyEmails.Where(a => a.PartyId == id)
                .Select(i => new EmailModel
                {
                    CommunicationTypeId = i.Email.CommunicationTypeId,
                    CommunicationTypeNameTr = i.Email.CommunicationType.NameTr,
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

                }).ToList();


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




            ViewBag.AddressTypes = await context.AddressTypes.Select(i => new AddressType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.PhoneTypes = await context.PhoneTypes.Select(i => new PhoneType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Countries = await context.Countries.OrderBy(i => i.Sira).ThenBy(i => i.NameTr).Select(i => new Country { Id = i.Id, NameTr = i.NameTr, Iso2 = i.Iso2, PhoneCode = i.PhoneCode }).AsNoTracking().ToListAsync();
            ViewBag.Provinces = await context.Provinces.Select(i => new Province { Name = i.Name, Id = i.Id }).OrderBy(i => i.Name).AsNoTracking().ToListAsync();

            ViewBag.CommunicationTypes = await context.CommunicationTypes.Select(i => new CommunicationType { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();
            ViewBag.SocialTypes = await context.SocialTypes.Select(i => new SocialType { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();

            ViewBag.SocialTypes = await context.SocialTypes.OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();

            ViewBag.People = await (from a in context.People
                                    where a.IsActive
                                    orderby a.FirstName, a.LastName
                                    select new PeopleListModel
                                    {
                                        Id = a.Id,
                                        LastName = a.LastName,
                                        FirstName = a.FirstName
                                    }).ToListAsync();

            return View(model);

        }


        public async Task<IActionResult> AddAddress(AddressModel model, int id)
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
                    PartyAddress pa = new PartyAddress { PartyId = model.Id, AddressId = address.Id };
                    await context.PartyAddresses.AddAsync(pa);
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
        public async Task<IActionResult> AddPhone(PhoneModel model, int id)
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
                    };
                    context.Phones.Add(phone);
                    await context.SaveChangesAsync();
                    PartyPhone pa = new PartyPhone { PartyId = id, PhoneId = phone.Id };
                    await context.PartyPhones.AddAsync(pa);
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

        public async Task<IActionResult> AddEmail(EmailModel model, int id)
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
                    PartyEmail pa = new PartyEmail { PartyId = id, EmailId = email.Id };
                    await context.PartyEmails.AddAsync(pa);
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

        public async Task<IActionResult> AddSocialMedia(SocialMediaModel model, int id)
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
                    PartySocialParty pa = new PartySocialParty { PartyId = id, SocialMediaId = social.Id };
                    await context.PartySocialParties.AddAsync(pa);
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

        public async Task<IActionResult> AddText(PartyTextListModel model, int id)
        {
            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    PartyText text = new PartyText
                    {
                        Partyid = id,
                        Text = model.Text,
                        SectorId=model.SectorId
                    };
                    context.PartyTexts.Add(text);
                    await context.SaveChangesAsync();

                    //foreach (var item in model.SectorId)
                    //{
                    //    PartyTextsSector partyTextsSector = new PartyTextsSector
                    //    {
                    //        PartyTextid = text.Id,
                    //        Sectorid = item,
                    //    };
                    //    context.PartyTextsSectors.Add(partyTextsSector);
                    //}
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
                    var partyaddress = context.PartyAddresses.FirstOrDefault(a => a.AddressId == id);
                    context.PartyAddresses.Remove(partyaddress);
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

                    var partyemail = context.PartyEmails.FirstOrDefault(a => a.EmailId == id);
                    context.PartyEmails.Remove(partyemail);
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
                    var partyphone = context.PartyPhones.FirstOrDefault(a => a.Id == id);
                    if (partyphone == null)
                        return Ok(new { success = false, msg = "İlgili telefon kaydı bulunamadı" });

                    var phone = context.Phones.FirstOrDefault(a => a.Id == partyphone.PhoneId);
                    context.PartyPhones.Remove(partyphone);

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
                var partysocial = context.PartySocialParties.FirstOrDefault(a => a.SocialMediaId == id);
                context.PartySocialParties.Remove(partysocial);
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

        public async Task<IActionResult> DeleteText(int id)
        {
            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var textsectors = context.PartyTextsSectors.Where(a => a.PartyTextid == id).ToList();
                    foreach (var textsector in textsectors)
                    {
                        var asd = context.PartyTextsSectors.FirstOrDefault(c => c.Id == textsector.Id);
                        context.PartyTextsSectors.Remove(asd);
                        await context.SaveChangesAsync();

                    }

                    var text = context.PartyTexts.FirstOrDefault(a => a.Id == id);
                    context.PartyTexts.Remove(text);
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

        public async Task<IActionResult> EditTextGetRecord(int id)
        {

            try
            {
                var adress = await (from a in context.PartyTexts
                                    where a.Id == id
                                    select new PartyTextListModel
                                    {
                                        Id = a.Id,
                                        PartyId = a.Partyid,
                                        SectorId = a.SectorId,
                                        SectorName=a.Sector.NameTr,
                                        Text = a.Text,
                                    }).ToListAsync();
                return Ok(adress);

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


            var email = await context.PartyEmails.Where(i => i.EmailId == id)
                .Select(i => new EmailModel
                {
                    CommunicationTypeId = i.Email.CommunicationTypeId,
                    CommunicationTypeNameEn = i.Email.CommunicationType.NameEn,
                    CommunicationTypeNameTr = i.Email.CommunicationType.NameTr,
                    ParentId = i.PartyId,
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
                var phone = context.PartyPhones.Where(i => i.Id == id).Select(i => new Phone
                {
                    PhoneTypeId = i.Phone.PhoneTypeId,
                    Id = i.Phone.Id,
                    PrefixCode = i.Phone.PrefixCode,
                    CountryId = i.Phone.CountryId,
                    CountryCode = i.Phone.CountryCode,
                    PhoneNumber = i.Phone.PhoneNumber,
                    CommunicationTypeId = i.Phone.CommunicationTypeId,

                }).FirstOrDefault();
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
                var partyphone = await context.PartyPhones.FirstOrDefaultAsync(i => i.Id == model.Id);
                Phone phone = context.Phones.FirstOrDefault(a => a.Id == partyphone.PhoneId);

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
                if (social == null)
                    return Ok(new { success = false, msg = "Sosyal medya linki bulunmadı" });

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


        [HttpPost]
        public async Task<IActionResult> UpdateText(PartyTextListModel model)
        {

            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    PartyText text = context.PartyTexts.FirstOrDefault(a => a.Id == model.Id);
                    if (text == null)
                        return Ok(new { success = false, msg = "Sosyal medya linki bulunmadı" });

                    text.Text = model.Text;
                    text.Partyid = model.PartyId;
                    text.SectorId = model.SectorId;
                    context.PartyTexts.Update(text);
                    await context.SaveChangesAsync();

                    //var silList = context.PartyTextsSectors.Where(a => a.PartyTextid == model.Id).ToList();

                    //foreach (var item in silList)
                    //{
                    //    context.PartyTextsSectors.Remove(item);
                    //    await context.SaveChangesAsync();
                    //}


                    //foreach (var item in model.SectorId)
                    //{
                    //    PartyTextsSector partyTextsSector = new PartyTextsSector
                    //    {
                    //        PartyTextid = text.Id,
                    //        Sectorid = item,
                    //    };
                    //    context.PartyTextsSectors.Add(partyTextsSector);
                    //    await context.SaveChangesAsync();

                    //}
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
        public async Task<IActionResult> UploadAttachment([FromForm] int partyId, [FromForm] string nameTr, [FromForm] string nameEn)
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

                PartyAttachment pa = new PartyAttachment
                {
                    AttachmentId = a.Id,
                    PartyId = partyId
                };
                await context.PartyAttachments.AddAsync(pa);
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
                    var pa = await context.PartyAttachments.FirstOrDefaultAsync(i => i.AttachmentId == id);
                    var a = await context.Attachments.FirstOrDefaultAsync(i => i.Id == id);
                    if (a == null || pa == null)
                        return Ok(new { success = false, msg = "Belirtilen ekler bulunamadı" });

                    context.PartyAttachments.Remove(pa);
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


    }
}
