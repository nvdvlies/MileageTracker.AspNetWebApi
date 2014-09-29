using System.Net;
using System.Net.Http;
using System.Text;
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
        public async Task<HttpResponseMessage> Register([FromBody]RegisterViewModel registerViewModel) {
            if (!ModelState.IsValid) {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var result = await _accountService.RegisterUser(registerViewModel);

            var errorResult = GetErrorResult(result);

            return errorResult ?? Request.CreateResponse(HttpStatusCode.OK);
        }

        private HttpResponseMessage GetErrorResult(IdentityResult result) {
            if (result == null) {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            if (!result.Succeeded) {
                var sb = new StringBuilder();
                if (result.Errors != null) {
                    foreach (var error in result.Errors) {
                        sb.AppendLine(error);
                    }
                }

                if (string.IsNullOrEmpty(sb.ToString())) {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = sb.ToString() });
            }

            return null;
        }
    }
}
