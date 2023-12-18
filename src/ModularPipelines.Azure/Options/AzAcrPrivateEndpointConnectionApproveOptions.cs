using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "private-endpoint-connection", "approve")]
public record AzAcrPrivateEndpointConnectionApproveOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry-name")] string RegistryName
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

