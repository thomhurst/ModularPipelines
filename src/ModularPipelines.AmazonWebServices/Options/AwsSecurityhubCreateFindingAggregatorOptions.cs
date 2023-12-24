using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "create-finding-aggregator")]
public record AwsSecurityhubCreateFindingAggregatorOptions(
[property: CommandSwitch("--region-linking-mode")] string RegionLinkingMode
) : AwsOptions
{
    [CommandSwitch("--regions")]
    public string[]? Regions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}