using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "private-endpoint-connection", "delete")]
public record AzAcrPrivateEndpointConnectionDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry-name")] string RegistryName
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}