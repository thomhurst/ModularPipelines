using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "external-vpn-gateways", "create")]
public record GcloudComputeExternalVpnGatewaysCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--interfaces")] string[] Interfaces
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}