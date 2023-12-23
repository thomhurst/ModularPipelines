using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "update-cache-policy")]
public record AwsCloudfrontUpdateCachePolicyOptions(
[property: CommandSwitch("--cache-policy-config")] string CachePolicyConfig,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}