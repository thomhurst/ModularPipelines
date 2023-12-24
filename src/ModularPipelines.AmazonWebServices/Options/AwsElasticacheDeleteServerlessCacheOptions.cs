using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "delete-serverless-cache")]
public record AwsElasticacheDeleteServerlessCacheOptions(
[property: CommandSwitch("--serverless-cache-name")] string ServerlessCacheName
) : AwsOptions
{
    [CommandSwitch("--final-snapshot-name")]
    public string? FinalSnapshotName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}