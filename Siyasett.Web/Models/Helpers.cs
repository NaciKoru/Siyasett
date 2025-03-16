using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Siyasett.Models
{
    public static class Helpers
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

        }
        public static string StripHTML(string input)
        {
            if (input == null)
            {
                return string.Empty;
            }

            return Regex.Replace(input, "<.*?>", String.Empty);

        }
        public static string StripLinkHTML(string input)
        {
            if (input == null)
            {
                return string.Empty;
            }
            

            return Regex.Replace(input, "</?(a|A).*?>", "");

        }
        public static string SubStringSentences(string input, int index)
        {
            int indexof = input.IndexOf(".", index);
            return input.Substring(0, indexof+1);


        }

        public static string GetSocialMediaLink(int mediaType, string address)
        {
            switch (mediaType)
            {
                case 1://twitter
                    return $"https://twitter.com/{address.Replace("@", "").Replace("#", "")}";
                case 2:
                    return $"https://www.facebook.com/{address.Replace("@", "").Replace("#", "")}";
                case 3:
                    return $"https://www.youtube.com/c/{address.Replace("@", "").Replace("#", "")}";
                case 4:
                    return $"https://www.instagram.com/{address.Replace("@", "").Replace("#", "")}";


                default:
                    return "";
            }

        }

        public static string GetStartEndDateString(int? day, int?month, int? year )
        {

            string result = "";

            if (day.HasValue)
                result += $"{day.Value.ToString("00")}.";
            if (month.HasValue)
                result += $"{month.Value.ToString("00")}.";
            if (year.HasValue)
                result += $"{year}";

            return result;
        }
        public static string IsSelected(this IHtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = html.ViewContext.RouteData.Values["action"].ToString().ToLower();
            string currentController = html.ViewContext.RouteData.Values["controller"].ToString().ToLower();

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller.ToLower() == currentController.ToLower() && action.ToLower().Contains(currentAction.ToLower()) ?
                cssClass : String.Empty;
        }



        public static string GetUserPhoto(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "/images/users/user2.png";

            return $"/images/users/{name}";
        }

        public static string GetBookPhoto( string? name)
        {
            if (string.IsNullOrEmpty(name))
                return "/images/books/book.png";
            

            return $"/images/books/{name}";
        }

        public static string GetPersonPhoto(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "/images/person/user2.png";
            


            return $"/images/person/{name}";
        }

        public static string GetPartyLogo(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "/images/parties/logo.png";

            return $"/images/parties/{name}";
        }


 


        public static string ToReadableString(this TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}",
                span.Duration().Days > 0 ? string.Format("{0:0} g, ", span.Days) : string.Empty,
                span.Duration().Hours > 0 ? string.Format("{0:0} s, ", span.Hours) : string.Empty,
                span.Duration().Minutes > 0 ? string.Format("{0:0} dk, ", span.Minutes) : string.Empty,
                span.Duration().Seconds > 0 ? string.Format("{0:0} sn", span.Seconds) : string.Empty);

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted)) formatted = "0 seconds";

            return formatted;
        }

        public static string ToReadableDistance(this double distance)
        {
            if (distance >= 1000)
            {
                return Math.Round(distance / 1000, 1).ToString("#,###.#") + " km.";
            }
            else
            {
                return Math.Round(distance, 1).ToString("#,###.#") + " m.";

            }

        }

        public static string ToUrlSlug(string value)
        {

            //First to lower case 
            value = value.Replace("İ", "i").Replace("ı", "i").ToLowerInvariant();
            value = value.Replace("Ü", "u").Replace("Ö", "o").ToLowerInvariant();
            value = value.Replace("ü", "u").Replace("ö", "o").ToLowerInvariant();
            value = value.Replace("Ç", "c").Replace("ç", "c").ToLowerInvariant();
            value = value.Replace("Ğ", "g").Replace("ğ", "g").ToLowerInvariant();
            value = value.Replace("Ş", "s").Replace("ş", "s").ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("utf-8").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces 
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars 
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            //Trim dashes from end 
            value = value.Trim('-', '_');

            //Replace double occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }



        public static string Normalize(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            //First to upper case 
            value = value.Replace("i", "I").Replace("ı", "I");
            value = value.Replace("ü", "U").Replace("Ü", "U");
            value = value.Replace("ö", "O").Replace("Ö", "O");
            value = value.Replace("Ç", "C").Replace("ç", "C");
            value = value.Replace("Ğ", "G").Replace("ğ", "G");
            value = value.Replace("İ", "I");
            value = value.Replace("Ş", "S").Replace("ş", "S").ToUpperInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("utf-8").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces 
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars 
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            //Trim dashes from end 
            value = value.Trim('-', '_');

            //Replace double occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }



    }
}
