using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.Interfaces;

/// <summary>
/// Manages the disposal of dependency injection scopes.
/// </summary>
public interface IScopeDisposer : IDisposable
{
    /// <summary>
    /// Gets all service scopes that need to be disposed.
    /// </summary>
    /// <returns>An enumerable collection of service scopes.</returns>
    IEnumerable<IServiceScope> GetScopes();

    /// <summary>
    /// Disposes all registered service scopes.
    /// </summary>
    void IDisposable.Dispose()
    {
        foreach (var serviceScope in GetScopes())
        {
            serviceScope.Dispose();
        }
    }
}
