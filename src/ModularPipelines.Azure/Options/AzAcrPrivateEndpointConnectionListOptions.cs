using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "private-endpoint-connection", "list")]
public record AzAcrPrivateEndpointConnectionListOptions(
[property: CommandSwitch("--registry-name")] string RegistryName
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}