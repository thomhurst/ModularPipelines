using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "update-virtual-interface-attributes")]
public record AwsDirectconnectUpdateVirtualInterfaceAttributesOptions(
[property: CommandSwitch("--virtual-interface-id")] string VirtualInterfaceId
) : AwsOptions
{
    [CommandSwitch("--mtu")]
    public int? Mtu { get; set; }

    [CommandSwitch("--virtual-interface-name")]
    public string? VirtualInterfaceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}