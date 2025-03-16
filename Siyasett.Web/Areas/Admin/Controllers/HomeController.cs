using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siyasett.Data;
using Siyasett.Data.Data;
using Siyasett.Web.Models;

namespace Siyasett.Web.Areas.Admin.Controllers
{


    public class HomeController : AdminController
    {
        public HomeController(ILogger<AdminController> logger, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {


        }

        public async  Task<IActionResult> Index()
        {
            ViewBag.KisiSayisi = await context.People.CountAsync();
            ViewBag.PartyCount = await context.Parties.CountAsync();

            return View();
        }

    }
}
