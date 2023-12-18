using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "connected-registry", "get-settings")]
public record AzAcrConnectedRegistryGetSettingsOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--parent-protocol")] string ParentProtocol,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--generate-password")]
    public string? GeneratePassword { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}