using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appmesh", "delete-virtual-router")]
public record AwsAppmeshDeleteVirtualRouterOptions(
[property: CommandSwitch("--mesh-name")] string MeshName,
[property: CommandSwitch("--virtual-router-name")] string VirtualRouterName
) : AwsOptions
{
    [CommandSwitch("--mesh-owner")]
    public string? MeshOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}