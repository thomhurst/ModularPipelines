using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "generate-mobile-sdk-release-url")]
public record AwsWafv2GenerateMobileSdkReleaseUrlOptions(
[property: CliOption("--platform")] string Platform,
[property: CliOption("--release-version")] string ReleaseVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}