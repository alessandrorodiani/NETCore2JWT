using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prj.Api.Controllers
{
    [Route("test")]
    public class TestController : Controller
    {
        [Route("policy-portaluser")]
        [Authorize(Policy = "PolicyPortalUser")]
        public IActionResult Index()
        {
            return Json(new {Ok = true});
        }
    }
}