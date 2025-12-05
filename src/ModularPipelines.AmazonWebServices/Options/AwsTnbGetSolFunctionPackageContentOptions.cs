using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tnb", "get-sol-function-package-content")]
public record AwsTnbGetSolFunctionPackageContentOptions(
[property: CliOption("--accept")] string Accept,
[property: CliOption("--vnf-pkg-id")] string VnfPkgId
) : AwsOptions;