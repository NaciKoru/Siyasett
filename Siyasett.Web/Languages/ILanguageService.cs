using Siyasett.Data.Data;

namespace Siyasett.Web.Languages
{
    public interface ILanguageService
    {
        IEnumerable<Language> GetLanguages();
        Language GetLanguageByCulture(string culture);
    }
}
