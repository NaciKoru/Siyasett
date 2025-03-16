using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Siyasett.Data;
using Siyasett.Data.Data;
using Siyasett.Models;

namespace Siyasett.Web.Models
{

    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class AdminController: Controller
    {


        protected AppUser? appUser;
        protected readonly ILogger<AdminController> logger;
        protected readonly SignInManager<AppUser> signInManager;
        protected readonly UserManager<AppUser> userManager;
        protected readonly RoleManager<IdentityRole<Guid>> roleManager;
        protected readonly AppDbContext context;

        public AdminController(ILogger<AdminController> logger, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager,AppDbContext context)
        {
            this.logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {

                var userTask = userManager.FindByNameAsync(this.User.Identity.Name);
                userTask.Wait();

                this.appUser = userTask.Result;


            }

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {


            base.OnActionExecuted(context);
        }


        public ToastMessage AddToastMessage(string title, string message, ToastType toastType = ToastType.info)
        {
            Toastr toastr = this.TempData["Toastr"] as Toastr;
            toastr = toastr ?? new Toastr();

            var toastMessage = toastr.AddToastMessage(title, message, toastType);
            this.TempData["Toastr"] = JsonConvert.SerializeObject(toastr);
            return toastMessage;
        }


    }
}
