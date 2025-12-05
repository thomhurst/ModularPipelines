using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appmesh", "create-gateway-route")]
public record AwsAppmeshCreateGatewayRouteOptions(
[property: CliOption("--gateway-route-name")] string GatewayRouteName,
[property: CliOption("--mesh-name")] string MeshName,
[property: CliOption("--spec")] string Spec,
[property: CliOption("--virtual-gateway-name")] string VirtualGatewayName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--mesh-owner")]
    public string? MeshOwner { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}