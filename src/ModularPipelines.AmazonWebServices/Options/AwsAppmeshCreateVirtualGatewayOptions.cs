using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appmesh", "create-virtual-gateway")]
public record AwsAppmeshCreateVirtualGatewayOptions(
[property: CommandSwitch("--mesh-name")] string MeshName,
[property: CommandSwitch("--spec")] string Spec,
[property: CommandSwitch("--virtual-gateway-name")] string VirtualGatewayName
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--mesh-owner")]
    public string? MeshOwner { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}