using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "restore-snapshot-from-recycle-bin")]
public record AwsEc2RestoreSnapshotFromRecycleBinOptions(
[property: CommandSwitch("--snapshot-id")] string SnapshotId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}