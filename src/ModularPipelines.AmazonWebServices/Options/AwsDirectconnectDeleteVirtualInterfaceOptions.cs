using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "delete-virtual-interface")]
public record AwsDirectconnectDeleteVirtualInterfaceOptions(
[property: CliOption("--virtual-interface-id")] string VirtualInterfaceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}