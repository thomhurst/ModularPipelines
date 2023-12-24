using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "allocate-transit-virtual-interface")]
public record AwsDirectconnectAllocateTransitVirtualInterfaceOptions(
[property: CommandSwitch("--connection-id")] string ConnectionId,
[property: CommandSwitch("--owner-account")] string OwnerAccount,
[property: CommandSwitch("--new-transit-virtual-interface-allocation")] string NewTransitVirtualInterfaceAllocation
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}