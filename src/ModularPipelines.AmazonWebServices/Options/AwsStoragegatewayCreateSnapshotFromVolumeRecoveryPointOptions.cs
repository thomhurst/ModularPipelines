using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "create-snapshot-from-volume-recovery-point")]
public record AwsStoragegatewayCreateSnapshotFromVolumeRecoveryPointOptions(
[property: CliOption("--volume-arn")] string VolumeArn,
[property: CliOption("--snapshot-description")] string SnapshotDescription
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}