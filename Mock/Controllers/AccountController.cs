using Microsoft.AspNetCore.Mvc;

namespace Mock.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        // All must be async
        // Initial table with all data
        public IActionResult Index()
        {
            return View();
        }

        [Route("Summary")]
        // Calculated response in table
        public IActionResult ReportSummary(int UserId)
        {
            return View();
        }



    }
}
