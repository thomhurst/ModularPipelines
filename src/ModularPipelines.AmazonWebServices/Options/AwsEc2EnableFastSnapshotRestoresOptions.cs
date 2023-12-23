using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "enable-fast-snapshot-restores")]
public record AwsEc2EnableFastSnapshotRestoresOptions(
[property: CommandSwitch("--availability-zones")] string[] AvailabilityZones,
[property: CommandSwitch("--source-snapshot-ids")] string[] SourceSnapshotIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}