using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "create-origin-request-policy")]
public record AwsCloudfrontCreateOriginRequestPolicyOptions(
[property: CommandSwitch("--origin-request-policy-config")] string OriginRequestPolicyConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}