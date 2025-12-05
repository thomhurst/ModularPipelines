using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appmesh", "update-route")]
public record AwsAppmeshUpdateRouteOptions(
[property: CliOption("--mesh-name")] string MeshName,
[property: CliOption("--route-name")] string RouteName,
[property: CliOption("--spec")] string Spec,
[property: CliOption("--virtual-router-name")] string VirtualRouterName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--mesh-owner")]
    public string? MeshOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}