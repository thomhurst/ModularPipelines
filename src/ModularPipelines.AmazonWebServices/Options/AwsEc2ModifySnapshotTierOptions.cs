using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-snapshot-tier")]
public record AwsEc2ModifySnapshotTierOptions(
[property: CommandSwitch("--snapshot-id")] string SnapshotId
) : AwsOptions
{
    [CommandSwitch("--storage-tier")]
    public string? StorageTier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}