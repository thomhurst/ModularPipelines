using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "delete-cache-parameter-group")]
public record AwsElasticacheDeleteCacheParameterGroupOptions(
[property: CommandSwitch("--cache-parameter-group-name")] string CacheParameterGroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}