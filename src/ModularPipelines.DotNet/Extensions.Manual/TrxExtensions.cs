using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Parsers.Trx;
using ModularPipelines.DotNet.Services;

namespace ModularPipelines.DotNet.Extensions;

/// <summary>
/// Provides extension methods for integrating .NET CLI functionality into the ModularPipelines framework.
/// </summary>
[ExcludeFromCodeCoverage]
public static class TrxExtensions
{
    /// <summary>
    /// Registers .NET CLI services with the dependency injection container.
    /// This includes services for running dotnet commands such as build, test, pack, publish, and NuGet operations.
    /// </summary>
    /// <param name="services">The service collection to add the .NET services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterTrxContext(this IServiceCollection services)
    {
        services.TryAddScoped<ITrxParser, TrxParser>();
        services.TryAddScoped<ITrx, Trx>();

        return services;
    }
}
