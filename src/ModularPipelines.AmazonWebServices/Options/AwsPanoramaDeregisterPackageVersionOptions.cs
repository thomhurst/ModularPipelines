using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("panorama", "deregister-package-version")]
public record AwsPanoramaDeregisterPackageVersionOptions(
[property: CommandSwitch("--package-id")] string PackageId,
[property: CommandSwitch("--package-version")] string PackageVersion,
[property: CommandSwitch("--patch-version")] string PatchVersion
) : AwsOptions
{
    [CommandSwitch("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CommandSwitch("--updated-latest-patch-version")]
    public string? UpdatedLatestPatchVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}