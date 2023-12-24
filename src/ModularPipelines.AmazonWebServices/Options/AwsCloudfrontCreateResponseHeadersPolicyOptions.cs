using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "create-response-headers-policy")]
public record AwsCloudfrontCreateResponseHeadersPolicyOptions(
[property: CommandSwitch("--response-headers-policy-config")] string ResponseHeadersPolicyConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}