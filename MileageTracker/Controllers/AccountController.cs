using Microsoft.AspNet.Identity;
using MileageTracker.Interfaces;
using MileageTracker.ViewModels;
using System.Threading.Tasks;
using System.Web.Http;

namespace MileageTracker.Controllers {

    public class AccountController : ApiController {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService) {
            _accountService = accountService;
        }

        /// <summary>
        /// Register a new account
        /// </summary>
        /// <param name="registerViewModel">Userdetails for registration</param>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register([FromBody]RegisterViewModel registerViewModel) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var result = await _accountService.RegisterUser(registerViewModel);

            var errorResult = GetErrorResult(result);

            return errorResult ?? Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result) {
            if (result == null) {
                return InternalServerError();
            }

            if (!result.Succeeded) {
                if (result.Errors != null) {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid) {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
