using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "connected-registry", "update")]
public record AzAcrConnectedRegistryUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--add-client-tokens")]
    public string? AddClientTokens { get; set; }

    [CommandSwitch("--add-notifications")]
    public string? AddNotifications { get; set; }

    [CommandSwitch("--log-level")]
    public string? LogLevel { get; set; }

    [CommandSwitch("--remove-client-tokens")]
    public string? RemoveClientTokens { get; set; }

    [CommandSwitch("--remove-notifications")]
    public string? RemoveNotifications { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sync-message-ttl")]
    public string? SyncMessageTtl { get; set; }

    [CommandSwitch("--sync-schedule")]
    public string? SyncSchedule { get; set; }

    [CommandSwitch("--sync-window")]
    public string? SyncWindow { get; set; }
}