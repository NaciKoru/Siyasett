using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using Siyasett.Core.Extensions;
using Siyasett.Core.Users;
using Siyasett.Data;
using Siyasett.Data.Data;
using Siyasett.Models;
using Siyasett.Web.Models;
using System.Drawing.Printing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Siyasett.Web.Areas.Admin.Controllers
{


    public class UsersController : AdminController
    {
        public UsersController(ILogger<AdminController> logger, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {

        }

        public async Task<IActionResult> Index(int? page, byte sort = 3, byte desc = 0, string firstName = "", string lastName = "", string email = "", string createDate = "",int roles=0)
        {
            if (!page.HasValue)
                page = 1;
            string[] orderFields = new string[] { "FirstName", "LastName", "Email", "CreateDate","Roles", "UserTypeId" };

            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = (int)page;
            pager.PageSize = 30;

            UserSearchModel searchModel = new UserSearchModel();
            
            searchModel.FirstName = firstName;
            searchModel.LastName = lastName;
            searchModel.Email= email;
            searchModel.CreateDate = createDate;
            searchModel.Role = roles;
            searchModel.OrderBy = sort;
            searchModel.OrderByDesc = desc;
            var users = await (from a in context.AspNetUsers
                               where (string.IsNullOrEmpty(firstName) || (a.FirstName.ToUpper()).Contains(firstName.ToUpper()))
                               && (string.IsNullOrEmpty(lastName) || (a.LastName.ToUpper()).Contains(lastName.ToUpper()))
                               && (string.IsNullOrEmpty(email) || (a.Email.ToUpper()).Contains(email.ToUpper()))
                               && (string.IsNullOrEmpty(createDate) || (a.CreateDate.ToString().ToUpper()).Contains(createDate.ToUpper()))
                               select new UserListModel
                               {
                                   CreateDate = a.CreateDate,
                                   Email = a.Email,
                                   FirstName = a.FirstName,
                                   Id = a.Id,
                                   LastName = a.LastName,
                                   Roles = String.Join(", " ,a.Roles.Select(i => i.Name)),
                                   UserTypeId = a.UserTypeId,
                                   Phone=a.PhoneNumber,
                                   
                               }).OrderByField(orderFields[searchModel.OrderBy], searchModel.OrderByDesc == 1).AsNoTracking().Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).ToListAsync();

            pager.Total = await (from a in context.AspNetUsers
                                 where (string.IsNullOrEmpty(firstName) || (a.FirstName.ToUpper()).Contains(firstName.ToUpper()))
                                 && (string.IsNullOrEmpty(lastName) || (a.LastName.ToUpper()).Contains(lastName.ToUpper()))
                                 && (string.IsNullOrEmpty(email) || (a.Email.ToUpper()).Contains(email.ToUpper()))
                                 && (string.IsNullOrEmpty(createDate) || (a.CreateDate.ToString().ToUpper()).Contains(createDate.ToUpper()))
                                 select a.Id).CountAsync();


            ViewBag.SearchModel = searchModel;
            ViewBag.Pager = pager;
            return View(users);
        }

        public async Task<IActionResult> Edit (string id)
        {
            Guid g = Guid.Parse(id);
            var usr = await context.AspNetUsers.Where(i=>i.Id==g)
                .Select(i=>new UserEditModel
                {
                    Email=i.Email,
                    Id=i.Id,
                    FirstName=i.FirstName, 
                    LastName=i.LastName,
                     IsActive=true,
                     PhoneNumber= i.PhoneNumber,
                     CreateDate=i.CreateDate,
                     EmailConfirmed=i.EmailConfirmed,
                     Roles=context.AspNetRoles.Select(l=>new UserRoleModel
                     {
                         Id=l.Id,
                         Name=l.Name,
                         Selected=i.Roles.Any(k=>k.Id==l.Id)
                     }).ToList(),
                })
                .FirstOrDefaultAsync();
            return View(usr);
        }

      
        public async Task<IActionResult> Delete(string id)
        {
            Guid g = Guid.Parse(id);
            var usr = await userManager.FindByIdAsync(id);

            //var usr = await context.AspNetUsers.FirstOrDefaultAsync(i => i.Id == g);
            await userManager.DeleteAsync(usr);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserEditModel model)
        {

            if (!Helpers.IsValidEmail(model.Email))
                ModelState.AddModelError("", "E-posta adresi geçerli değil");
            if (ModelState.IsValid)
            {
                Guid g = Guid.Parse(id);
                var usr =await userManager.FindByIdAsync(id);
                
                //var usr = await context.AspNetUsers.FirstOrDefaultAsync(i => i.Id == g);

                usr.PhoneNumber = model.PhoneNumber;
                usr.FirstName = model.FirstName;
                usr.LastName = model.LastName;
                usr.Email = model.Email;
                usr.UserName = model.Email;
                usr.EmailConfirmed=model.EmailConfirmed;
                usr.LastUpdate = DateTime.Now;
                await userManager.UpdateAsync(usr);
                                

                foreach (var item in model.Roles)
                {
                    if(await userManager.IsInRoleAsync(usr, item.Name))
                    {
                        if (!item.Selected)
                        {
                            await userManager.RemoveFromRoleAsync(usr, item.Name);
                        }
                    }
                    else
                    {
                        if(item.Selected)
                        {
                            await userManager.AddToRoleAsync(usr, item.Name);
                        }
                    }
                }
                return RedirectToAction("index");
            }
            return View(model);
        }

    }
}
