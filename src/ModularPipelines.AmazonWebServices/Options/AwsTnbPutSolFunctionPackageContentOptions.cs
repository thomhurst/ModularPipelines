using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tnb", "put-sol-function-package-content")]
public record AwsTnbPutSolFunctionPackageContentOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--vnf-pkg-id")] string VnfPkgId
) : AwsOptions
{
    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}