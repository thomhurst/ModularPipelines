using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "allocate-hosts")]
public record AwsEc2AllocateHostsOptions(
[property: CliOption("--availability-zone")] string AvailabilityZone
) : AwsOptions
{
    [CliOption("--auto-placement")]
    public string? AutoPlacement { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--instance-family")]
    public string? InstanceFamily { get; set; }

    [CliOption("--quantity")]
    public int? Quantity { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--host-recovery")]
    public string? HostRecovery { get; set; }

    [CliOption("--outpost-arn")]
    public string? OutpostArn { get; set; }

    [CliOption("--host-maintenance")]
    public string? HostMaintenance { get; set; }

    [CliOption("--asset-ids")]
    public string[]? AssetIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}