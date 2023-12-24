using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "create-cache-parameter-group")]
public record AwsElasticacheCreateCacheParameterGroupOptions(
[property: CommandSwitch("--cache-parameter-group-name")] string CacheParameterGroupName,
[property: CommandSwitch("--cache-parameter-group-family")] string CacheParameterGroupFamily,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}