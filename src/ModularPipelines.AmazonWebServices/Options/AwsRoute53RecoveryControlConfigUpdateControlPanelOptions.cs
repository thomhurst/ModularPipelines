using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-control-config", "update-control-panel")]
public record AwsRoute53RecoveryControlConfigUpdateControlPanelOptions(
[property: CliOption("--control-panel-arn")] string ControlPanelArn,
[property: CliOption("--control-panel-name")] string ControlPanelName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}