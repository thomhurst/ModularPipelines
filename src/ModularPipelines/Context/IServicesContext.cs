using Microsoft.Extensions.Configuration;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to dependency injection and configuration.
/// </summary>
public interface IServicesContext
{
    /// <summary>
    /// Resolves a required service from the DI container.
    /// </summary>
    /// <typeparam name="T">The service type to resolve.</typeparam>
    /// <returns>The resolved service instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the service is not registered.</exception>
    T GetRequiredService<T>() where T : class;

    /// <summary>
    /// Tries to resolve a service from the DI container.
    /// </summary>
    /// <typeparam name="T">The service type to resolve.</typeparam>
    /// <returns>The resolved service instance, or <see langword="null"/> when it is not registered.</returns>
    T? GetService<T>() where T : class;

    /// <summary>
    /// Application configuration.
    /// </summary>
    IConfiguration Configuration { get; }

    /// <summary>
    /// Pipeline options.
    /// </summary>
    PipelineOptions Options { get; }
}
