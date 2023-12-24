using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "update-instance")]
public record AwsOpsworksUpdateInstanceOptions(
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--layer-ids")]
    public string[]? LayerIds { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--auto-scaling-type")]
    public string? AutoScalingType { get; set; }

    [CommandSwitch("--hostname")]
    public string? Hostname { get; set; }

    [CommandSwitch("--os")]
    public string? Os { get; set; }

    [CommandSwitch("--ami-id")]
    public string? AmiId { get; set; }

    [CommandSwitch("--ssh-key-name")]
    public string? SshKeyName { get; set; }

    [CommandSwitch("--architecture")]
    public string? Architecture { get; set; }

    [CommandSwitch("--agent-version")]
    public string? AgentVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}