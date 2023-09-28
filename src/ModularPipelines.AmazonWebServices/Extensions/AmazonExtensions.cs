using System.Diagnostics.CodeAnalysis;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.AmazonWebServices;
using ModularPipelines.Context;

namespace ModularPipelines.Amazon.Extensions;

[ExcludeFromCodeCoverage]
public static class AmazonExtensions
{
    public static IServiceCollection RegisterAmazonContext(this IServiceCollection services)
    {
        services.TryAddScoped<IAmazon, AmazonWebServices.Amazon>();
        services.TryAddScoped<IAmazonProvisioner, AmazonProvisioner>();
        services.TryAddScoped<S3>();
        return services;
    }

    public static IServiceCollection AddS3Client(this IServiceCollection services, AmazonS3Client armClient)
    {
        RegisterAmazonContext(services);
        return services.AddSingleton(armClient);
    }

    public static IAmazon Amazon(this IPipelineContext context) => context.ServiceProvider.GetRequiredService<IAmazon>();
}
