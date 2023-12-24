using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "get-mobile-sdk-release")]
public record AwsWafv2GetMobileSdkReleaseOptions(
[property: CommandSwitch("--platform")] string Platform,
[property: CommandSwitch("--release-version")] string ReleaseVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}