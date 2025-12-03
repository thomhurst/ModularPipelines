using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("panorama", "deregister-package-version")]
public record AwsPanoramaDeregisterPackageVersionOptions(
[property: CliOption("--package-id")] string PackageId,
[property: CliOption("--package-version")] string PackageVersion,
[property: CliOption("--patch-version")] string PatchVersion
) : AwsOptions
{
    [CliOption("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CliOption("--updated-latest-patch-version")]
    public string? UpdatedLatestPatchVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}