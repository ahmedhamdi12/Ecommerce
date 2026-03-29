using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Ecommerce.Resources;

namespace Ecommerce.Controllers
{
    [Route("debug")]
    public class DebugController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public DebugController(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Get()
        {
            System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo("ar");
            var str = _localizer["Dashboard"];
            
            return Ok(new { 
                Key = "Dashboard",
                Value = str.Value,
                ResourceNotFound = str.ResourceNotFound,
                BaseName = _localizer.GetType().Name,
                SearchedLocation = str.SearchedLocation
            });
        }
    }
}
