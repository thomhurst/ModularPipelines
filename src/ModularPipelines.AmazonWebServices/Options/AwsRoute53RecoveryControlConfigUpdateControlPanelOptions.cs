using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-control-config", "update-control-panel")]
public record AwsRoute53RecoveryControlConfigUpdateControlPanelOptions(
[property: CommandSwitch("--control-panel-arn")] string ControlPanelArn,
[property: CommandSwitch("--control-panel-name")] string ControlPanelName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}