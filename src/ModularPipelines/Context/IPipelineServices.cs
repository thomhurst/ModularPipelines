using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to dependency injection and configuration services.
/// </summary>
/// <remarks>
/// This interface groups DI and configuration-related members for better Interface Segregation.
/// </remarks>
public interface IPipelineServices
{
    /// <summary>
    /// Gets the service provider orchestrating DI within the pipeline.
    /// </summary>
    /// <remarks>
    /// Use the generic Get&lt;T&gt;() method instead of accessing ServiceProvider directly
    /// when possible for better type safety.
    /// </remarks>
    IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Helper method for retrieving services from the Service Provider.
    /// </summary>
    /// <typeparam name="T">Type to retrieve.</typeparam>
    /// <returns>The service instance, or null if not registered.</returns>
    T? Get<T>();

    /// <summary>
    /// Gets the configuration powering the pipeline.
    /// </summary>
    /// <remarks>
    /// Configuration is loaded from appsettings.json, environment variables, user secrets, and command line.
    /// </remarks>
    IConfiguration Configuration { get; }

    /// <summary>
    /// Gets the pipeline's options.
    /// </summary>
    IOptions<PipelineOptions> PipelineOptions { get; }
}
