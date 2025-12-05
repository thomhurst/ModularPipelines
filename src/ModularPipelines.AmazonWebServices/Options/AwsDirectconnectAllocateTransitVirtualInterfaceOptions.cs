using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "allocate-transit-virtual-interface")]
public record AwsDirectconnectAllocateTransitVirtualInterfaceOptions(
[property: CliOption("--connection-id")] string ConnectionId,
[property: CliOption("--owner-account")] string OwnerAccount,
[property: CliOption("--new-transit-virtual-interface-allocation")] string NewTransitVirtualInterfaceAllocation
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}