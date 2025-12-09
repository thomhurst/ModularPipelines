using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appmesh", "delete-virtual-node")]
public record AwsAppmeshDeleteVirtualNodeOptions(
[property: CliOption("--mesh-name")] string MeshName,
[property: CliOption("--virtual-node-name")] string VirtualNodeName
) : AwsOptions
{
    [CliOption("--mesh-owner")]
    public string? MeshOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}