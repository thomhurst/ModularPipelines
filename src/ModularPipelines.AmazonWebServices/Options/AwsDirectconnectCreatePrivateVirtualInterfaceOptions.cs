using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "create-private-virtual-interface")]
public record AwsDirectconnectCreatePrivateVirtualInterfaceOptions(
[property: CliOption("--connection-id")] string ConnectionId,
[property: CliOption("--new-private-virtual-interface")] string NewPrivateVirtualInterface
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}