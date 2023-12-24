using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "generate-mobile-sdk-release-url")]
public record AwsWafv2GenerateMobileSdkReleaseUrlOptions(
[property: CommandSwitch("--platform")] string Platform,
[property: CommandSwitch("--release-version")] string ReleaseVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}