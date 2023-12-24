using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "describe-virtual-interfaces")]
public record AwsDirectconnectDescribeVirtualInterfacesOptions : AwsOptions
{
    [CommandSwitch("--connection-id")]
    public string? ConnectionId { get; set; }

    [CommandSwitch("--virtual-interface-id")]
    public string? VirtualInterfaceId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}