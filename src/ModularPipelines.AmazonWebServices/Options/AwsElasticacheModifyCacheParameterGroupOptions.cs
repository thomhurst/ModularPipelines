using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "modify-cache-parameter-group")]
public record AwsElasticacheModifyCacheParameterGroupOptions(
[property: CommandSwitch("--cache-parameter-group-name")] string CacheParameterGroupName,
[property: CommandSwitch("--parameter-name-values")] string[] ParameterNameValues
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}