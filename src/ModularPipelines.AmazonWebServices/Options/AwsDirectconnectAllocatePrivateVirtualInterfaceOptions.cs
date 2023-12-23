using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "allocate-private-virtual-interface")]
public record AwsDirectconnectAllocatePrivateVirtualInterfaceOptions(
[property: CommandSwitch("--connection-id")] string ConnectionId,
[property: CommandSwitch("--owner-account")] string OwnerAccount,
[property: CommandSwitch("--new-private-virtual-interface-allocation")] string NewPrivateVirtualInterfaceAllocation
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}