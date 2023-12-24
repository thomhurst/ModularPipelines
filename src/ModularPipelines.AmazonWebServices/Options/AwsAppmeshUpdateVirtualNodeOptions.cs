using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appmesh", "update-virtual-node")]
public record AwsAppmeshUpdateVirtualNodeOptions(
[property: CommandSwitch("--mesh-name")] string MeshName,
[property: CommandSwitch("--spec")] string Spec,
[property: CommandSwitch("--virtual-node-name")] string VirtualNodeName
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--mesh-owner")]
    public string? MeshOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}