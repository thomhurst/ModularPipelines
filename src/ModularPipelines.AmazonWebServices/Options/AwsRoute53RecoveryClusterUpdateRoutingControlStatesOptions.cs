using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-cluster", "update-routing-control-states")]
public record AwsRoute53RecoveryClusterUpdateRoutingControlStatesOptions(
[property: CommandSwitch("--update-routing-control-state-entries")] string[] UpdateRoutingControlStateEntries
) : AwsOptions
{
    [CommandSwitch("--safety-rules-to-override")]
    public string[]? SafetyRulesToOverride { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}