using Microsoft.Extensions.Configuration;
using ModularPipelines.Options;

namespace ModularPipelines.Context.Domains;

/// <summary>
/// Provides access to dependency injection and configuration.
/// </summary>
public interface IServicesContext
{
    /// <summary>
    /// Resolve a service from the DI container.
    /// </summary>
    /// <typeparam name="T">The service type to resolve.</typeparam>
    /// <returns>The resolved service instance.</returns>
    T Get<T>() where T : class;

    /// <summary>
    /// Application configuration.
    /// </summary>
    IConfiguration Configuration { get; }

    /// <summary>
    /// Pipeline options.
    /// </summary>
    PipelineOptions Options { get; }
}
