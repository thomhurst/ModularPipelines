using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "allocate-hosts")]
public record AwsEc2AllocateHostsOptions(
[property: CommandSwitch("--availability-zone")] string AvailabilityZone
) : AwsOptions
{
    [CommandSwitch("--auto-placement")]
    public string? AutoPlacement { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--instance-family")]
    public string? InstanceFamily { get; set; }

    [CommandSwitch("--quantity")]
    public int? Quantity { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--host-recovery")]
    public string? HostRecovery { get; set; }

    [CommandSwitch("--outpost-arn")]
    public string? OutpostArn { get; set; }

    [CommandSwitch("--host-maintenance")]
    public string? HostMaintenance { get; set; }

    [CommandSwitch("--asset-ids")]
    public string[]? AssetIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}