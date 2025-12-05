using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "resolve-app-version-resources")]
public record AwsResiliencehubResolveAppVersionResourcesOptions(
[property: CliOption("--app-arn")] string AppArn,
[property: CliOption("--app-version")] string AppVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}