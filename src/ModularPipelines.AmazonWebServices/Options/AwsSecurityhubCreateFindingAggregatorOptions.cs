using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "create-finding-aggregator")]
public record AwsSecurityhubCreateFindingAggregatorOptions(
[property: CliOption("--region-linking-mode")] string RegionLinkingMode
) : AwsOptions
{
    [CliOption("--regions")]
    public string[]? Regions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}