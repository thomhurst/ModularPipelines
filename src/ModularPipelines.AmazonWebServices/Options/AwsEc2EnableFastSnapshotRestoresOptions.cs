using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "enable-fast-snapshot-restores")]
public record AwsEc2EnableFastSnapshotRestoresOptions(
[property: CliOption("--availability-zones")] string[] AvailabilityZones,
[property: CliOption("--source-snapshot-ids")] string[] SourceSnapshotIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}