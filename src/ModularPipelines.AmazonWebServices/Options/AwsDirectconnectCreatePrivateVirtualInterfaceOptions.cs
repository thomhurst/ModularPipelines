using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "create-private-virtual-interface")]
public record AwsDirectconnectCreatePrivateVirtualInterfaceOptions(
[property: CommandSwitch("--connection-id")] string ConnectionId,
[property: CommandSwitch("--new-private-virtual-interface")] string NewPrivateVirtualInterface
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}