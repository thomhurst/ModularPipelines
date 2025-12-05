using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tnb", "cancel-sol-network-operation")]
public record AwsTnbCancelSolNetworkOperationOptions(
[property: CliOption("--ns-lcm-op-occ-id")] string NsLcmOpOccId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}