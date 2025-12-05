using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "describe-virtual-interfaces")]
public record AwsDirectconnectDescribeVirtualInterfacesOptions : AwsOptions
{
    [CliOption("--connection-id")]
    public string? ConnectionId { get; set; }

    [CliOption("--virtual-interface-id")]
    public string? VirtualInterfaceId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}