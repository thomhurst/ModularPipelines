using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "delete-serverless-cache-snapshot")]
public record AwsElasticacheDeleteServerlessCacheSnapshotOptions(
[property: CommandSwitch("--serverless-cache-snapshot-name")] string ServerlessCacheSnapshotName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}