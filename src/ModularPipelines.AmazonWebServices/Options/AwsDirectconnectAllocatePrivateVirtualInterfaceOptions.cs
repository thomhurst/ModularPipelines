using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "allocate-private-virtual-interface")]
public record AwsDirectconnectAllocatePrivateVirtualInterfaceOptions(
[property: CliOption("--connection-id")] string ConnectionId,
[property: CliOption("--owner-account")] string OwnerAccount,
[property: CliOption("--new-private-virtual-interface-allocation")] string NewPrivateVirtualInterfaceAllocation
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}