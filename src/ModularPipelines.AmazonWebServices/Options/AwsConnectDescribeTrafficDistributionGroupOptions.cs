using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "describe-traffic-distribution-group")]
public record AwsConnectDescribeTrafficDistributionGroupOptions(
[property: CommandSwitch("--traffic-distribution-group-id")] string TrafficDistributionGroupId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}