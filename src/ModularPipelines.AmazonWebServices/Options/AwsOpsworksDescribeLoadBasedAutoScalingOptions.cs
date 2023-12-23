using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "describe-load-based-auto-scaling")]
public record AwsOpsworksDescribeLoadBasedAutoScalingOptions(
[property: CommandSwitch("--layer-ids")] string[] LayerIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}