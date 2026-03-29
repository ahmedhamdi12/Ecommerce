using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class CultureController : Controller
    {
        /// <summary>
        /// Sets the user's preferred culture via a cookie, then redirects back.
        /// Called when the user clicks EN / AR in the layout.
        /// </summary>
        [HttpPost]
        public IActionResult SetCulture(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),  // persists the choice
                    IsEssential = true                             // GDPR: works even without consent
                }
            );

            // LocalRedirect prevents open redirect attacks (only redirects within the app)
            return LocalRedirect(returnUrl ?? "/");
        }
    }
}
