using Microsoft.Extensions.Caching.Memory;
using Siyasett.Data.Data;
using System.Diagnostics;

namespace Siyasett.Web.Languages
{
    public class LocalizationService : ILocalizationService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache memCache;
        const string cacheKey = "languageResourceKey";
        public LocalizationService(AppDbContext context, IMemoryCache memCache)
        {
            _context = context;
            this.memCache = memCache;
        }

        public LanguageResource GetStringResource(string resourceKey, int languageId)
        {
            var list = GetLanguageResources();

            return list.FirstOrDefault(x =>x.Name.Trim().ToLower() == resourceKey.Trim().ToLower() && x.LanguageId == languageId);
        }

        private List<LanguageResource> GetLanguageResources()
        {
            if (!memCache.TryGetValue(cacheKey, out List<LanguageResource> langList))
            {
                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    Priority = CacheItemPriority.Normal
                };

                langList = _context.LanguageResources.ToList();
                memCache.Set(cacheKey, langList, cacheExpOptions);
            }

            return langList;

        }


    }

}
