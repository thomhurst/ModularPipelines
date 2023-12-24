using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "describe-elastic-load-balancers")]
public record AwsOpsworksDescribeElasticLoadBalancersOptions : AwsOptions
{
    [CommandSwitch("--stack-id")]
    public string? StackId { get; set; }

    [CommandSwitch("--layer-ids")]
    public string[]? LayerIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}