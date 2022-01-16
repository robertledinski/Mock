using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Mock.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        IAccountService _accountService;    
        public AccountController (IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        // UI React table presentation
        public AccountSummary Get()
            => _accountService.GetAccountSummary();


        [Route("MockAPI")]
        [HttpPost]
        // Required MockAPI usually here we would have attribute which implements auth (SSO, Identity Server, etc.)
        // It would be probably on controller. Usually backend with services important like this would have unit test
        public IActionResult Mock(DataSourceInput model)
        {
            // Model validation
            if (model == null)
                return BadRequest(AccountErrorCodes.InvalidModel); // Log error

            // Data validation
            if(model.Accounts == null || model.Accounts.Count == 0 || !model.Accounts.SelectMany(x => x.Transactions).Any())
                return BadRequest(AccountErrorCodes.MissingData); // Log error

            return Ok(_accountService.ProcessAccountSummary(model.Accounts));
        }
    }
}
