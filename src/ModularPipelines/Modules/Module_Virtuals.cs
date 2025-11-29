using ModularPipelines.Helpers;
using ModularPipelines.Modules.Behaviors;
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
    /// If the module implements <see cref="IRetryable{T}"/>, delegates to the interface.
    /// </summary>
    protected virtual AsyncRetryPolicy<T?> RetryPolicy
    {
        get
        {
            if (this is IRetryable<T> retryable)
            {
                return retryable.GetRetryPolicy(Context);
            }

            return DefaultRetryPolicyProvider.GetDefaultRetryPolicy<T?>(Context);
        }
    }
}
