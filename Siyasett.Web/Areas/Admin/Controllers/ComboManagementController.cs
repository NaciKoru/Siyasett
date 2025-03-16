using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siyasett.Core;
using Siyasett.Core.People;
using Siyasett.Data;
using Siyasett.Data.Data;
using Siyasett.Web.Models;
using System.Data;
using System.Security.Claims;

namespace Siyasett.Web.Areas.Admin.Controllers
{

    public class ComboManagementController : AdminController
    {
        private readonly IWebHostEnvironment _host;
        public ComboManagementController(ILogger<AdminController> logger, IWebHostEnvironment env, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {
            _host = env;
        }


        public async Task<IActionResult> Index(int page = 1, int pagesize = 30, string query = "")
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;

            var liste = await (from a in context.People
                               where string.IsNullOrEmpty(query) || (a.FirstName + " " + a.LastName).Contains(query)
                               orderby a.FirstName
                               select new PeopleListModel
                               {
                                   LastName = a.LastName,
                                   CreatedDate = a.CreatedDate,
                                   EducationId = a.EducationId,
                                   EducationName = a.LastName,
                                   FirstName = a.LastName,
                                   GenderId = a.GenderId,
                                   GenderName = a.Gender.NameTr,
                                   Id = a.Id,
                                   Photo = a.Photo,
                                   UpdatedDate = a.UpdatedDate
                               }
                       ).AsNoTracking().Skip(((pager.CurrentPage-1)*pager.PageSize)).Take(pager.PageSize).ToListAsync();

            pager.Total = await (from a in context.People
                                 where string.IsNullOrEmpty(query) || (a.FirstName + " " + a.LastName).Contains(query)
                                 select new { a.Id }).AsNoTracking().CountAsync();

            ViewBag.Pager = pager;
            return View(liste);
        }


        public async Task<IActionResult> AddresTypes()
        {
            var liste = await context.AddressTypes.OrderBy(i => i.NameTr).Where(i=>i.IsActive==true).Select(a => new ComboBaseModel
            {
                Id = a.Id,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                NameTr = a.NameTr,
                NameEn = a.NameEn
            }).ToListAsync();
           
            return View(liste);
        }

 

