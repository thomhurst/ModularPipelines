using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "create-serverless-cache-snapshot")]
public record AwsElasticacheCreateServerlessCacheSnapshotOptions(
[property: CommandSwitch("--serverless-cache-snapshot-name")] string ServerlessCacheSnapshotName,
[property: CommandSwitch("--serverless-cache-name")] string ServerlessCacheName
) : AwsOptions
{
    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}