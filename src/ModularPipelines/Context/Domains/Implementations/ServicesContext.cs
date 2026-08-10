using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides access to dependency injection and configuration.
/// </summary>
internal class ServicesContext : IServicesContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<PipelineOptions> _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServicesContext"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="options">The pipeline options.</param>
    public ServicesContext(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        IOptions<PipelineOptions> options)
    {
        _serviceProvider = serviceProvider;
        Configuration = configuration;
        _options = options;
    }

    /// <inheritdoc />
    public T Get<T>() where T : class =>
        _serviceProvider.GetService<T>() ?? throw CreateMissingServiceException<T>();

    /// <inheritdoc />
    public T? TryGet<T>() where T : class => _serviceProvider.GetService<T>();

    /// <inheritdoc />
    public IConfiguration Configuration { get; }

    /// <inheritdoc />
    public PipelineOptions Options => _options.Value;

    private static InvalidOperationException CreateMissingServiceException<T>()
    {
        var serviceType = typeof(T);
        var assemblyName = serviceType.Assembly.GetName().Name;
        const string packagePrefix = "ModularPipelines.";

        if (assemblyName?.StartsWith(packagePrefix, StringComparison.Ordinal) == true)
        {
            var integrationName = assemblyName[packagePrefix.Length..];
            return new InvalidOperationException(
                $"Tool integration service '{serviceType.FullName}' is not registered. " +
                $"Reference the {assemblyName} package and ensure Register{integrationName}Context() runs. " +
                "When using Native AOT, register tool integrations explicitly.");
        }

        return new InvalidOperationException(
            $"Service '{serviceType.FullName}' is not registered. " +
            "Register it with the pipeline service collection before building the pipeline.");
    }
}
