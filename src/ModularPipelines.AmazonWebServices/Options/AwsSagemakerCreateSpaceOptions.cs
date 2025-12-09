using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-space")]
public record AwsSagemakerCreateSpaceOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--space-name")] string SpaceName
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--space-settings")]
    public string? SpaceSettings { get; set; }

    [CliOption("--ownership-settings")]
    public string? OwnershipSettings { get; set; }

    [CliOption("--space-sharing-settings")]
    public string? SpaceSharingSettings { get; set; }

    [CliOption("--space-display-name")]
    public string? SpaceDisplayName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}