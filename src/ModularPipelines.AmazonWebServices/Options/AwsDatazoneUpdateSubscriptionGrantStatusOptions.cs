using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "update-subscription-grant-status")]
public record AwsDatazoneUpdateSubscriptionGrantStatusOptions(
[property: CliOption("--asset-identifier")] string AssetIdentifier,
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--failure-cause")]
    public string? FailureCause { get; set; }

    [CliOption("--target-name")]
    public string? TargetName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}