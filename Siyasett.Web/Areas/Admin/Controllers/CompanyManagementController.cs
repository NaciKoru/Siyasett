using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siyasett.Core.Addresses;
using Siyasett.Core;
using Siyasett.Core.Company;
using Siyasett.Core.Party;
using Siyasett.Core.People;
using Siyasett.Core.Phones;
using Siyasett.Core.SocialMedia;
using Siyasett.Data;
using Siyasett.Data.Data;
using Siyasett.Models;
using Siyasett.Web.Models;
using Siyasett.Core.Emails;
using NuGet.Protocol.Plugins;
using Siyasett.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Siyasett.Web.Areas.Admin.Controllers
{
    
    public class CompanyManagementController : AdminController
    {
        private readonly IWebHostEnvironment _host;
        public CompanyManagementController(ILogger<AdminController> logger, IWebHostEnvironment env, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {

            _host = env;

        }

        public async Task<IActionResult> Index(int page = 1, byte sort = 0, byte desc = 0, int pagesize = 30, string name = "", string shortname = "", string manager = "", string website = "", string date = "")
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;
            string[] orderFields = new string[] { "Name", "ShortName", "LeaderName", "Dof", "WebAdress" };
            pager.SetQuery(Request.Query);
            CompanySearchModel searchModel = new CompanySearchModel();
            searchModel.CompanyName = name;
            searchModel.CompanyShortName = shortname;
            searchModel.CompanyManager = manager;
            searchModel.CompanyWebSite = website;
            searchModel.CompanyDate = date;
            searchModel.OrderBy = sort;
            searchModel.OrderByDesc = desc;


            var liste = await (from a in context.Companies
                               join l in context.People on a.CompanyLeaderId equals l.Id into l2
                               from leader in l2.DefaultIfEmpty()
                               where (string.IsNullOrEmpty(name) || (a.Name.ToUpper()).Contains(Helpers.Normalize(name)))
                               && (string.IsNullOrEmpty(shortname) || (a.ShortName.ToUpper()).Contains(Helpers.Normalize(shortname)))
                               && (string.IsNullOrEmpty(manager) || (leader.FirstName.ToUpper()).Contains(manager.ToUpper()) || (leader.LastName.ToUpper()).Contains(manager.ToUpper()))
                               && (string.IsNullOrEmpty(website) || (a.WebAdress.ToUpper()).Contains(Helpers.Normalize(website)))
                               && (string.IsNullOrEmpty(date) || (a.Dof.ToUpper()).Contains(Helpers.Normalize(date)))
                              
                               select new CompanyListModel
                               {
                                   LeaderPeopleId = a.CompanyLeaderId,
                                   Name = a.Name,
                                   ShortName = a.ShortName,
                                   Id = a.Id,
                                   LeaderName = leader == null ? "" : leader.FirstName + " " + leader.LastName,
                                   Dof = a.Dof,
                                   WebAdress = a.WebAdress,
                               }
                      )
                      .OrderByField(orderFields[searchModel.OrderBy], searchModel.OrderByDesc == 0)
                      .AsNoTracking().Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).ToListAsync();

            pager.Total = await (from a in context.Companies
                                 join l in context.People on a.CompanyLeaderId equals l.Id into l2
                                 from leader in l2.DefaultIfEmpty()
                                 where (string.IsNullOrEmpty(name) || (a.Name.ToUpper()).Contains(Helpers.Normalize(name)))
                               && (string.IsNullOrEmpty(shortname) || (a.ShortName.ToUpper()).Contains(Helpers.Normalize(shortname)))
                               && (string.IsNullOrEmpty(manager) || (leader.FirstName.ToUpper()).Contains(manager.ToUpper()) || (leader.LastName.ToUpper()).Contains(manager.ToUpper()))
                               && (string.IsNullOrEmpty(website) || (a.WebAdress.ToUpper()).Contains(Helpers.Normalize(website)))
                               && (string.IsNullOrEmpty(date) || (a.Dof.ToUpper()).Contains(Helpers.Normalize(date)))
                                 select new { a.Id }).AsNoTracking().CountAsync();

            ViewBag.SearchModel = searchModel;
            ViewBag.Pager = pager;

            return View(liste);
        }


        public async Task<IActionResult> New()
        {
            CompanyCreateModel model = new CompanyCreateModel();
            model.Active = true;

            ViewBag.People = await (from a in context.People
                                    where a.IsActive
                                    orderby a.FirstName, a.LastName
                                    select new PeopleListModel
                                    {
                                        Id = a.Id,
                                        LastName = a.LastName,
                                        FirstName = a.FirstName
                                    }).ToListAsync();

            ViewBag.Sector = await (from a in context.Sectors
                                    orderby a.NameTr
                                    select new Sector
                                    {
                                        Id = a.Id,
                                        NameTr = a.NameTr,
                                        NameEn = a.NameEn,
                                    }).ToListAsync();


            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> New(CompanyCreateModel model, int durum)
        {

            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    Company company = new Company()
                    {
                        Name = model.Name,
                        CompanyLeaderId = model.LeaderPeopleId,
                        Active = model.Active,
                        Dof = model.Dof,
                        ShortName = model.ShortName,
                        CreatedUserId = this.appUser.Id,
                        UpdatedUserId = this.appUser.Id,
                        WebAdress = model.WebAdress,
                    };


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
                        company.Logo = filename;
                    }


                    context.Companies.Add(company);
                    context.SaveChanges();
                    if (model.SectorId != null)
                    {
                        foreach (var item in model.SectorId)
                        {
                            CompanySector sector = new()
                            {
                                CompanyId = company.Id,
                                SectorId = item,
                            };
                            context.CompanySectors.Add(sector);
                        }
                        context.SaveChanges();
                    }
                    await trans.CommitAsync();
                    if (durum == 1)
                    {
                        return RedirectToAction("Edit", new { id = company.Id });
                    }
                    else
                    {

                        return RedirectToAction("Index");
                    }

                }
                catch (Exception e)
                {
                    return RedirectToAction("Index");

                }
            }




        }

        public async Task<IActionResult> Edit(int id)
        {
            CompanyCreateModel? model = await (from a in context.Companies
                                               where a.Id == id
                                               select new CompanyCreateModel
                                               {
                                                   Id = a.Id,
                                                   CreatedDate = a.CreatedDate,
                                                   Dof = a.Dof,
                                                   Active = (bool)a.Active,
                                                   Name = a.Name,
                                                   ShortName = a.ShortName,
                                                   LeaderPeopleId = a.CompanyLeaderId,
                                                   Photo = a.Logo,
                                                   WebAdress = a.WebAdress,
                                                   SectorId = a.CompanySectors.Select(i => i.SectorId).ToArray(),
                                               }
                                        ).FirstOrDefaultAsync();

            ViewBag.Phones = await (from a in context.CompanyPhones
                                    where a.CompanyId == id
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

            ViewBag.Emails = await context.CompanyEmails.Where(a => a.CompanyId == id)
                .Select(i => new EmailModel
                {
                    CommunicationTypeId = i.Email.CommunicationTypeId,
                    CommunicationTypeNameTr = i.Email.CommunicationType.NameTr,
                    Id = i.EmailId,
                    ParentId = i.CompanyId,
                    EmailAddress = i.Email.EmailAddress,

                }).ToListAsync();



            ViewBag.SocialAdress = context.CompanySocialMedias.Where(a => a.CompanyId == id)
                .Select(i => new SocialMediaModel
                {
                    Id = i.SocialMediaId,
                    ParentId = i.CompanyId,
                    SocialAddress = i.SocialMedia.SocialAddress,
                    SocialTypeId = i.SocialMedia.SocialTypeId,
                    SocialTypeNameEn = i.SocialMedia.SocialType.NameEn,
                    SocialTypeNameTr = i.SocialMedia.SocialType.NameTr,

                }).ToList();


            ViewBag.Addresses = await (from a in context.CompanyAddresses
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

            ViewBag.Sector = await (from a in context.Sectors
                                    orderby a.NameTr
                                    select new Sector
                                    {
                                        Id = a.Id,
                                        NameTr = a.NameTr,
                                        NameEn = a.NameEn,
                                    }).ToListAsync();

            ViewBag.Prev = await context.Companies.Where(i => i.Id < id).OrderByDescending(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.Name }).FirstOrDefaultAsync();
            ViewBag.Next = await context.Companies.Where(i => i.Id > id).OrderBy(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.Name }).FirstOrDefaultAsync();


            ViewBag.AddressTypes = await context.AddressTypes.Select(i => new AddressType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.PhoneTypes = await context.PhoneTypes.Select(i => new PhoneType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Countries = await context.Countries.OrderBy(i => i.Sira).ThenBy(i => i.NameTr).Select(i => new Country { Id = i.Id, NameTr = i.NameTr, Iso2 = i.Iso2, PhoneCode = i.PhoneCode }).AsNoTracking().ToListAsync();
            ViewBag.Provinces = await context.Provinces.Select(i => new Province { Name = i.Name, Id = i.Id }).OrderBy(i => i.Name).AsNoTracking().ToListAsync();

            ViewBag.CommunicationTypes = await context.CommunicationTypes.Select(i => new CommunicationType { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();
            ViewBag.SocialTypes = await context.SocialTypes.Select(i => new SocialType { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();

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
        public async Task<IActionResult> Edit(int id, CompanyCreateModel model)
        {

            if (ModelState.IsValid)
            {
                using (var trans = await context.Database.BeginTransactionAsync())
                {
                    var company = await context.Companies.FirstOrDefaultAsync(i => i.Id == id);
                    if (company == null)
                        return RedirectToAction("Index");

                    company.Name = model.Name;
                    company.CompanyLeaderId = model.LeaderPeopleId;
                    company.Active = model.Active;
                    company.Dof = model.Dof;
                    company.ShortName = model.ShortName;
                    company.UpdatedUserId = this.appUser.Id;
                    company.UpdatedDate = DateTime.Now;
                    company.WebAdress = model.WebAdress;

                    var sectorIds = await context.CompanySectors.Where(i => i.CompanyId == id).ToListAsync();
                    if (sectorIds.Count > 0)
                    {
                        context.CompanySectors.RemoveRange(sectorIds.Where(i => !model.SectorId.Contains(i.SectorId)).ToList());
                    }

                    if (model.SectorId != null)
                        foreach (var item in model.SectorId)
                        {
                            if (!sectorIds.Any(i => i.SectorId == item))
                                context.CompanySectors.Add(new CompanySector { CompanyId = id, SectorId = item });
                        }


                    if (!string.IsNullOrEmpty(model.UploadPhoto))
                    {

                        byte[] bytes = Convert.FromBase64String(model.UploadPhoto.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[1]);
                        var extension = model.UploadPhoto.Contains("png") ? ".png" : ".jpg";

                        string path = _host.WebRootPath;

                        string filename = Guid.NewGuid().ToString("N") + extension;
                        if (!Directory.Exists(Path.Combine(path, "images", "companies")))
                            Directory.CreateDirectory(Path.Combine(path, "images", "companies"));

                        string savefile = Path.Combine(path, "images", "companies", filename);
                        using (var imageFile = new FileStream(savefile, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();


                        }
                        company.Logo = filename;
                    }

                    await context.SaveChangesAsync();
                    await trans.CommitAsync();

                }
                return RedirectToAction("Index");


            }


            ViewBag.Prev = await context.Companies.Where(i => i.Id < id).OrderByDescending(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.Name }).FirstOrDefaultAsync();
            ViewBag.Next = await context.Companies.Where(i => i.Id > id).OrderBy(i => i.Id).Select(i => new BaseModel { Id = i.Id, Name = i.Name }).FirstOrDefaultAsync();


            ViewBag.Phones = await (from a in context.CompanyPhones
                                    where a.CompanyId == id
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

            ViewBag.Emails = await context.CompanyEmails.Where(a => a.CompanyId == id)
                .Select(i => new EmailModel
                {
                    CommunicationTypeId = i.Email.CommunicationTypeId,
                    CommunicationTypeNameTr = i.Email.CommunicationType.NameTr,
                    Id = i.EmailId,
                    ParentId = i.CompanyId,
                    EmailAddress = i.Email.EmailAddress,

                }).ToListAsync();



            ViewBag.SocialAdress = context.CompanySocialMedias.Where(a => a.CompanyId == id)
                .Select(i => new SocialMediaModel
                {
                    Id = i.SocialMediaId,
                    ParentId = i.CompanyId,
                    SocialAddress = i.SocialMedia.SocialAddress,
                    SocialTypeId = i.SocialMedia.SocialTypeId,
                    SocialTypeNameEn = i.SocialMedia.SocialType.NameEn,
                    SocialTypeNameTr = i.SocialMedia.SocialType.NameTr,

                }).ToList();


            ViewBag.Addresses = await (from a in context.CompanyAddresses
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




            ViewBag.AddressTypes = await context.AddressTypes.Select(i => new AddressType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.PhoneTypes = await context.PhoneTypes.Select(i => new PhoneType { Id = i.Id, NameTr = i.NameTr }).AsNoTracking().ToListAsync();
            ViewBag.Countries = await context.Countries.OrderBy(i => i.Sira).ThenBy(i => i.NameTr).Select(i => new Country { Id = i.Id, NameTr = i.NameTr, Iso2 = i.Iso2, PhoneCode = i.PhoneCode }).AsNoTracking().ToListAsync();
            ViewBag.Provinces = await context.Provinces.Select(i => new Province { Name = i.Name, Id = i.Id }).OrderBy(i => i.Name).AsNoTracking().ToListAsync();

            ViewBag.CommunicationTypes = await context.CommunicationTypes.Select(i => new CommunicationType { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();
            ViewBag.SocialTypes = await context.SocialTypes.Select(i => new SocialType { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();

            ViewBag.SocialTypes = await context.SocialTypes.OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();

            ViewBag.Sector = await (from a in context.Sectors
                                    orderby a.NameTr
                                    select new Sector
                                    {
                                        Id = a.Id,
                                        NameTr = a.NameTr,
                                        NameEn = a.NameEn,
                                    }).ToListAsync();

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
                    CompanyAddress pa = new CompanyAddress { CompanyId = model.Id, AddressId = address.Id };
                    await context.CompanyAddresses.AddAsync(pa);
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
                    CompanyPhone pa = new CompanyPhone { CompanyId = id, PhoneId = phone.Id };
                    await context.CompanyPhones.AddAsync(pa);
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
                    CompanyEmail pa = new CompanyEmail { CompanyId = id, EmailId = email.Id };
                    await context.CompanyEmails.AddAsync(pa);
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
                    CompanySocialMedia pa = new CompanySocialMedia { CompanyId = id, SocialMediaId = social.Id };
                    await context.CompanySocialMedias.AddAsync(pa);
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
                    var companyAddress = context.CompanyAddresses.FirstOrDefault(a => a.AddressId == id);
                    context.CompanyAddresses.Remove(companyAddress);
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

                    var companyEmail = context.CompanyEmails.FirstOrDefault(a => a.EmailId == id);
                    context.CompanyEmails.Remove(companyEmail);
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
                    var companyPhone = context.CompanyPhones.FirstOrDefault(a => a.Id == id);
                    if (companyPhone == null)
                        return Ok(new { success = false, msg = "İlgili telefon kaydı bulunamadı" });

                    var phone = context.Phones.FirstOrDefault(a => a.Id == companyPhone.PhoneId);
                    context.CompanyPhones.Remove(companyPhone);

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
                var companySocial = context.CompanySocialMedias.FirstOrDefault(a => a.SocialMediaId == id);
                context.CompanySocialMedias.Remove(companySocial);
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


            var email = await context.CompanyEmails.Where(i => i.EmailId == id)
                .Select(i => new EmailModel
                {
                    CommunicationTypeId = i.Email.CommunicationTypeId,
                    CommunicationTypeNameEn = i.Email.CommunicationType.NameEn,
                    CommunicationTypeNameTr = i.Email.CommunicationType.NameTr,
                    ParentId = i.CompanyId,
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
                var phone = context.CompanyPhones.Where(i => i.Id == id).Select(i => new Phone
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
                var companyPhone = await context.CompanyPhones.FirstOrDefaultAsync(i => i.Id == model.Id);
                Phone phone = context.Phones.FirstOrDefault(a => a.Id == companyPhone.PhoneId);

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

        public async Task<IActionResult> Delete(int id)
        {

            using (var trans = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var delsector = context.CompanySectors.Where(a => a.CompanyId == id);
                    foreach (var item in delsector)
                    {
                        context.CompanySectors.Remove(item);
                    }
                    await context.SaveChangesAsync();

                    var deladresses = context.CompanyAddresses.Where(a => a.CompanyId == id);
                    foreach (var item in deladresses)
                    {
                        context.CompanyAddresses.Remove(item);
                    }
                    await context.SaveChangesAsync();

                    var delattach = context.CompanyAttachments.Where(a => a.CompanyId == id);
                    foreach (var item in delattach)
                    {
                        context.CompanyAttachments.Remove(item);
                    }
                    await context.SaveChangesAsync();

                    var deleail = context.CompanyEmails.Where(a => a.CompanyId == id);
                    foreach (var item in deleail)
                    {
                        context.CompanyEmails.Remove(item);
                    }
                    await context.SaveChangesAsync();

                    var delphone = context.CompanyPhones.Where(a => a.CompanyId == id);
                    foreach (var item in delphone)
                    {
                        context.CompanyPhones.Remove(item);
                    }
                    await context.SaveChangesAsync();

                    var delsoc = context.CompanySocialMedias.Where(a => a.CompanyId == id);
                    foreach (var item in delsoc)
                    {
                        context.CompanySocialMedias.Remove(item);
                    }
                    await context.SaveChangesAsync();

                    var company = context.Companies.FirstOrDefault(a => a.Id == id);
                    context.Companies.Remove(company);
                    await context.SaveChangesAsync();
                    await trans.CommitAsync();
                    return Ok();
                }
                catch
                {
                    await trans.RollbackAsync();
                    return NotFound();
                }

            }




        }

    }
}
