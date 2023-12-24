using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appmesh", "update-gateway-route")]
public record AwsAppmeshUpdateGatewayRouteOptions(
[property: CommandSwitch("--gateway-route-name")] string GatewayRouteName,
[property: CommandSwitch("--mesh-name")] string MeshName,
[property: CommandSwitch("--spec")] string Spec,
[property: CommandSwitch("--virtual-gateway-name")] string VirtualGatewayName
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--mesh-owner")]
    public string? MeshOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}