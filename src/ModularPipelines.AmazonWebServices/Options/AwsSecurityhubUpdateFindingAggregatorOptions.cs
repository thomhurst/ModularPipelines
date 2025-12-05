using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "update-finding-aggregator")]
public record AwsSecurityhubUpdateFindingAggregatorOptions(
[property: CliOption("--finding-aggregator-arn")] string FindingAggregatorArn,
[property: CliOption("--region-linking-mode")] string RegionLinkingMode
) : AwsOptions
{
    [CliOption("--regions")]
    public string[]? Regions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}