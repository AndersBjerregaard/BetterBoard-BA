using App.Betterboard.Api.Model;
using System;
using App.Betterboard.Api.Services.Interfaces;
using App.BetterBoard.Api.Builders;
using App.BetterBoard.Api.Model;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using App.BetterBoard.Api.Exceptions;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Bson;
using System.Configuration;
using App.BetterBoard.Api.Attributes;
using System.Net.Http;
using App.BetterBoard.Api.Services.Transaction.Interfaces;
using App.BetterBoard.Api.Exceptions.Transaction;
using App.BetterBoard.Api.Helpers;
using BetterBoard.Core.Model;

namespace App.BetterBoard.Api
{
    [RoutePrefix("api/v1")]
    public class TestTransactionController : ApiController
    {
        private readonly ITransactionService _transactionService;

        public TestTransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        [Route(@"testtransaction/start")]
        [ApiKeyAuthorization]
        public async Task<IHttpActionResult> Start([FromBody] TestRunIdentifier runIdentifier)
        {
            try
            {
                await _transactionService.Seed(runIdentifier);
            }
            catch (Exception e)
            {
                string returnMessage = ExceptionMessageHelper.ConcatenateExceptionMessage(e);

                if (e.GetType() == typeof(InvalidTransactionException))
                {
                    return BadRequest(returnMessage);
                }

                return InternalServerError(e);
            }
            return StatusCode(HttpStatusCode.Created);
        }

        [HttpDelete]
        [Route(@"testtransaction/cleanup")]
        [ApiKeyAuthorization]
        public async Task<IHttpActionResult> CleanUp([FromBody] TestRunIdentifier runIdentifier)
        {
            try
            {
                await _transactionService.CleanUp(runIdentifier);
            }
            catch (Exception e)
            {
                string returnMessage = ExceptionMessageHelper.ConcatenateExceptionMessage(e);

                if (e.GetType() == typeof(InvalidTransactionException))
                {
                    return BadRequest(returnMessage);
                }

                return InternalServerError(e);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}