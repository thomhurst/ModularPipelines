using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "private-endpoint-connection", "list")]
public record AzKeyvaultPrivateEndpointConnectionListOptions(
[property: CommandSwitch("--hsm-name")] string HsmName
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}