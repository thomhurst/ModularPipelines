using ModularPipelines.Context;
using Polly;
using Polly.Retry;

namespace ModularPipelines.Helpers;

internal static class DefaultRetryPolicyProvider
{
    public static AsyncRetryPolicy<T> GetDefaultRetryPolicy<T>(IPipelineContext context)
    {
        var retryCount = context.PipelineOptions.Value.DefaultRetryCount;

        return Policy<T>.Handle<Exception>()
            .WaitAndRetryAsync(retryCount,
                i => TimeSpan.FromMilliseconds(i * i * 100)
            );
    }
}