using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-control-config", "wait", "control-panel-created")]
public record AwsRoute53RecoveryControlConfigWaitControlPanelCreatedOptions(
[property: CommandSwitch("--control-panel-arn")] string ControlPanelArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}