using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-volume")]
public record AwsEc2CreateVolumeOptions(
[property: CliOption("--availability-zone")] string AvailabilityZone
) : AwsOptions
{
    [CliOption("--iops")]
    public int? Iops { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--outpost-arn")]
    public string? OutpostArn { get; set; }

    [CliOption("--size")]
    public int? Size { get; set; }

    [CliOption("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CliOption("--volume-type")]
    public string? VolumeType { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--throughput")]
    public int? Throughput { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}