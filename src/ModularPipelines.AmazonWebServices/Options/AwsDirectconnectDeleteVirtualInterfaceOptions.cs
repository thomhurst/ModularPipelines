using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "delete-virtual-interface")]
public record AwsDirectconnectDeleteVirtualInterfaceOptions(
[property: CommandSwitch("--virtual-interface-id")] string VirtualInterfaceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}