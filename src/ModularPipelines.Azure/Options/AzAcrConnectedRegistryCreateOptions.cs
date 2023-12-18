using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "connected-registry", "create")]
public record AzAcrConnectedRegistryCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--client-tokens")]
    public string? ClientTokens { get; set; }

    [CommandSwitch("--log-level")]
    public string? LogLevel { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--notifications")]
    public string? Notifications { get; set; }

    [CommandSwitch("--parent")]
    public string? Parent { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sync-message-ttl")]
    public string? SyncMessageTtl { get; set; }

    [CommandSwitch("--sync-schedule")]
    public string? SyncSchedule { get; set; }

    [CommandSwitch("--sync-token")]
    public string? SyncToken { get; set; }

    [CommandSwitch("--sync-window")]
    public string? SyncWindow { get; set; }
}

