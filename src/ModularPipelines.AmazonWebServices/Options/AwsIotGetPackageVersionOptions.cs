using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "get-package-version")]
public record AwsIotGetPackageVersionOptions(
[property: CliOption("--package-name")] string PackageName,
[property: CliOption("--version-name")] string VersionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}