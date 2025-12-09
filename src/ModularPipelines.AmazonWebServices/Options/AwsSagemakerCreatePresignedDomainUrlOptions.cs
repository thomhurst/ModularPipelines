using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-presigned-domain-url")]
public record AwsSagemakerCreatePresignedDomainUrlOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--user-profile-name")] string UserProfileName
) : AwsOptions
{
    [CliOption("--session-expiration-duration-in-seconds")]
    public int? SessionExpirationDurationInSeconds { get; set; }

    [CliOption("--expires-in-seconds")]
    public int? ExpiresInSeconds { get; set; }

    [CliOption("--space-name")]
    public string? SpaceName { get; set; }

    [CliOption("--landing-uri")]
    public string? LandingUri { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}