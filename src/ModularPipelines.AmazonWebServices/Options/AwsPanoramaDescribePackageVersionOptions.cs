using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("panorama", "describe-package-version")]
public record AwsPanoramaDescribePackageVersionOptions(
[property: CliOption("--package-id")] string PackageId,
[property: CliOption("--package-version")] string PackageVersion
) : AwsOptions
{
    [CliOption("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CliOption("--patch-version")]
    public string? PatchVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}