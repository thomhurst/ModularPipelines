using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-cluster", "update-routing-control-state")]
public record AwsRoute53RecoveryClusterUpdateRoutingControlStateOptions(
[property: CommandSwitch("--routing-control-arn")] string RoutingControlArn,
[property: CommandSwitch("--routing-control-state")] string RoutingControlState
) : AwsOptions
{
    [CommandSwitch("--safety-rules-to-override")]
    public string[]? SafetyRulesToOverride { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}