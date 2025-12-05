using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "lock-snapshot")]
public record AwsEc2LockSnapshotOptions(
[property: CliOption("--snapshot-id")] string SnapshotId,
[property: CliOption("--lock-mode")] string LockMode
) : AwsOptions
{
    [CliOption("--cool-off-period")]
    public int? CoolOffPeriod { get; set; }

    [CliOption("--lock-duration")]
    public int? LockDuration { get; set; }

    [CliOption("--expiration-date")]
    public long? ExpirationDate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}