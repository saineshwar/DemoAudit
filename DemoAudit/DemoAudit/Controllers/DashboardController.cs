using DemoAudit.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DemoAudit.Controllers
{
    [ServiceFilter(typeof(AuditFilterAttribute))]
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
