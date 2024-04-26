using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace App.BetterBoard.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class ApiKeyAuthorizationAttribute : AuthorizationFilterAttribute
    {
        private readonly string _apiKey;

        public ApiKeyAuthorizationAttribute()
        {
            _apiKey = ConfigurationManager.AppSettings["TransactionApiKey"];
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            IEnumerable<string> apiKeyHeaders = Enumerable.Empty<string>();
            if (!actionContext.Request.Headers.TryGetValues("X-Api-Key", out apiKeyHeaders))
            {
                actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Missing API Key");
                throw new UnauthorizedAccessException("Missing API Key.");
            }

            var apiKey = apiKeyHeaders.FirstOrDefault();
            if (!IsValidApiKey(apiKey))
            {
                actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid API Key");
                throw new UnauthorizedAccessException("Invalid API Key.");
            }
        }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext,
                                                  CancellationToken cancellationToken)
        {
            try
            {
                OnAuthorization(actionContext);
            }
            catch (Exception exception)
            {
                return Task.FromException(exception);
            }

            return Task.CompletedTask;
        }

        private bool IsValidApiKey(string apiKey)
        {
            if (apiKey == null)
                return false;
            return apiKey == _apiKey;
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext,
                                                                                       CancellationToken cancellationToken,
                                                                                       Func<Task<HttpResponseMessage>> continuation)
        {
            if (actionContext == null) 
                throw new ArgumentNullException(nameof(actionContext));
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));

            return ExecuteAuthorizationFilterAsyncCore(actionContext,
                                                       cancellationToken,
                                                       continuation);
        }

        private async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsyncCore(HttpActionContext actionContext,
                                                                                    CancellationToken cancellationToken,
                                                                                    Func<Task<HttpResponseMessage>> continuation)
        {
            await OnAuthorizationAsync(actionContext, cancellationToken);

            if (actionContext != null)
            {
                return actionContext.Response;
            }

            return await continuation();
        }
    }
}