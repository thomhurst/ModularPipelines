using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tnb", "update-sol-function-package")]
public record AwsTnbUpdateSolFunctionPackageOptions(
[property: CliOption("--operational-state")] string OperationalState,
[property: CliOption("--vnf-pkg-id")] string VnfPkgId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}