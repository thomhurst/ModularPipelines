using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Ftp.Extensions;

[ExcludeFromCodeCoverage]
public static class FtpExtensions
{
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterFtpContext(this IServiceCollection services)
    {
        services.TryAddScoped<IFtp, Ftp>();
        return services;
    }

    public static IFtp Ftp(this IPipelineContext context) => context.Services.GetRequiredService<IFtp>();
}