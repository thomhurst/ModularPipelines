using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "delete-traffic-distribution-group")]
public record AwsConnectDeleteTrafficDistributionGroupOptions(
[property: CommandSwitch("--traffic-distribution-group-id")] string TrafficDistributionGroupId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}