using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tnb", "update-sol-network-package")]
public record AwsTnbUpdateSolNetworkPackageOptions(
[property: CliOption("--nsd-info-id")] string NsdInfoId,
[property: CliOption("--nsd-operational-state")] string NsdOperationalState
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}