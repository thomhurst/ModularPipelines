using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-control-config", "wait", "control-panel-deleted")]
public record AwsRoute53RecoveryControlConfigWaitControlPanelDeletedOptions(
[property: CommandSwitch("--control-panel-arn")] string ControlPanelArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}