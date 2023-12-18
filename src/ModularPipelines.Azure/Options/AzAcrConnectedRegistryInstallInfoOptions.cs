using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "connected-registry", "install", "info")]
public record AzAcrConnectedRegistryInstallInfoOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--parent-protocol")] string ParentProtocol,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