        [HttpPost]
        public async Task<IActionResult> NewAddressType(string nametr, string nameen)
        {                        
            try
            {
                var newItem = new AddressType
                {
                    NameTr = nametr,
                    NameEn = nameen,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedUserId= Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    UpdatedUserId= Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    IsActive = true
            };

                if (context.AddressTypes.Any(i => i.NameTr == nametr || i.NameEn==nameen))
                    return Ok(new { success = false, msg = "This item already exists." });

                context.AddressTypes.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditAddressType(int id, string nametr, string nameen)
        {
            try
            {
                var editItem = await context.AddressTypes.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.NameTr = nametr;
                editItem.NameEn = nameen;
                editItem.UpdatedDate = DateTime.Now;
                editItem.UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                editItem.IsActive = true;

                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAddressType(int id)
        {
            try
            {
                var deleteItem = await context.AddressTypes.FirstOrDefaultAsync(i => i.Id == id);
                if (deleteItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.AddressTypes.Remove(deleteItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }




        public async Task<IActionResult> Actives()
        {
            var liste = await context.Actives.OrderBy(i => i.NameTr).Where(i => i.IsActive == true).Select(a => new ComboBaseModel
            {
                Id = a.Id,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                NameTr = a.NameTr,
                NameEn = a.NameEn
            }).ToListAsync();

            return View(liste);
        }

        [HttpPost]
        public async Task<IActionResult> NewActivity(string nametr, string nameen)
        {
            try
            {
                var newItem = new Active
                {
                    NameTr = nametr,
                    NameEn = nameen,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    IsActive = true
                };

                if (context.Actives.Any(i => i.NameTr == nametr || i.NameEn == nameen))
                    return Ok(new { success = false, msg = "This item already exists." });

                context.Actives.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditActivity(int id, string nametr, string nameen)
        {
            try
            {
                var editItem = await context.Actives.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.NameTr = nametr;
                editItem.NameEn = nameen;
                editItem.UpdatedDate = DateTime.Now;
                editItem.UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                editItem.IsActive = true;

                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            try
            {
                var deleteItem = await context.Actives.FirstOrDefaultAsync(i => i.Id == id);
                if (deleteItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.Actives.Remove(deleteItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }



        public async Task<IActionResult> Educations()
        {
            var liste = await context.Educations.OrderBy(i => i.NameTr).Where(i => i.IsActive == true).Select(a => new ComboBaseModel
            {
                Id = a.Id,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                NameTr = a.NameTr,
                NameEn = a.NameEn
            }).ToListAsync();

            return View(liste);
        }


        public async Task<IActionResult> PhoneTypes()
        {
            var liste = await context.PhoneTypes.OrderBy(i => i.NameTr).Select(a => new ComboBaseModel
            {
                Id = a.Id,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                NameTr = a.NameTr,
                NameEn = a.NameEn
            }).ToListAsync();

            return View(liste);
        }

        [HttpPost]
        public async Task<IActionResult> EditPhoneType(int id, string nametr, string nameen)
        {
            try
            {
                var editItem = await context.PhoneTypes.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.NameTr = nametr;
                editItem.NameEn = nameen;
                editItem.UpdatedDate = DateTime.Now;
                editItem.UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                editItem.IsActive = true;

                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> NewPhoneType(string nametr, string nameen)
        {
            try
            {
                var newItem = new PhoneType
                {
                    NameTr = nametr,
                    NameEn = nameen,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    IsActive = true
                };

                if (context.Educations.Any(i => i.NameTr == nametr || i.NameEn == nameen))
                    return Ok(new { success = false, msg = "This item already exists." });

                context.PhoneTypes.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoneType(int id)
        {
            try
            {
                var deleteItem = await context.PhoneTypes.FirstOrDefaultAsync(i => i.Id == id);
                if (deleteItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.PhoneTypes.Remove(deleteItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }



        [HttpPost]
        public async Task<IActionResult> NewEducation(string nametr, string nameen)
        {
            try
            {
                var newItem = new Education
                {
                    NameTr = nametr,
                    NameEn = nameen,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    IsActive = true
                };

                if (context.Educations.Any(i => i.NameTr == nametr || i.NameEn == nameen))
                    return Ok(new { success = false, msg = "This item already exists." });

                context.Educations.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditEducation(int id, string nametr, string nameen)
        {
            try
            {
                var editItem = await context.Educations.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.NameTr = nametr;
                editItem.NameEn = nameen;
                editItem.UpdatedDate = DateTime.Now;
                editItem.UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                editItem.IsActive = true;

                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            try
            {
                var deleteItem = await context.Educations.FirstOrDefaultAsync(i => i.Id == id);
                if (deleteItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.Educations.Remove(deleteItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        public async Task<IActionResult> Socials()
        {
            var liste = await context.SocialTypes.OrderBy(i => i.NameTr).Where(i => i.IsActive == true).Select(a => new ComboBaseModel
            {
                Id = a.Id,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                NameTr = a.NameTr,
                NameEn = a.NameEn,
                UrlPattern=a.UrlPattern,
            }).ToListAsync();

            return View(liste);
        }

        [HttpPost]
        public async Task<IActionResult> NewSocial(string nametr, string nameen,string pattern)
        {
            try
            {
                var newItem = new SocialType
                {
                    NameTr = nametr,
                    NameEn = nameen,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    UrlPattern=pattern,
                    IsActive = true
                };

                if (context.SocialTypes.Any(i => i.NameTr == nametr || i.NameEn == nameen))
                    return Ok(new { success = false, msg = nametr +  " zaten eklenmiş." });

                context.SocialTypes.Add(newItem);
                await context.SaveChangesAsync();

                return Ok(new { success = true, id = newItem.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditSocial(int id, string nametr, string nameen,string pattern)
        {
            try
            {
                var editItem = await context.SocialTypes.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.NameTr = nametr;
                editItem.NameEn = nameen;
                editItem.UrlPattern = pattern;
                editItem.UpdatedDate = DateTime.Now;
                editItem.UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                editItem.IsActive = true;

                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSocial(int id)
        {
            try
            {
                var deleteItem = await context.SocialTypes.FirstOrDefaultAsync(i => i.Id == id);
                if (deleteItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.SocialTypes.Remove(deleteItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        public async Task<IActionResult> Sectors()
        {
            var liste = await context.Sectors.OrderBy(i => i.NameTr).Where(i => i.IsActive == true).Select(a => new ComboBaseModel
            {
                Id = a.Id,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                NameTr = a.NameTr,
                NameEn = a.NameEn,
                Weightiness=a.Weightiness,
            }).OrderBy(b=>b.Weightiness).ToListAsync();

            return View(liste);
        }

        [HttpPost]
        public async Task<IActionResult> NewSector(string nametr, string nameen, int weightiness)
        {
            try
            {
                var newItem = new Sector
                {
                    NameTr = nametr,
                    NameEn = nameen,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    IsActive = true,
                    Weightiness = weightiness,
                };

                if (context.Sectors.Any(i => i.NameTr == nametr || i.NameEn == nameen))
                    return Ok(new { success = false, msg = "This item already exists." });

                context.Sectors.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditSector(int id, string nametr, string nameen, int weightiness)
        {
            try
            {
                var editItem = await context.Sectors.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.NameTr = nametr;
                editItem.NameEn = nameen;
                editItem.UpdatedDate = DateTime.Now;
                editItem.UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                editItem.IsActive = true;
                editItem.Weightiness = weightiness;


                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSector(int id)
        {
            try
            {
                var deleteItem = await context.Sectors.FirstOrDefaultAsync(i => i.Id == id);
                if (deleteItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.Sectors.Remove(deleteItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        public async Task<IActionResult> Institutions()
        {
            var liste = await context.InstitutionTypes.OrderBy(i => i.NameTr).Where(i => i.IsActive == true).Select(a => new ComboBaseModel
            {
                Id = a.Id,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                NameTr = a.NameTr,
                NameEn = a.NameEn
            }).ToListAsync();

            return View(liste);
        }

        [HttpPost]
        public async Task<IActionResult> NewInstitution(string nametr, string nameen)
        {
            try
            {
                var newItem = new InstitutionType
                {
                    NameTr = nametr,
                    NameEn = nameen,
                    CreateDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    IsActive = true
                };

                if (context.InstitutionTypes.Any(i => i.NameTr == nametr || i.NameEn == nameen))
                    return Ok(new { success = false, msg = "This item already exists." });

                context.InstitutionTypes.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditInstitution(int id, string nametr, string nameen)
        {
            try
            {
                var editItem = await context.InstitutionTypes.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.NameTr = nametr;
                editItem.NameEn = nameen;
                editItem.UpdatedDate = DateTime.Now;
                editItem.UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                editItem.IsActive = true;

                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteInstitution(int id)
        {
            try
            {
                var deleteItem = await context.InstitutionTypes.FirstOrDefaultAsync(i => i.Id == id);
                if (deleteItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.InstitutionTypes.Remove(deleteItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        public async Task<IActionResult> Regions()
        {
            var liste = await context.Regions.OrderBy(i => i.NameTr).Where(i => i.IsActive == true).Select(a => new ComboBaseModel
            {
                Id = a.Id,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                NameTr = a.NameTr,
                NameEn = a.NameEn
            }).ToListAsync();

            return View(liste);
        }

        [HttpPost]
        public async Task<IActionResult> NewRegion(string nametr, string nameen)
        {
            try
            {
                var newItem = new Region
                {
                    NameTr = nametr,
                    NameEn = nameen,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    IsActive = true
                };

                if (context.Regions.Any(i => i.NameTr == nametr || i.NameEn == nameen))
                    return Ok(new { success = false, msg = "This item already exists." });

                context.Regions.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditRegion(int id, string nametr, string nameen)
        {
            try
            {
                var editItem = await context.Regions.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.NameTr = nametr;
                editItem.NameEn = nameen;
                editItem.UpdatedDate = DateTime.Now;
                editItem.UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                editItem.IsActive = true;

                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRegions(int id)
        {
            try
            {
                var deleteItem = await context.Regions.FirstOrDefaultAsync(i => i.Id == id);
                if (deleteItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.Regions.Remove(deleteItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        public async Task<IActionResult> Jobs()
        {
            var liste = await context.Jobs.OrderBy(i => i.NameTr).Where(i => i.IsActive == true).Select(a => new ComboBaseModel
            {
                Id = a.Id,
                IsActive = a.IsActive,
                NameTr = a.NameTr,
                NameEn = a.NameEn
            }).ToListAsync();

            return View(liste);
        }

        [HttpPost]
        public async Task<IActionResult> NewJob(string nametr, string nameen)
        {
            try
            {
                var newItem = new Job
                {
                    NameTr = nametr,
                    NameEn = nameen,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    IsActive = true
                };

                if (context.Jobs.Any(i => i.NameTr == nametr || i.NameEn == nameen))
                    return Ok(new { success = false, msg = "This item already exists." });

                context.Jobs.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditJob(int id, string nametr, string nameen)
        {
            try
            {
                var editItem = await context.Jobs.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.NameTr = nametr;
                editItem.NameEn = nameen;
                editItem.UpdatedDate = DateTime.Now;
                editItem.UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                editItem.IsActive = true;

                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteJob(int id)
        {
            try
            {
                var deleteItem = await context.Jobs.FirstOrDefaultAsync(i => i.Id == id);
                if (deleteItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.Jobs.Remove(deleteItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        public async Task<IActionResult> Positions()
        {
            var liste = await context.Positions.OrderBy(i => i.NameTr).Where(i => i.IsActive == true).Select(a => new ComboBaseModel
            {
                Id = a.Id,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value: false,
                NameTr = a.NameTr,
                NameEn = a.NameEn,
                Weightiness=a.Weightiness,
            }).ToListAsync();

            return View(liste);
        }

        [HttpPost]
        public async Task<IActionResult> NewPosition(string nametr, string nameen,int weightiness)
        {
            try
            {
                var newItem = new Position
                {
                    NameTr = nametr,
                    NameEn = nameen,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    IsActive = true,
                    Weightiness=weightiness,
                };

                if (context.Positions.Any(i => i.NameTr == nametr || i.NameEn == nameen))
                    return Ok(new { success = false, msg = "This item already exists." });

                context.Positions.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditPosition(int id, string nametr, string nameen,int weightiness)
        {
            try
            {
                var editItem = await context.Positions.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.NameTr = nametr;
                editItem.NameEn = nameen;
                editItem.UpdatedDate = DateTime.Now;
                editItem.UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                editItem.IsActive = true;
                editItem.Weightiness = weightiness;

                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePosition(int id)
        {
            try
            {
                var deleteItem = await context.Positions.FirstOrDefaultAsync(i => i.Id == id);
                if (deleteItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.Positions.Remove(deleteItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> New(PersonCreateModel model,int durum)
        {

            if (ModelState.IsValid)
            {
                var person = new Person();
                person.FirstName = model.FirstName;
                person.LastName = model.LastName;
                person.PartyId = model.PartyId;
                person.InstitutionTypeId = model.InstitutionTypeId;
                
                person.GenderId = model.GenderId;
                person.PositionFieldId = model.PositionFieldId;
                person.CreatedUserId = this.appUser.Id;
                person.UpdatedUserId=this.appUser.Id;
                person.EducationId = model.EducationId;
                person.CvEn = model.CvEn;
                person.CvTr = model.CvTr;

                if(!string.IsNullOrEmpty( model.UploadPhoto))
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

                context.People.Add(person);
                await context.SaveChangesAsync();
                if(durum==1)
                {
                    return RedirectToAction("Edit", new { id = person.Id });
                }
                else
                {

                    return RedirectToAction("Index");
                }

            }


            ViewBag.Genders=context.Genders.AsNoTracking().ToList();
            ViewBag.InstitutionTypes = context.InstitutionTypes.AsNoTracking().ToList();
            ViewBag.Positions = context.Positions.AsNoTracking().ToList();
            ViewBag.PositionFields = context.PositionFields.AsNoTracking().ToList();
            ViewBag.Educations = context.Educations.AsNoTracking().ToList();
            


            return View(model);

        }

        public async Task<IActionResult> PollTypes()
        {
            var liste = await context.PollTypes.OrderBy(i => i.NameTr).Where(i => i.IsActive == true).Select(a => new ComboBaseModel
            {
                Id = a.Id,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                NameTr = a.NameTr,
                NameEn = a.NameEn
            }).ToListAsync();

            return View(liste);
        }

        [HttpPost]
        public async Task<IActionResult> NewPollType(string nametr, string nameen)
        {
            try
            {
                var newItem = new PollType
                {
                    NameTr = nametr,
                    NameEn = nameen,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    IsActive = true
                };

                if (context.PollTypes.Any(i => i.NameTr == nametr || i.NameEn == nameen))
                    return Ok(new { success = false, msg = "This item already exists." });

                context.PollTypes.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditPollType(int id, string nametr, string nameen)
        {
            try
            {
                var editItem = await context.PollTypes.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.NameTr = nametr;
                editItem.NameEn = nameen;
                editItem.UpdatedDate = DateTime.Now;
                editItem.UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                editItem.IsActive = true;

                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePollType(int id)
        {
            try
            {
                var deleteItem = await context.PollTypes.FirstOrDefaultAsync(i => i.Id == id);
                if (deleteItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.PollTypes.Remove(deleteItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        public async Task<IActionResult> PollKinds()
        {
            var liste = await context.PollKinds.OrderBy(i => i.NameTr).Where(i => i.IsActive == true).Select(a => new ComboBaseModel
            {
                Id = a.Id,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                NameTr = a.NameTr,
                NameEn = a.NameEn
            }).ToListAsync();

            return View(liste);
        }

        [HttpPost]
        public async Task<IActionResult> NewPollKind(string nametr, string nameen)
        {
            try
            {
                var newItem = new PollKind
                {
                    NameTr = nametr,
                    NameEn = nameen,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    IsActive = true
                };

                if (context.PollTypes.Any(i => i.NameTr == nametr || i.NameEn == nameen))
                    return Ok(new { success = false, msg = "This item already exists." });

                context.PollKinds.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditPollKind(int id, string nametr, string nameen)
        {
            try
            {
                var editItem = await context.PollKinds.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.NameTr = nametr;
                editItem.NameEn = nameen;
                editItem.UpdatedDate = DateTime.Now;
                editItem.UpdatedUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                editItem.IsActive = true;

                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePollKind(int id)
        {
            try
            {
                var deleteItem = await context.PollKinds.FirstOrDefaultAsync(i => i.Id == id);
                if (deleteItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.PollKinds.Remove(deleteItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        public async Task<IActionResult> Language()
        {
            var liste = await context.LanguageResources.OrderBy(i => i.Name).Select(a => new LanguageResource
            {
                Id = a.Id,
               Value=a.Value,
               Name=a.Name,
               LanguageId=a.LanguageId,
            }).ToListAsync();

            return View(liste);
        }

        [HttpPost]
        public async Task<IActionResult> NewLanguange(string nametr, string nameen, string nameen2)
        {
            try
            {
                var newItem = new LanguageResource
                {
                    Name= nametr,
                    Value=nameen,
                    LanguageId=1,
                };

                if (context.LanguageResources.Any(i => i.Name == nametr || i.Value == nameen))
                    return Ok(new { success = false, msg = "Türkçe karşılık kaydı mevcut." });

                context.LanguageResources.Add(newItem);

                await context.SaveChangesAsync();

               var newItem2 = new LanguageResource
                {
                    Name = nametr,
                    Value = nameen2,
                    LanguageId = 2,
                };

                if (context.LanguageResources.Any(i => i.Value == nameen2))
                    return Ok(new { success = false, msg = "İngilizce karşılık kaydı mevcut.Türkçe karşılık eklendi!" });

                context.LanguageResources.Add(newItem2);

                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id,id2=newItem2.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditLanguange(string id, string nametr, string nameen)
        {
            try
            {
                var editItemtr = await context.LanguageResources.FirstOrDefaultAsync(i => i.Name == id && i.LanguageId==1);
                var editItemen = await context.LanguageResources.FirstOrDefaultAsync(i => i.Name == id && i.LanguageId==2);
                
                if (editItemtr == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItemtr.Value= nametr;
                editItemen.Value = nameen;

                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        
        public async Task<IActionResult> GetLanguange(string id)
        {
            try
            {
                var editItem = await context.LanguageResources.Where(i => i.Name == id).ToListAsync();

                if (editItem.Count==0)
                    return Ok(new { success = false });

                var tr = editItem.FirstOrDefault(i => i.LanguageId == 1).Value;
                var en = editItem.FirstOrDefault(i => i.LanguageId == 2).Value;

                
                return Ok(new { success = true,id=id,tr,en });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLanguange(int id)
        {
            try
            {
                var deleteItem = await context.LanguageResources.FirstOrDefaultAsync(i => i.Id == id);
                if (deleteItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.LanguageResources.Remove(deleteItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }



        public async Task<IActionResult> Ideologies()
        {
            var liste = await context.PoliticalIdeologies.OrderBy(i => i.NameTr).Where(i => i.IsActive == true).Select(a => new ComboBaseModel
            {
                Id = a.Id,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                NameTr = a.NameTr,
                NameEn = a.NameEn
            }).ToListAsync();

            return View(liste);
        }

        [HttpPost]
        public async Task<IActionResult> NewIdeology(string nametr, string nameen)
        {
            try
            {
                var newItem = new PoliticalIdeology
                {
                    NameTr = nametr,
                    NameEn = nameen,
                    IsActive = true
                };

                if (context.PoliticalIdeologies.Any(i => i.NameTr == nametr || i.NameEn == nameen))
                    return Ok(new { success = false, msg = "This item already exists." });

                context.PoliticalIdeologies.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditIdeology(int id, string nametr, string nameen)
        {
            try
            {
                var editItem = await context.PoliticalIdeologies.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.NameTr = nametr;
                editItem.NameEn = nameen;
                editItem.IsActive = true;

                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIdeology(int id)
        {
            try
            {
                var deleteItem = await context.PoliticalIdeologies.FirstOrDefaultAsync(i => i.Id == id);
                if (deleteItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.PoliticalIdeologies.Remove(deleteItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }


    }
}
