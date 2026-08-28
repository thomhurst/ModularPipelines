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

    public static IEmail Email(this IPipelineContext context) => context.Services.GetRequiredService<IEmail>();
}