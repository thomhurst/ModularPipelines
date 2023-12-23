using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memorydb", "copy-snapshot")]
public record AwsMemorydbCopySnapshotOptions(
[property: CommandSwitch("--source-snapshot-name")] string SourceSnapshotName,
[property: CommandSwitch("--target-snapshot-name")] string TargetSnapshotName
) : AwsOptions
{
    [CommandSwitch("--target-bucket")]
    public string? TargetBucket { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}