using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siyasett.Core.Addresses;
using Siyasett.Core.Attachments;
using Siyasett.Core.Emails;
using Siyasett.Core.Party;
using Siyasett.Core.People;
using Siyasett.Core.Phones;
using Siyasett.Core.SocialMedia;
using Siyasett.Data;
using Siyasett.Data.Data;

using Siyasett.Web.Models;

namespace Siyasett.Web.Areas.Admin.Controllers
{

    public class PartialsController : AdminController
    {
        private readonly IWebHostEnvironment _host;
        public PartialsController(ILogger<PartialsController> logger, IWebHostEnvironment env, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {
            _host = env;
        }
        public IActionResult AddressList(List<AddressModel> model)
        {
            return PartialView("PartialAddressList", model);
        }

        public async Task<IActionResult> PeopleAddresses(int id)
        {
            var model = await (from a in context.PeopleAddresses
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
            return PartialView("PartialAddressList", model);
        }

        public async Task<IActionResult> PartyAddresses(int id)
        {
            var model = await (from a in context.PartyAddresses
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
            return PartialView("PartialAddressList", model);
        }

        public async Task<IActionResult> CompanyAddresses(int id)
        {
            var model = await (from a in context.CompanyAddresses
                               where a.CompanyId == id
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
            return PartialView("PartialAddressList", model);
        }
        public IActionResult PhoneList(List<Core.Phones.PhoneModel> model)
        {
            return PartialView("PartialPhoneList", model);
        }

        public async Task<IActionResult> PeoplePositions(int id)
        {
            var model = await (from a in context.PeoplePositions
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
            return PartialView("PartialPositionList", model);
        }

        public async Task<IActionResult> PeoplePhones(int id)
        {
            var model = await (from a in context.PeoplePhones
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
            return PartialView("PartialPhoneList", model);
        }

        public async Task<IActionResult> PartyPhones(int id)
        {
            var model = await (from a in context.PartyPhones
                               where a.PartyId == id
                               select new Core.Phones.PhoneModel
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
            return PartialView("PartialPhoneList", model);
        }

        public async Task<IActionResult> CompanyPhones(int id)
        {
            var model = await (from a in context.CompanyPhones
                               where a.CompanyId == id
                               select new Core.Phones.PhoneModel
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
            return PartialView("PartialPhoneList", model);
        }
        public IActionResult EmailList(List<Email> model)
        {
            return PartialView("PartialEmailList", model);
        }
        public async Task<IActionResult> PeopleEmails(int id)
        {
            var emaillist = await context.PeopleEmails.Where(a => a.PeopleId == id)
                .Select(i => new EmailModel
                {
                    EmailAddress = i.Email.EmailAddress,
                    ParentId = i.PeopleId,
                    CommunicationTypeId = i.Email.CommunicationTypeId,
                    CommunicationTypeNameTr = i.Email.CommunicationType.NameTr,
                    CommunicationTypeNameEn = i.Email.CommunicationType.NameEn,
                    Id = i.Email.Id,

                }).ToListAsync();


            return PartialView("PartialEmailList", emaillist);
        }

        public async Task<IActionResult> PartyEmails(int id)
        {
            var emaillist = await context.PartyEmails.Where(a => a.PartyId == id)
                .Select(i => new EmailModel
                {
                    EmailAddress = i.Email.EmailAddress,
                    ParentId = i.PartyId,
                    CommunicationTypeId = i.Email.CommunicationTypeId,
                    CommunicationTypeNameTr = i.Email.CommunicationType.NameTr,
                    CommunicationTypeNameEn = i.Email.CommunicationType.NameEn,
                    Id = i.Email.Id,

                }).ToListAsync();


            return PartialView("PartialEmailList", emaillist);
        }

        public async Task<IActionResult> CompanyEmails(int id)
        {
            var emaillist = await context.CompanyEmails.Where(a => a.CompanyId == id)
                .Select(i => new EmailModel
                {
                    EmailAddress = i.Email.EmailAddress,
                    ParentId = i.CompanyId,
                    CommunicationTypeId = i.Email.CommunicationTypeId,
                    CommunicationTypeNameTr = i.Email.CommunicationType.NameTr,
                    CommunicationTypeNameEn = i.Email.CommunicationType.NameEn,
                    Id = i.Email.Id,

                }).ToListAsync();


            return PartialView("PartialEmailList", emaillist);
        }
        public async Task<IActionResult> PeopleAttachments(int id)
        {
            var attachments = await context.PeopleAttachments.Where(a => a.PeopleId == id)
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
                    ParentId = i.PeopleId

                }).ToListAsync();
            return PartialView("PartialAttachments", attachments);
        }

        public async Task<IActionResult> PartyAttachments(int id)
        {
            var attachments = await context.PartyAttachments.Where(a => a.PartyId == id)
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
            return PartialView("PartialAttachments", attachments);
        }


        public async Task<IActionResult> CompanyAttachments(int id)
        {
            var attachments = await context.CompanyAttachments.Where(a => a.CompanyId == id)
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
                    ParentId = i.CompanyId

                }).ToListAsync();
            return PartialView("PartialAttachments", attachments);
        }
        public IActionResult SocialMediaList(List<Core.SocialMedia.SocialMediaModel> model)
        {
            return PartialView("PartialSocialMediaList", model);
        }

        public async Task<IActionResult> PeopleSocialMedias(int id)
        {

            var model = await context.PeopleSocialMedia.Where(a => a.PeopleId == id)
                .Select(i => new SocialMediaModel
                {
                    Id = i.SocialMediaId,
                    ParentId = i.PeopleId,
                    SocialAddress = i.SocialMedia.SocialAddress,
                    SocialTypeId = i.SocialMedia.SocialTypeId,
                    SocialTypeNameEn = i.SocialMedia.SocialType.NameEn,
                    SocialTypeNameTr = i.SocialMedia.SocialType.NameTr,

                }).ToListAsync();


            return PartialView("PartialSocialMediaList", model);
        }
        public async Task<IActionResult> PartySocialMedias(int id)
        {

            var model = await context.PartySocialParties.Where(a => a.PartyId == id)
                .Select(i => new SocialMediaModel
                {
                    Id = i.SocialMediaId,
                    ParentId = i.PartyId,
                    SocialAddress = i.SocialMedia.SocialAddress,
                    SocialTypeId = i.SocialMedia.SocialTypeId,
                    SocialTypeNameEn = i.SocialMedia.SocialType.NameEn,
                    SocialTypeNameTr = i.SocialMedia.SocialType.NameTr,

                }).ToListAsync();


            return PartialView("PartialSocialMediaList", model);
        }

        public async Task<IActionResult> CompanySocialMedias(int id)
        {

            var model = await context.CompanySocialMedias.Where(a => a.CompanyId == id)
                .Select(i => new SocialMediaModel
                {
                    Id = i.SocialMediaId,
                    ParentId = i.CompanyId,
                    SocialAddress = i.SocialMedia.SocialAddress,
                    SocialTypeId = i.SocialMedia.SocialTypeId,
                    SocialTypeNameEn = i.SocialMedia.SocialType.NameEn,
                    SocialTypeNameTr = i.SocialMedia.SocialType.NameTr,

                }).ToListAsync();


            return PartialView("PartialSocialMediaList", model);
        }

        public async Task<IActionResult> PartyTexts(int id)
        {

            var model = await context.PartyTexts.Where(a => a.Partyid == id)
                .Select(i => new PartyTextListModel
                {
                   Id=i.Id,
                   PartyId=i.Partyid,
                    //Sector=context.Sectors.FirstOrDefault(c=>c.Id==i.Sectorid).NameTr,
                    //SectorId = i.PartyTextsSectors.Select(i => i.Sectorid).ToArray(),
                    Text =i.Text,

                }).ToListAsync();

            ViewBag.SectorsCombo = await context.Sectors.OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();


            return PartialView("PartialTextsList", model);
        }

        public async Task<IActionResult> AddressEntry()
        {
            ViewBag.Countries = await context.Countries.AsNoTracking().ToListAsync();
            return PartialView();
        }

        public List<District> GetDistricts(int id)
        {
            return context.Districts.Where(i => i.ProvinceId == id).OrderBy(i => i.Name).Select(i => new District { Id = i.Id, Name = i.Name, ProvinceId = i.ProvinceId }).ToList();
        }
    }
}
