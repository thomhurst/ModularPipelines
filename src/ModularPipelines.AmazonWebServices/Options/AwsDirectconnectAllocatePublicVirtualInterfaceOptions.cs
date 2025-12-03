using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "allocate-public-virtual-interface")]
public record AwsDirectconnectAllocatePublicVirtualInterfaceOptions(
[property: CliOption("--connection-id")] string ConnectionId,
[property: CliOption("--owner-account")] string OwnerAccount,
[property: CliOption("--new-public-virtual-interface-allocation")] string NewPublicVirtualInterfaceAllocation
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}