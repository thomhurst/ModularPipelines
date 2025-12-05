using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "describe-app-version-resources-resolution-status")]
public record AwsResiliencehubDescribeAppVersionResourcesResolutionStatusOptions(
[property: CliOption("--app-arn")] string AppArn,
[property: CliOption("--app-version")] string AppVersion
) : AwsOptions
{
    [CliOption("--resolution-id")]
    public string? ResolutionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}