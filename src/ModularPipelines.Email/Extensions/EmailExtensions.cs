using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Email.Extensions;

[ExcludeFromCodeCoverage]
public static class EmailExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterEmailContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterEmailContext(collection));
    }

    public static IServiceCollection RegisterEmailContext(this IServiceCollection services)
    {
        services.TryAddScoped<IEmail, Email>();
        return services;
    }

    public static IEmail Email(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<IEmail>();
}