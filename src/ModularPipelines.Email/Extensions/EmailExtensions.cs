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

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    [global::System.Obsolete("Use context.Tools.Get<global::ModularPipelines.Email.IEmail>().")]
    public static IEmail Email(this IPipelineContext context) => context.Services.GetRequiredService<IEmail>();
}
