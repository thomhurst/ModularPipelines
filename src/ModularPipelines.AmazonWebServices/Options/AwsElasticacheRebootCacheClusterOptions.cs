using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "reboot-cache-cluster")]
public record AwsElasticacheRebootCacheClusterOptions(
[property: CliOption("--cache-cluster-id")] string CacheClusterId,
[property: CliOption("--cache-node-ids-to-reboot")] string[] CacheNodeIdsToReboot
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}