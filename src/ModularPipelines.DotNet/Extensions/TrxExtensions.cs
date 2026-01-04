using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Parsers.Trx;
using ModularPipelines.DotNet.Services;
using ModularPipelines.Engine;

namespace ModularPipelines.DotNet.Extensions;

/// <summary>
/// Provides extension methods for integrating .NET CLI functionality into the ModularPipelines framework.
/// </summary>
[ExcludeFromCodeCoverage]
public static class TrxExtensions
{
    /// <summary>
    /// Automatically registers the .NET context services with the ModularPipelines framework.
    /// This method is called by the module initializer and should not be called directly.
    /// </summary>
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterTrxContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterTrxContext(collection));
    }

    /// <summary>
    /// Registers .NET CLI services with the dependency injection container.
    /// This includes services for running dotnet commands such as build, test, pack, publish, and NuGet operations.
    /// </summary>
    /// <param name="services">The service collection to add the .NET services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection RegisterTrxContext(this IServiceCollection services)
    {
        services.TryAddScoped<ITrxParser, TrxParser>();
        services.TryAddScoped<ITrx, Trx>();

        return services;
    }

    /// <summary>
    /// Gets the TRX (Test Results XML) parser service from the pipeline context.
    /// This provides access to parse and analyze .NET test result files.
    /// </summary>
    /// <param name="context">The pipeline hook context.</param>
    /// <returns>The <see cref="ITrx"/> service for parsing TRX test result files.</returns>
    public static ITrx Trx(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<ITrx>();
}