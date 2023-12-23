using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "allocate-public-virtual-interface")]
public record AwsDirectconnectAllocatePublicVirtualInterfaceOptions(
[property: CommandSwitch("--connection-id")] string ConnectionId,
[property: CommandSwitch("--owner-account")] string OwnerAccount,
[property: CommandSwitch("--new-public-virtual-interface-allocation")] string NewPublicVirtualInterfaceAllocation
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}