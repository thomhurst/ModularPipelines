using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "create-cache-subnet-group")]
public record AwsElasticacheCreateCacheSubnetGroupOptions(
[property: CommandSwitch("--cache-subnet-group-name")] string CacheSubnetGroupName,
[property: CommandSwitch("--cache-subnet-group-description")] string CacheSubnetGroupDescription,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}