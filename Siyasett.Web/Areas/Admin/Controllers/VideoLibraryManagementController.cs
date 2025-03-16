using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Siyasett.Data.Data;
using Siyasett.Data;
using Siyasett.Web.Models;
using Siyasett.Core.VideoLibrary;
using Microsoft.EntityFrameworkCore;

namespace Siyasett.Web.Areas.Admin.Controllers
{
    
    public class VideoLibraryManagementController : AdminController
    {
        private readonly IWebHostEnvironment _host;
        public VideoLibraryManagementController(ILogger<FaqManagementController> logger, IWebHostEnvironment env, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {
            _host = env;
        }


        public async Task<IActionResult> Index(int page = 1, int pagesize = 20, string query = "", int session = 0, int lang = 0)
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;

            var liste = await (from a in context.VideoLibraries
                               where string.IsNullOrEmpty(query) || (!string.IsNullOrEmpty(query) && a.Head.ToUpper().Contains(query.ToUpper())) || (session != 0 && a.Session == session) || (!string.IsNullOrEmpty(query) && a.VideoLink.ToUpper().Contains(query.ToUpper())) || (lang != 0 && a.Language == lang)
                               select new VideoLibraryModel
                               {
                                   Id = a.Id,
                                   Header = a.Head,
                                   Language = a.Language,
                                   VideoLink = a.VideoLink,
                                   Session = a.Session
                               }
                                ).OrderBy(j => j.Session).ThenBy(k => k.Header).AsNoTracking().Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).ToListAsync();

            pager.Total = await (from a in context.VideoLibraries
                                 where string.IsNullOrEmpty(query) || (!string.IsNullOrEmpty(query) && a.Head.ToUpper().Contains(query.ToUpper())) || (session != 0 && a.Session == session) || (!string.IsNullOrEmpty(query) && a.VideoLink.ToUpper().Contains(query.ToUpper())) || (lang != 0 && a.Language == lang)
                                 select new VideoLibrary
                                 {
                                     Id = a.Id,

                                 }
                                ).AsNoTracking().CountAsync();

            ViewBag.Session = await context.VideoSessions.AsNoTracking().ToListAsync();
            ViewBag.Language = await context.GeneralLanguages.AsNoTracking().ToListAsync();


            ViewBag.Pager = pager;

            return View(liste);
        }

        [HttpPost]
        public async Task<IActionResult> NewVideo(int session, string header, int language, string videoLink)
        {
            try
            {
                var asd = videoLink.Split("=")[0] +"="+ videoLink.Split("=")[1];
                var newItem = new VideoLibrary
                {
                    Head = header,
                    Language = language,
                    Session = session,
                    VideoLink = asd,
                };

                //if (context.PartyIdeologies.Any(i => i.PartyId == party && i.PoliticialIdeologyId == ideology))
                //    return Ok(new { success = false, msg = "This item already exists." });


                context.VideoLibraries.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id, session = session, header = header, language = language, videoLink = asd });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditVideo(int id, int session, string header, int language, string videoLink)
        {
            try
            {
                var editItem = await context.VideoLibraries.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.Session = session;
                editItem.VideoLink = videoLink;
                editItem.Language = language;
                editItem.Head = header;

                await context.SaveChangesAsync();
                return Ok(new { success = true, id = id, session = session, header = header, language = language, videoLink = videoLink });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            try
            {
                var editItem = await context.VideoLibraries.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.VideoLibraries.Remove(editItem);

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
