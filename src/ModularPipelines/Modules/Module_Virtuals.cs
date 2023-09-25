using ModularPipelines.Helpers;
using Polly.Retry;

namespace ModularPipelines.Modules;

public partial class Module<T>
{
    protected virtual AsyncRetryPolicy<T?> RetryPolicy
        => DefaultRetryPolicyProvider.GetDefaultRetryPolicy<T?>(Context);
}
