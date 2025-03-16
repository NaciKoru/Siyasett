using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siyasett.Data.Data;

namespace Siyasett.Web.Components
{
    public class Historical : ViewComponent
    {
        private readonly AppDbContext appcontext;

        public Historical(AppDbContext appcontext)
        {
            this.appcontext = appcontext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var ay = DateTime.Now.Month;
            var gun = DateTime.Now.Day;
            var bugun = await appcontext.Chronologies.Where(i => i.EventDate.Month == ay && i.EventDate.Day == gun)
                .Select(i => i).OrderByDescending(i=>i.EventDate)
                .Take(8).ToListAsync();



            return View(bugun);
        }

    }
}
