using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tnb", "delete-sol-network-package")]
public record AwsTnbDeleteSolNetworkPackageOptions(
[property: CliOption("--nsd-info-id")] string NsdInfoId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}