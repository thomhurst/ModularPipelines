using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "copy-serverless-cache-snapshot")]
public record AwsElasticacheCopyServerlessCacheSnapshotOptions(
[property: CommandSwitch("--source-serverless-cache-snapshot-name")] string SourceServerlessCacheSnapshotName,
[property: CommandSwitch("--target-serverless-cache-snapshot-name")] string TargetServerlessCacheSnapshotName
) : AwsOptions
{
    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}