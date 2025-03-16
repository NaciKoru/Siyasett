using Siyasett.Data.Data;

namespace Siyasett.Web.Languages
{
    public interface ILocalizationService
    {
        LanguageResource GetStringResource(string resourceKey, int languageId);
    }
}
