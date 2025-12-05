using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "restore-snapshot-tier")]
public record AwsEc2RestoreSnapshotTierOptions(
[property: CliOption("--snapshot-id")] string SnapshotId
) : AwsOptions
{
    [CliOption("--temporary-restore-days")]
    public int? TemporaryRestoreDays { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}