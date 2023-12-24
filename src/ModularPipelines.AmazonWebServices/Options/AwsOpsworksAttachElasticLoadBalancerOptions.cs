using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "attach-elastic-load-balancer")]
public record AwsOpsworksAttachElasticLoadBalancerOptions(
[property: CommandSwitch("--elastic-load-balancer-name")] string ElasticLoadBalancerName,
[property: CommandSwitch("--layer-id")] string LayerId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}