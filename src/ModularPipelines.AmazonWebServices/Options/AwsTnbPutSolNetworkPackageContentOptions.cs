using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tnb", "put-sol-network-package-content")]
public record AwsTnbPutSolNetworkPackageContentOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--nsd-info-id")] string NsdInfoId
) : AwsOptions
{
    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}