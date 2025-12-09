using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "associate-virtual-interface")]
public record AwsDirectconnectAssociateVirtualInterfaceOptions(
[property: CliOption("--virtual-interface-id")] string VirtualInterfaceId,
[property: CliOption("--connection-id")] string ConnectionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}