using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Siyasett.Core.Addresses;
using Siyasett.Core;
using Siyasett.Core.Extensions;
using Siyasett.Core.Party;
using Siyasett.Core.People;
using Siyasett.Core.Phones;
using Siyasett.Core.SocialMedia;
using Siyasett.Data;
using Siyasett.Data.Data;
using Siyasett.Models;
using Siyasett.Web.Models;
using System.Diagnostics;
using System.Drawing.Printing;
using Siyasett.Core.Emails;
using Siyasett.Core.Genel;
using Siyasett.Core.Users;
using Microsoft.AspNetCore.Localization;
using Siyasett.Web.Languages;
using System;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Siyasett.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly SignInManager<AppUser> SignInManager;
        private readonly UserManager<AppUser> UserManager;
        private readonly IWebHostEnvironment _host;
        protected readonly AppDbContext context;
        private readonly IEmailSender emailSender;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext appcontext, IEmailSender emailSender,
            ILanguageService languageService, ILocalizationService localizationService) : base(languageService, localizationService)
        {
            _host = env;
            _logger = logger;
            SignInManager = signInManager;
            UserManager = userManager;
            this.roleManager = roleManager;
            context = appcontext;
            this.emailSender = emailSender;
        }


        [HttpPost]
        public IActionResult ChangeLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Index()
        {

            //var roleAdmin = await roleManager.FindByNameAsync("Admin");
            //if (roleAdmin == null)
            //    await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));

            return RedirectToAction("Columns", "Actuals");
            //return View();
        }
        public async Task<IActionResult> TarihteBugun()
        {

            //var roleAdmin = await roleManager.FindByNameAsync("Admin");
            //if (roleAdmin == null)
            //    await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            var ay = DateTime.Now.Month;
            var gun = DateTime.Now.Day;
            var bugun = await context.Chronologies.Where(i => i.EventDate.Month == ay && i.EventDate.Day == gun)
                .Select(i => i)
                .Take(8).ToListAsync();

            return PartialView(bugun);
        }


        [Route("iletisim")]
        [Route("contact")]
        public IActionResult Contact()
        {
            return View();
        }



        [Route("hakkimizda")]
        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("sss")]
        [Route("faq")]
        public IActionResult SSS()
        {
            var faqGroups = context.FaqGroups.ToList();
            var faqs = context.Faqs.ToList();
            ViewBag.Groups = faqGroups;
            return View(faqs);
        }

        [Route("KullanimKosullari")]
        [Route("TermsConAndSec")]
        public IActionResult TermsConAndSec()
        {
            return View();
        }
        [Route("Cerezler")]
        [Route("CookiePolicy")]
        public IActionResult CookiePolicy()
        {
            
            return View();
        }
        [Route("Kvkk")]
        public IActionResult Kvkk()
        {
            
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Login")]
        public IActionResult Login(string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (Url.IsLocalUrl(ViewBag.ReturnUrl))
                    return Redirect(ViewBag.ReturnUrl);
                return RedirectToAction("Index", "Home");
            }
            ModelState.ClearValidationState("");
            LoginViewModel model = new LoginViewModel();
            model.ReturnUrl = returnUrl;
            return View(model);
        }


        [Route("Signin")]
        [HttpPost]
        public async Task<IActionResult> Signin(LoginViewModel model)
        {

            string hata = "Bir hata oluştu";
            var result = await SignInManager.PasswordSignInAsync(model.Eposta, model.Sifre, model.BeniHatirla, true);

            if (result.Succeeded)
            {


                return Ok(new { success = true });


            }
            else
            {

                if (result.IsNotAllowed)
                    hata = "Girişinize izin verilmedi. Şifremi unuttum ile yeni bir şifre vererek devam edebilirsiniz.";

                if (result.IsLockedOut)
                    hata = "Hesabınız kilitli olduğu için giriş yapamazsınız. Şifremi unuttum ile yeni bir şifre vererek devam edebilirsiniz.";

                if (result.RequiresTwoFactor)
                    hata = "E-posta hesabınız henüz doğrulanmamış. Şifremi unuttum'u tıklayıp şifrenizi değiştirdikten sonra sitemizi kullanmaya devam edebilirsiniz.";

            }

            return Ok(new { success = false, msg = hata });
        }



        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel model, string returnUrl = "")
        {
            ModelState.ClearValidationState("");

            var result = await SignInManager.PasswordSignInAsync(model.Eposta, model.Sifre, model.BeniHatirla, true);

            if (result.Succeeded)
            {

                if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");


            }
            else
            {
                if (result.IsNotAllowed)
                    ModelState.AddModelError("", "Girişinize izin verilmedi. Şifremi unuttum ile yeni bir şifre vererek devam edebilirsiniz.");

                if (result.IsLockedOut)
                    ModelState.AddModelError("", "Hesabınız kilitli olduğu için giriş yapamazsınız. Şifremi unuttum'u tıklayıp şifrenizi değiştirdikten sonra sitemizi kullanmaya devam edebilirsiniz.");

                if (result.RequiresTwoFactor)
                    ModelState.AddModelError("", "E-posta hesabınız henüz doğrulanmamış. Şifremi unuttum'u tıklayıp şifrenizi değiştirdikten sonra sitemizi kullanmaya devam edebilirsiniz.");

            }

            return View();
        }



        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index");
        }


        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {

            if (model.FirstName.Contains("http") || model.FirstName.Contains("JETZT KRYPTO") || model.FirstName.Length>50)
                ModelState.AddModelError("", "Wrong name");

            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser();
                appUser.FirstName = model.FirstName.Length > 50 ? model.FirstName.Substring(0, 50) : model.FirstName;
                appUser.LastName = model.LastName.Length > 50 ? model.LastName.Substring(0, 50) : model.LastName;
                appUser.PhoneNumber = model.PhoneNumber;
                appUser.Email = model.Email;
                appUser.EmailConfirmed = true;//daha sonra kapatılacak

                appUser.UserName = model.Email;
                var result = await UserManager.CreateAsync(appUser, model.Password);

                if (result.Succeeded)
                {
                    var membershipUser = new MembershipUser();
                    membershipUser.MshipStartdate = DateTime.Now;
                    membershipUser.UserId = appUser.Id;
                    membershipUser.MshipType = 1;
                    membershipUser.MshipEnddate=DateTime.Now.AddYears(10);
                    await context.MembershipUsers.AddAsync(membershipUser);
                    await context.SaveChangesAsync();

                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(appUser);
                    appUser.Token = token;
                    await UserManager.UpdateAsync(appUser);

                    //var callbackUrl = Url.Page("Home/SifreYenile", pageHandler: null,  values: new { code },    protocol: Request.Scheme);


                    var confirmationLink = Url.Action("EPostaOnayla", "Home", new { token = token, email = appUser.Email }, Request.Scheme);
                    TempData["Message"] = model.Email + " adresine bir doğrulama postası gönderdik. İçerikte yer alan linki tıklayarak e-posta adresinizi onaylabilirsiniz.";

                    try
                    {

                        string text = $@"<strong>Sayın {appUser.FirstName} {appUser.LastName}</strong>,
                                <p>Siyasett sayfaları üzerinden yapmış olduğunuz üyelik işleminizin tamamlanması için lütfen bu mesajımızı onaylayınız.</p>
                                <p>Onay için <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>tıklayınız</a>.</p>
                                <p>Sitemizde yapacağınız gezinti sonrasında düşüncelerinizi iletişim formunu doldurarak bizimle paylaşabilirsiniz.</p>
                                <p>Üyelik başvurusunda bulunduğunuz için teşekkür ederiz.</p>
                                <p>En iyi dileklerimizle,</p>
                                <p><strong>Siyasett Teknik Ekibi</strong></p>";

                        await emailSender.SendEmailAsync(model.Email, "E-posta Onayı", text);


                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.ToString());
                        ModelState.AddModelError("", "E-posta gönderilirken bir hata oluştu.");
                    }

                    return RedirectToAction("SendConfirmEmail","Home", new {email= model.Email });
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return View(model);
        }

        [AllowAnonymous]

        public async Task<IActionResult> EpostaOnayla(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "E-posta adresi ve güvenlik kodu gereklidir.";
                return View();
            }

            var user = await context.AspNetUsers.FirstOrDefaultAsync(i => i.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                ViewBag.Message = "Belirtilen kullanıcı bulunmuyor.";
            }
            else
            {


                if (!string.IsNullOrEmpty(user.Token) && user.Token == token)
                {
                    user.EmailConfirmed = true;
                    user.TwoFactorEnabled = false;
                    user.Token = "";
                    await context.SaveChangesAsync();
                    ViewBag.Message = "E-posta hesabınız başarıyla onaylandı.<br>Sayfa üzerindeki menü başlıklarından sitemizde gezintiye başlayabilirsiniz. <br> Siyasett'e üye olduğunuz için teşekkür ederiz.";

                }
                else
                {
                    ViewBag.Message = "Şifre yanlış. E-postanız onaylanamadı.";

                }
            }

            return View();
        }


        [HttpGet("ForgotPassword")]
        [HttpGet("sifremiunuttum")]
        public async Task<IActionResult> ForgotPassword()
        {


            return View();
        }

        [HttpGet("ResetPassword")]
        [HttpGet("sifrekurtar")]
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Index");
            PasswordRecoverModel model = null;

            var usr = await UserManager.FindByEmailAsync(email);
            if (usr == null)
                return RedirectToAction("Index");

            var user = await context.AspNetUsers.FirstOrDefaultAsync(i => i.Email.ToLower() == email.ToLower());

            if (user == null)
            {
                ViewBag.Message = "Belirtilen kullanıcı bulunmuyor.";
            }
            else
            {
                model = new PasswordRecoverModel();
                model.Email = email;
                model.Token = token;
            }


            return View(model);
        }

        [HttpPost("ResetPassword")]
        [HttpPost("sifrekurtar")]
        public async Task<IActionResult> ResetPassword(PasswordRecoverModel model)
        {


            var usr = await UserManager.FindByEmailAsync(model.Email);
            if (usr == null)
                return RedirectToAction("Index");
            if (string.IsNullOrEmpty(model.Password))
                ModelState.AddModelError("", "Şifre girilmesi zorunludur.");

            if (model.Password != model.PasswordConfirm)
                ModelState.AddModelError("", "Girilen şifreler eşleşmiyor.");


            if (ModelState.IsValid)
            {
                var result = await UserManager.ResetPasswordAsync(usr, model.Token, model.Password);
                if (result.Succeeded)
                {
                    usr.Token = "";
                    await UserManager.UpdateAsync(usr);
                    return RedirectToAction("ResetPasswordConfirm");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }


            return View(model);
        }


        [HttpGet("ResetPasswordConfirm")]
        [HttpGet("sifrekurtaronay")]
        public async Task<IActionResult> ResetPasswordConfirm()
        {




            return View();
        }


        [HttpGet("ForgotPasswordConfirm")]
        [HttpGet("sifremiunuttumonay")]
        public async Task<IActionResult> ForgotPasswordConfirm()
        {




            return View();
        }

        [HttpPost("ForgotPassword")]
        [HttpPost("sifremiunuttum")]
        public async Task<IActionResult> ForgotPassword(string Email)
        {


            var usr = await UserManager.FindByEmailAsync(Email);
            if (usr != null)
            {
                var token = await UserManager.GeneratePasswordResetTokenAsync(usr);

                if (!string.IsNullOrEmpty(token))
                {


                    usr.Token = token;
                    await UserManager.UpdateAsync(usr);

                    //var callbackUrl = Url.Page("Home/SifreYenile", pageHandler: null,  values: new { code },    protocol: Request.Scheme);


                    var confirmationLink = Url.Action("ResetPassword", "Home", new { token = token, email = usr.Email }, Request.Scheme);
                    TempData["Message"] = Email + " adresine bir doğrulama postası gönderdik. İçerikte yer alan linke tıklayarak e-posta adresinizi onaylabilirsiniz.";

                    try
                    {

                        string text = $@"<strong>Sayın {usr.FirstName} {usr.LastName}</strong>,
                                <p>Siyasett sayfaları üzerinden yapmış olduğunuz şifremi unuttum işleminizin tamamlanması için lütfen bu mesajımızı onaylayınız.</p>
                                <p>Yeni şifre almak için <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>tıklayınız</a>.</p>
                                <p>Sitemizde yapacağınız gezinti sonrasında düşüncelerinizi iletişim formunu doldurarak bizimle paylaşabilirsiniz.</p>
                                <p>En iyi dileklerimizle,</p>
                                <p><strong>Siyasett Teknik Ekibi</strong></p>";

                        await emailSender.SendEmailAsync(Email, "Şifremi Unuttum", text);

                        return RedirectToAction("ForgotPasswordConfirm");

                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.ToString());
                        ModelState.AddModelError("", "E-posta gönderilirken bir hata oluştu");
                    }
                }
            }
            else
                ModelState.AddModelError("", "Girilen E-posta ile ilgili hesap bulunmuyor.");


            return View();
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup(UserRegisterModel model)
        {

            AppUser appUser = new AppUser();

            appUser.Email = model.Email;
            appUser.EmailConfirmed = true;//daha sonra kapatılacak
            appUser.TwoFactorEnabled = false;
            appUser.UserName = model.Email;
            if (model.Password != model.PasswordConfirm)
                return Ok(new { success = false, msg = "Girilen şifreler farklı." });

            try
            {


                var result = await UserManager.CreateAsync(appUser, model.Password);
                if (result.Succeeded)
                {

                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(appUser);

                    return Ok(new { success = true });

                }
                else
                {
                    string err = "";
                    foreach (var item in result.Errors)
                    {
                        err += " " + item.Description;

                    }
                    return Ok(new { success = false, msg = err });

                }
            }
            catch (Exception e)
            {
                return Ok(new { success = false, msg = e.ToString() });

            }



        }


        [Route("SendConfirmEmail")]
        [HttpGet]
        public IActionResult SendConfirmEmail(string email="")
        {
            ViewBag.Mail = email;
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Peoples(int page = 1, byte sort = 0, byte desc = 0, int pagesize = 30, string firstName = "", string lastName = "", byte gender = 0, int education = 0)
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;
            string[] orderFields = new string[] { "FirstName", "LastName", "GenderId", "EducationId", "PartyName", "PositionName" };
            pager.SetQuery(Request.Query);
            PeopleSearchModel searchModel = new PeopleSearchModel();
            searchModel.FirstName = firstName;
            searchModel.LastName = lastName;
            searchModel.GenderId = gender;
            searchModel.EducationId = education;
            searchModel.OrderBy = sort;
            searchModel.OrderByDesc = desc;

            var liste = await (from a in context.People
                               let position = context.PeoplePositions.Where(i => i.PeopleId == a.Id && !i.EndDate.HasValue).Select(i => new
                               {
                                   PartyName = i.PartyId.HasValue ? i.Party.ShortName : "",
                                   PositionName = i.Position.NameTr,
                                   Institution = i.InstitutionName
                               }).FirstOrDefault()

                               where (string.IsNullOrEmpty(firstName) || (a.FirstNameNormalize).Contains(Helpers.Normalize(firstName)))
                               && (string.IsNullOrEmpty(lastName) || (a.LastNameNormalize).Contains(Helpers.Normalize(lastName)))
                               && (searchModel.GenderId == 0 || (searchModel.GenderId == a.GenderId))
                               && (searchModel.EducationId == 0 || (searchModel.EducationId == a.EducationId))
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
                               }
                       ).OrderByField(orderFields[searchModel.OrderBy], searchModel.OrderByDesc == 0).Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).AsNoTracking().ToListAsync();

            pager.Total = await (from a in context.People
                                 where (string.IsNullOrEmpty(firstName) || (a.FirstNameNormalize).Contains(Helpers.Normalize(firstName)))
                               && (string.IsNullOrEmpty(lastName) || (a.LastNameNormalize).Contains(Helpers.Normalize(lastName)))
                               && (searchModel.GenderId == 0 || (searchModel.GenderId == a.GenderId))
                               && (searchModel.EducationId == 0 || (searchModel.EducationId == a.EducationId))
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

        public async Task<IActionResult> PeopleBasicInfo(int id)
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

        public async Task<IActionResult> PeopleCvInfo(int id)
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

            return View(model);
        }

        public async Task<IActionResult> PeoplePositionsInfo(int id)
        {
            PersonCreateModel? model = await (from a in context.People
                                              where a.Id == id
                                              select new PersonCreateModel
                                              {
                                                  Id = a.Id,

                                                  FirstName = a.FirstName,

                                                  LastName = a.LastName,
                                                  Photo = a.Photo,

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
                                             }).ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> PeopleSamelyRecord(int id)
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
            //try
            //{
            //    var posizyon = context.PeoplePositions.Where(a => a.PeopleId == id).OrderByDescending(i => i.StartDate).First().PositionId;
            //    var sector = context.PeoplePositionSectors.Where(a => a.PeoplePositionId == posizyon).First().SectorId;
            //    ViewBag.PeopleSamely = await (from a in context.People
            //                                  join p in context.PeoplePositions on posizyon equals p.PositionId into p1
            //                                  from PeoplePosition in p1.DefaultIfEmpty()
            //                                  join d in context.PeopleJobs on a.Id equals d.PeopleId into d1
            //                                  from PeopleJob in d1.DefaultIfEmpty()
            //                                  join e in context.Sectors on sector equals e.Id into d2
            //                                  from PeoplePositionSector in d2.DefaultIfEmpty()
            //                                  orderby a.FirstName ascending
            //                                  select new PeopleSamelyModel
            //                                  {
            //                                      PersonFirstName = a.FirstName,
            //                                      PersonLastName = a.LastName,
            //                                      PersonJob = a.PeopleJobs.FirstOrDefault().Job.NameTr,
            //                                      PersonParty=a.Parties.FirstOrDefault().Name,
            //                                      PersonPosition=a.PeoplePositions.FirstOrDefault().Position.NameTr,
            //                                      Photo=a.Photo,

            //                                  }).ToListAsync();
            //}
            //catch (Exception)
            //{

            //    throw;
            //}





            return View(model);
        }

        public async Task<IActionResult> PeopleComesWiki(int id)
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

            return View(model);
        }

        [Route("uyelik")]
        [Route("membership")]
        public async Task<IActionResult> Membership()
        {
           

            return View();
        }

        [Route("uyeliktipi")]
        [Route("MembershipType")]
        public async Task<IActionResult> MembershipType(string email)
        {
            ViewBag.UserId = context.AspNetUsers.FirstOrDefault(a => a.Email == email).Id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MembershipType(string asd,int asdasd)
        {

            return RedirectToAction("/Peoples");
        }
        [Route("Payment")]
        [HttpGet]
        public IActionResult MembershipPayment(int type)
        {
            var payment = 0;
            switch (type)
            {
                case 2:
                    payment = 40;
                    break;
                case 3:
                    payment = 450;
                    break;
                case 4:
                    payment = 1400;
                    break;
                case 5:
                    payment = 300;
                    break;
            }
            ViewBag.Payment = payment;
            return View();
        }
    }
}