using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-presigned-domain-url")]
public record AwsSagemakerCreatePresignedDomainUrlOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--user-profile-name")] string UserProfileName
) : AwsOptions
{
    [CommandSwitch("--session-expiration-duration-in-seconds")]
    public int? SessionExpirationDurationInSeconds { get; set; }

    [CommandSwitch("--expires-in-seconds")]
    public int? ExpiresInSeconds { get; set; }

    [CommandSwitch("--space-name")]
    public string? SpaceName { get; set; }

    [CommandSwitch("--landing-uri")]
    public string? LandingUri { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}