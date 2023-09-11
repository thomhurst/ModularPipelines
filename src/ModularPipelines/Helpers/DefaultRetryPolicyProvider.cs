using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Options;
using Polly;
using Polly.Retry;

namespace ModularPipelines.Helpers;

internal static class DefaultRetryPolicyProvider
{
    public static AsyncRetryPolicy<T> GetDefaultRetryPolicy<T>(IPipelineContext context)
    {
        var pipelineOptions = context.Get<IOptions<PipelineOptions>>();

        return Policy<T>.Handle<Exception>()
            .WaitAndRetryAsync(pipelineOptions?.Value?.DefaultRetryCount ?? 0,
                i => TimeSpan.FromMilliseconds(i * i * 100)
            );
    }
}