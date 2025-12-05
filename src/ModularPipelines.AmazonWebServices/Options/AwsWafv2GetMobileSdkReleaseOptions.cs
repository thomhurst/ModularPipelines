using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "get-mobile-sdk-release")]
public record AwsWafv2GetMobileSdkReleaseOptions(
[property: CliOption("--platform")] string Platform,
[property: CliOption("--release-version")] string ReleaseVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}