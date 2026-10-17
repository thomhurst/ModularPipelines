using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Extensions;

/// <summary>
/// Extension methods for the handwritten AWS SDK services.
/// </summary>
public static class AmazonExtensions
{
    /// <summary>
    /// Registers scoped AWS SDK services and their AWS CLI dependency.
    /// Supply an <see cref="AmazonS3Client"/> before resolving these services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterAmazonContext(this IServiceCollection services)
    {
        services.RegisterAwsContext();
        services.TryAddScoped<IAmazon, Amazon>();
        services.TryAddScoped<IAmazonProvisioner, AmazonProvisioner>();
        // The container or caller owns the shared client, not each scoped wrapper.
        services.TryAddScoped<IS3>(provider => new S3(provider.GetRequiredService<AmazonS3Client>(), ownsClient: false));
        return services;
    }

    /// <summary>
    /// Registers AWS services with a caller-owned S3 client shared across scopes.
    /// The caller remains responsible for disposing the client.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="client">The S3 client to use.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddS3Client(this IServiceCollection services, AmazonS3Client client)
    {
        services.RegisterAmazonContext();
        return services.AddSingleton(client);
    }

    /// <summary>
    /// Gets the AWS SDK and CLI services from the current pipeline scope.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns>The scoped AWS services.</returns>
    public static IAmazon Amazon(this IPipelineContext context) => context.Services.GetRequiredService<IAmazon>();
}
