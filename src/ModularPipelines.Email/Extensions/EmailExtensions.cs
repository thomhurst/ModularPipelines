using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Email.Extensions;

[ExcludeFromCodeCoverage]
public static class EmailExtensions
{
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterEmailContext(this IServiceCollection services)
    {
        services.TryAddScoped<IEmail, Email>();
        return services;
    }
}
