using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "update-finding-aggregator")]
public record AwsSecurityhubUpdateFindingAggregatorOptions(
[property: CommandSwitch("--finding-aggregator-arn")] string FindingAggregatorArn,
[property: CommandSwitch("--region-linking-mode")] string RegionLinkingMode
) : AwsOptions
{
    [CommandSwitch("--regions")]
    public string[]? Regions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}