using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("panorama", "describe-package-version")]
public record AwsPanoramaDescribePackageVersionOptions(
[property: CommandSwitch("--package-id")] string PackageId,
[property: CommandSwitch("--package-version")] string PackageVersion
) : AwsOptions
{
    [CommandSwitch("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CommandSwitch("--patch-version")]
    public string? PatchVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}