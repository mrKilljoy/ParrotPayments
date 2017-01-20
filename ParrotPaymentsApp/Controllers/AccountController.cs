using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using ParrotPaymentsApp.Infrastructure;
using ParrotPaymentsApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ParrotPaymentsApp.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthManager _repo = null;

        public AccountController()
        {
            _repo = new AuthManager();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            IdentityResult result = null;
            string exMsg = null;

            try
            {
                result = await _repo.RegisterUser(userModel);
            }
            catch (Exception ex)
            {
                exMsg = ex.Message;
            }

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
                return errorResult;
            
            return Ok(exMsg);
        }

        [Authorize]
        [Route("GetPublicName")]
        public async Task<IHttpActionResult> GetPublicName(string user_login)
        {
            IdentityResult result = null;
            string username = null;
            string exMsg = null;

            try
            {
                username = await _repo.GetPublicUsername(user_login);

                return Ok(username);
            }
            catch (Exception ex)
            {
                exMsg = ex.Message;
                result = new IdentityResult(exMsg);
            }

            IHttpActionResult errorResult = GetErrorResult(result);
                return errorResult;
        }

        [Authorize]
        [Route("GetCurrentBalance")]
        public async Task<IHttpActionResult> GetCurrentBalance(string user_login)
        {
            IdentityResult result = null;
            int balance = default(int);
            string exMsg = null;

            try
            {
                balance = await _repo.GetUserBalance(user_login);

                return Ok(balance);
            }
            catch (Exception ex)
            {
                exMsg = ex.Message;
                result = new IdentityResult(exMsg);
            }

            IHttpActionResult errorResult = GetErrorResult(result);

            return errorResult;
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
                return InternalServerError();

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                        ModelState.AddModelError("", error);
                }

                if (ModelState.IsValid)
                    return BadRequest();

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
