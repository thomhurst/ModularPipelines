using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appmesh", "delete-virtual-service")]
public record AwsAppmeshDeleteVirtualServiceOptions(
[property: CliOption("--mesh-name")] string MeshName,
[property: CliOption("--virtual-service-name")] string VirtualServiceName
) : AwsOptions
{
    [CliOption("--mesh-owner")]
    public string? MeshOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}