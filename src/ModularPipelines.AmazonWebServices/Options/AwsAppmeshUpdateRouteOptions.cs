using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appmesh", "update-route")]
public record AwsAppmeshUpdateRouteOptions(
[property: CommandSwitch("--mesh-name")] string MeshName,
[property: CommandSwitch("--route-name")] string RouteName,
[property: CommandSwitch("--spec")] string Spec,
[property: CommandSwitch("--virtual-router-name")] string VirtualRouterName
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--mesh-owner")]
    public string? MeshOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}