using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appmesh", "describe-virtual-router")]
public record AwsAppmeshDescribeVirtualRouterOptions(
[property: CliOption("--mesh-name")] string MeshName,
[property: CliOption("--virtual-router-name")] string VirtualRouterName
) : AwsOptions
{
    [CliOption("--mesh-owner")]
    public string? MeshOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}