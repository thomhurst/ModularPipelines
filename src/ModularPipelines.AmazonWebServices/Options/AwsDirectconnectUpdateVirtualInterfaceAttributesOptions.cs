using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "update-virtual-interface-attributes")]
public record AwsDirectconnectUpdateVirtualInterfaceAttributesOptions(
[property: CliOption("--virtual-interface-id")] string VirtualInterfaceId
) : AwsOptions
{
    [CliOption("--mtu")]
    public int? Mtu { get; set; }

    [CliOption("--virtual-interface-name")]
    public string? VirtualInterfaceName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}