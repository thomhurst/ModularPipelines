using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "create-public-virtual-interface")]
public record AwsDirectconnectCreatePublicVirtualInterfaceOptions(
[property: CliOption("--connection-id")] string ConnectionId,
[property: CliOption("--new-public-virtual-interface")] string NewPublicVirtualInterface
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}