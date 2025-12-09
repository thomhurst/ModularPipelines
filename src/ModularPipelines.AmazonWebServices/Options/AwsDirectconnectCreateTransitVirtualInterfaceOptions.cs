using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "create-transit-virtual-interface")]
public record AwsDirectconnectCreateTransitVirtualInterfaceOptions(
[property: CliOption("--connection-id")] string ConnectionId,
[property: CliOption("--new-transit-virtual-interface")] string NewTransitVirtualInterface
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}