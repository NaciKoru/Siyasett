using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siyasett.Data.Data;
using Siyasett.Data;
using Siyasett.Web.Models;
using Siyasett.Core.Party;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Siyasett.Web.Areas.Admin.Controllers
{
    
    public class PartyIdeologyManagementController : AdminController
    {
        private readonly IWebHostEnvironment _host;
        public PartyIdeologyManagementController(ILogger<FaqManagementController> logger, IWebHostEnvironment env, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context) : base(logger, signInManager, userManager, roleManager, context)
        {
            _host = env;
        }


        public async Task<IActionResult> Index(int page = 1, int pagesize = 30, string query = "")
        {
            PagerInfo pager = new PagerInfo();
            pager.CurrentPage = page;
            pager.PageSize = pagesize;

            var liste = await (from a in context.PartyIdeologies
                               where context.Parties.FirstOrDefault(c => c.Id == a.PartyId).Active == true
                               &&(string.IsNullOrEmpty(query) || (!string.IsNullOrEmpty(query) && context.PoliticalIdeologies.FirstOrDefault(b => b.Id == a.PoliticialIdeologyId).NameTr.ToUpper().Contains(query.ToUpper()))
                               || (!string.IsNullOrEmpty(query) && context.Parties.FirstOrDefault(b => b.Id == a.PartyId).Name.ToUpper().Contains(query.ToUpper()))
                               || (!string.IsNullOrEmpty(query) && context.Parties.FirstOrDefault(b => b.Id == a.PartyId).ShortName.ToUpper().Contains(query.ToUpper())))
                               select new PartyIdeolojiModel
                               {
                                   Id = a.Id,
                                   PartyId = a.PartyId,
                                   IdeologyId = a.PoliticialIdeologyId,
                                   PartyName = context.Parties.FirstOrDefault(b => b.Id == a.PartyId).Name,
                                   PartyShortName = context.Parties.FirstOrDefault(b => b.Id == a.PartyId).ShortName,
                                   Value = a.Value,
                                   IdeologyName = context.PoliticalIdeologies.FirstOrDefault(b => b.Id == a.PoliticialIdeologyId).NameTr,


                               }
                       ).AsNoTracking().Skip(((pager.CurrentPage - 1) * pager.PageSize)).Take(pager.PageSize).ToListAsync();

            ViewBag.Parties = await (from a in context.Parties
                                     where a.Active
                                     orderby a.ShortName
                                     select new PartyListModel
                                     {
                                         Id = a.Id,
                                         PartyName = a.Name,
                                         PartyNameShort = a.ShortName,

                                     }).ToListAsync();

            ViewBag.Ideologies = await context.PoliticalIdeologies.Select(i => new PoliticalIdeology { Id = i.Id, NameTr = i.NameTr }).OrderBy(i => i.NameTr).AsNoTracking().ToListAsync();

            pager.Total = await (from a in context.PartyIdeologies
                                 where context.Parties.FirstOrDefault(c => c.Id == a.PartyId).Active == true
                             && (!string.IsNullOrEmpty(query) && context.PoliticalIdeologies.FirstOrDefault(b => b.Id == a.PoliticialIdeologyId).NameTr.ToUpper().Contains(query.ToUpper()))
                             && (!string.IsNullOrEmpty(query) && context.Parties.FirstOrDefault(b => b.Id == a.PartyId).Name.ToUpper().Contains(query.ToUpper()))
                             && (!string.IsNullOrEmpty(query) && context.Parties.FirstOrDefault(b => b.Id == a.PartyId).ShortName.ToUpper().Contains(query.ToUpper()))
                                 select new { a.Id }).AsNoTracking().CountAsync();


            ViewBag.Pager = pager;
            return View(liste);

        }

        [HttpPost]
        public async Task<IActionResult> NewIdeology(int party, int ideology, string value)
        {
            try
            {
                var newItem = new PartyIdeology
                {
                    PartyId = party,
                    PoliticialIdeologyId = ideology,
                    Value = Convert.ToInt32(value),
                };

                if (context.PartyIdeologies.Any(i => i.PartyId == party && i.PoliticialIdeologyId == ideology))
                    return Ok(new { success = false, msg = "This item already exists." });

                var partyname = context.Parties.FirstOrDefault(i => i.Id == party).Name;
                var partyshortname = context.Parties.FirstOrDefault(i => i.Id == party).ShortName;
                var ideologyname = context.PoliticalIdeologies.FirstOrDefault(a => a.Id == ideology).NameTr;

                context.PartyIdeologies.Add(newItem);
                await context.SaveChangesAsync();
                return Ok(new { success = true, id = newItem.Id, name = partyname, shortname = partyshortname, ideo = ideologyname });
            }
            catch
            {
                return Ok(new { success = false, msg = "An error occured." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditIdeology(int id, int party, int ideology, string value)
        {
            try
            {
                var editItem = await context.PartyIdeologies.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                editItem.PartyId = party;
                editItem.PoliticialIdeologyId = ideology;
                editItem.Value = Convert.ToInt32(value);

                var partyname = context.Parties.FirstOrDefault(i => i.Id == party).Name;
                var partyshortname = context.Parties.FirstOrDefault(i => i.Id == party).ShortName;
                var ideologyname = context.PoliticalIdeologies.FirstOrDefault(a => a.Id == ideology).NameTr;


                await context.SaveChangesAsync();
                return Ok(new { success = true, id = id, name = partyname, shortname = partyshortname, ideo = ideologyname });
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
                var editItem = await context.PartyIdeologies.FirstOrDefaultAsync(i => i.Id == id);
                if (editItem == null)
                    return Ok(new { success = false, msg = id + " not found." });

                context.PartyIdeologies.Remove(editItem);

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
