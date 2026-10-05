using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;

namespace ModularPipelines.Email;

[ExcludeFromCodeCoverage]
public static class EmailExtensions
{
    [ModularPipelinesIntegration]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static IServiceCollection RegisterEmailContext(this IServiceCollection services)
    {
        services.TryAddScoped<IEmail, Email>();
        return services;
    }
}
