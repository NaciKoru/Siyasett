using Microsoft.Extensions.Caching.Memory;
using Siyasett.Data.Data;

namespace Siyasett.Web.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly AppDbContext _context;
     

        public LanguageService(AppDbContext context)
        {
            _context = context;
          
        }

        public IEnumerable<Language> GetLanguages()
        {
            return _context.Languages.ToList();
        }

        public Language GetLanguageByCulture(string culture)
        {
            return _context.Languages.FirstOrDefault(x => x.Culture.Trim().ToLower() == culture.Trim().ToLower());
        }


    
    }
}
