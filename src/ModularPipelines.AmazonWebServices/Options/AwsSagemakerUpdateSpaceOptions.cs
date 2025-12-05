using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-space")]
public record AwsSagemakerUpdateSpaceOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--space-name")] string SpaceName
) : AwsOptions
{
    [CliOption("--space-settings")]
    public string? SpaceSettings { get; set; }

    [CliOption("--space-display-name")]
    public string? SpaceDisplayName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}