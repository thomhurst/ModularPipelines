using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("panorama", "register-package-version")]
public record AwsPanoramaRegisterPackageVersionOptions(
[property: CliOption("--package-id")] string PackageId,
[property: CliOption("--package-version")] string PackageVersion,
[property: CliOption("--patch-version")] string PatchVersion
) : AwsOptions
{
    [CliOption("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}