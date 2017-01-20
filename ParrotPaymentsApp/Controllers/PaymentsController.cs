using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ParrotPaymentsApp.Models;
using ParrotPaymentsApp.Infrastructure;

namespace ParrotPaymentsApp.Controllers
{
    [RoutePrefix("api/Payments")]
    public class PaymentsController : ApiController
    {
        [Authorize]
        [Route("")]
        public IHttpActionResult Get(string id)
        {
            //return Ok(new Models.PaymentOperation() { Amount = 100, Date = DateTime.Now, PostBalance = 200, CorrespondentId = "Yokozuna", SenderId = "Barbara" });

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Логин не указан!");

            PaymentVM[] result = null;

            using (var mng = new Infrastructure.PaymentsManager())
            {
                result = mng.GetPaymentsHistory(id, 0, 20);
            }

            //IHttpActionResult errorResult = GetErrorResult(result);

            //if (errorResult != null)
            //{
            //    return errorResult;
            //}

            return Ok(result);
        }

        [Authorize]
        [Route("Send")]
        public IHttpActionResult Send(string sender_login, string receiver_login, int amount /*PaymentOperation blank*/)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (string.IsNullOrWhiteSpace(sender_login) || string.IsNullOrWhiteSpace(receiver_login))
                return BadRequest("Один из участников операции не указан!");

            if (amount < 1)
                return BadRequest("Некорректная сумма перевода!");

            PaymentOperationResult result = null;

            using (var mng = new Infrastructure.PaymentsManager())
            {
                result = mng.CreateNewPayment(sender_login, receiver_login, amount);
            }

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();

            //return Ok(new Models.PaymentOperation() { Amount = 100, Date = DateTime.Now, PostBalance = 200, CorrespondentId = "Yokozuna", SenderId = "Barbara" });
        }

        [Authorize]
        [Route("History")]
        public  IHttpActionResult History(string login, int type, int count = 20)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (string.IsNullOrWhiteSpace(login))
                return BadRequest("Логин не указан!");

            PaymentVM[] result = null;

            using (var mng = new Infrastructure.PaymentsManager())
            {
                result = mng.GetPaymentsHistory(login, type, count);
            }

            //IHttpActionResult errorResult = GetErrorResult(result);

            //if (errorResult != null)
            //{
            //    return errorResult;
            //}

            return Ok(result);

            //return Ok(new Models.PaymentOperation() { Amount = 100, Date = DateTime.Now, PostBalance = 200, CorrespondentId = "Yokozuna", SenderId = "Barbara" });
        }

        private IHttpActionResult GetErrorResult(PaymentOperationResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.LastException != null)
                {
                    ModelState.AddModelError("", result.LastException.Message);
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

    }
}
