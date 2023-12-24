using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-volume")]
public record AwsEc2CreateVolumeOptions(
[property: CommandSwitch("--availability-zone")] string AvailabilityZone
) : AwsOptions
{
    [CommandSwitch("--iops")]
    public int? Iops { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--outpost-arn")]
    public string? OutpostArn { get; set; }

    [CommandSwitch("--size")]
    public int? Size { get; set; }

    [CommandSwitch("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CommandSwitch("--volume-type")]
    public string? VolumeType { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--throughput")]
    public int? Throughput { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}