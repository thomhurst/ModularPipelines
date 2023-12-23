using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-hosts")]
public record AwsEc2ModifyHostsOptions(
[property: CommandSwitch("--host-ids")] string[] HostIds
) : AwsOptions
{
    [CommandSwitch("--auto-placement")]
    public string? AutoPlacement { get; set; }

    [CommandSwitch("--host-recovery")]
    public string? HostRecovery { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--instance-family")]
    public string? InstanceFamily { get; set; }

    [CommandSwitch("--host-maintenance")]
    public string? HostMaintenance { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}