using ModularPipelines.Helpers;
using Polly.Retry;

namespace ModularPipelines.Modules;

/// <summary>
/// An independent module used to perform an action, and optionally return some data, which can be used within other modules. This is the base class from which all custom modules should inherit.
/// </summary>
/// <typeparam name="T">The data to return which can be used within other modules, which is returned from its ExecuteAsync method..</typeparam>
public partial class Module<T>
{
    /// <summary>
    /// Gets a retry policy used to control how and if the module should retry if it fails.
    /// </summary>
    protected virtual AsyncRetryPolicy<T?> RetryPolicy
        => DefaultRetryPolicyProvider.GetDefaultRetryPolicy<T?>(Context);
}