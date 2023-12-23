using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "reboot-cache-cluster")]
public record AwsElasticacheRebootCacheClusterOptions(
[property: CommandSwitch("--cache-cluster-id")] string CacheClusterId,
[property: CommandSwitch("--cache-node-ids-to-reboot")] string[] CacheNodeIdsToReboot
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}