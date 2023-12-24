using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "create-cache-security-group")]
public record AwsElasticacheCreateCacheSecurityGroupOptions(
[property: CommandSwitch("--cache-security-group-name")] string CacheSecurityGroupName,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}