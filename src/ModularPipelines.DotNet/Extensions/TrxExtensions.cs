using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Parsers.Trx;
using ModularPipelines.DotNet.Services;

namespace ModularPipelines.DotNet.Extensions;

/// <summary>
/// Provides extension methods for integrating TRX test result parsing into the ModularPipelines framework.
/// </summary>
[ExcludeFromCodeCoverage]
public static class TrxExtensions
{
    /// <summary>
    /// Registers TRX test result parsing services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add the TRX services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterTrxContext(this IServiceCollection services)
    {
        services.TryAddScoped<ITrxParser, TrxParser>();
        services.TryAddScoped<ITrx, Trx>();

        return services;
    }
}
