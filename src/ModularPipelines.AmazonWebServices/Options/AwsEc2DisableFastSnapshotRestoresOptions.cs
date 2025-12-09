using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "disable-fast-snapshot-restores")]
public record AwsEc2DisableFastSnapshotRestoresOptions(
[property: CliOption("--availability-zones")] string[] AvailabilityZones,
[property: CliOption("--source-snapshot-ids")] string[] SourceSnapshotIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}