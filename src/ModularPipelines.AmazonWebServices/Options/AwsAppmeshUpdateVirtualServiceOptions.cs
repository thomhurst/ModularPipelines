using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appmesh", "update-virtual-service")]
public record AwsAppmeshUpdateVirtualServiceOptions(
[property: CliOption("--mesh-name")] string MeshName,
[property: CliOption("--spec")] string Spec,
[property: CliOption("--virtual-service-name")] string VirtualServiceName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--mesh-owner")]
    public string? MeshOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}