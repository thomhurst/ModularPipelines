using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appmesh", "create-virtual-router")]
public record AwsAppmeshCreateVirtualRouterOptions(
[property: CliOption("--mesh-name")] string MeshName,
[property: CliOption("--spec")] string Spec,
[property: CliOption("--virtual-router-name")] string VirtualRouterName
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