using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-control-config", "wait", "control-panel-deleted")]
public record AwsRoute53RecoveryControlConfigWaitControlPanelDeletedOptions(
[property: CliOption("--control-panel-arn")] string ControlPanelArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}