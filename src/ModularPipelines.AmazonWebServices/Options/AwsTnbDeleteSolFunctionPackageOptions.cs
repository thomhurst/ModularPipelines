using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tnb", "delete-sol-function-package")]
public record AwsTnbDeleteSolFunctionPackageOptions(
[property: CliOption("--vnf-pkg-id")] string VnfPkgId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}