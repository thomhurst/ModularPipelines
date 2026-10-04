using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;

namespace ModularPipelines.Ftp;

[ExcludeFromCodeCoverage]
public static class FtpExtensions
{
    [ModularPipelinesIntegration]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static IServiceCollection RegisterFtpContext(this IServiceCollection services)
    {
        services.TryAddScoped<IFtp, Ftp>();
        return services;
    }
}
