using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "modify-cache-subnet-group")]
public record AwsElasticacheModifyCacheSubnetGroupOptions(
[property: CommandSwitch("--cache-subnet-group-name")] string CacheSubnetGroupName
) : AwsOptions
{
    [CommandSwitch("--cache-subnet-group-description")]
    public string? CacheSubnetGroupDescription { get; set; }

    [CommandSwitch("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}