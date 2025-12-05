using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "describe-load-based-auto-scaling")]
public record AwsOpsworksDescribeLoadBasedAutoScalingOptions(
[property: CliOption("--layer-ids")] string[] LayerIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}