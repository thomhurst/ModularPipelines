using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "publish-app-version")]
public record AwsResiliencehubPublishAppVersionOptions(
[property: CliOption("--app-arn")] string AppArn
) : AwsOptions
{
    [CliOption("--version-name")]
    public string? VersionName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}