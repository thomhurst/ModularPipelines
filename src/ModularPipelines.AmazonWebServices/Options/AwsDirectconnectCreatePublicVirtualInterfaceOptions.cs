using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "create-public-virtual-interface")]
public record AwsDirectconnectCreatePublicVirtualInterfaceOptions(
[property: CommandSwitch("--connection-id")] string ConnectionId,
[property: CommandSwitch("--new-public-virtual-interface")] string NewPublicVirtualInterface
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}