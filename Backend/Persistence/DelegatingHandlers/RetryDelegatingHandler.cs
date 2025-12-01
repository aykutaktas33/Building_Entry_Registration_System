using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Polly;
using Polly.Retry;

namespace Persistence.DelegatingHandlers
{
    public class RetryDelegatingHandler : DelegatingHandler
    {
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .RetryAsync(2);

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var policyResult = await _retryPolicy.ExecuteAndCaptureAsync(() => base.SendAsync(request, cancellationToken));

            if (policyResult.Outcome == OutcomeType.Failure)
            {
                throw new HttpRequestException("Something went wrong", policyResult.FinalException);
            }

            return policyResult.Result;
        }
    }
}
