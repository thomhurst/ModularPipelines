using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "reset-cache-parameter-group")]
public record AwsElasticacheResetCacheParameterGroupOptions(
[property: CommandSwitch("--cache-parameter-group-name")] string CacheParameterGroupName
) : AwsOptions
{
    [CommandSwitch("--parameter-name-values")]
    public string[]? ParameterNameValues { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}