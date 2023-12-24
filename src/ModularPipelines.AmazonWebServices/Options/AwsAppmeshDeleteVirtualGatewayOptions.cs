using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appmesh", "delete-virtual-gateway")]
public record AwsAppmeshDeleteVirtualGatewayOptions(
[property: CommandSwitch("--mesh-name")] string MeshName,
[property: CommandSwitch("--virtual-gateway-name")] string VirtualGatewayName
) : AwsOptions
{
    [CommandSwitch("--mesh-owner")]
    public string? MeshOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}