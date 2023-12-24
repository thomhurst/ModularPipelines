using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "confirm-public-virtual-interface")]
public record AwsDirectconnectConfirmPublicVirtualInterfaceOptions(
[property: CommandSwitch("--virtual-interface-id")] string VirtualInterfaceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}