using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-control-config", "create-control-panel")]
public record AwsRoute53RecoveryControlConfigCreateControlPanelOptions(
[property: CommandSwitch("--cluster-arn")] string ClusterArn,
[property: CommandSwitch("--control-panel-name")] string ControlPanelName
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}