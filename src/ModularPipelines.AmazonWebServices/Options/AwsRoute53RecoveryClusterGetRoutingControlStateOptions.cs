using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-cluster", "get-routing-control-state")]
public record AwsRoute53RecoveryClusterGetRoutingControlStateOptions(
[property: CommandSwitch("--routing-control-arn")] string RoutingControlArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}