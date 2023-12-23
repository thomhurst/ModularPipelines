using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "lock-snapshot")]
public record AwsEc2LockSnapshotOptions(
[property: CommandSwitch("--snapshot-id")] string SnapshotId,
[property: CommandSwitch("--lock-mode")] string LockMode
) : AwsOptions
{
    [CommandSwitch("--cool-off-period")]
    public int? CoolOffPeriod { get; set; }

    [CommandSwitch("--lock-duration")]
    public int? LockDuration { get; set; }

    [CommandSwitch("--expiration-date")]
    public long? ExpirationDate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}