using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Siyasett.Web.Languages;

namespace Siyasett.Web.Models
{
    public class BaseController:Controller
    {
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;

        public delegate HtmlString Localizer(string resourceKey, params object[] args);

        public BaseController(ILanguageService languageService, ILocalizationService localizationService)
        {
            _languageService = languageService;
            _localizationService = localizationService;
        }

        public HtmlString Localize(string resourceKey, params object[] args)
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;

            var language = _languageService.GetLanguageByCulture(currentCulture);
            if (language != null)
            {
                var stringResource = _localizationService.GetStringResource(resourceKey, language.Id);
                if (stringResource == null || string.IsNullOrEmpty(stringResource.Value))
                {
                    return new HtmlString(resourceKey);
                }

                return new HtmlString((args == null || args.Length == 0)
                    ? stringResource.Value
                    : string.Format(stringResource.Value, args));
            }

            return new HtmlString(resourceKey);
        }

    }
}
