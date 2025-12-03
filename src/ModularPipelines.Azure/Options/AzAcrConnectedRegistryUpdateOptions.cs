using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "connected-registry", "update")]
public record AzAcrConnectedRegistryUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--add-client-tokens")]
    public string? AddClientTokens { get; set; }

    [CliOption("--add-notifications")]
    public string? AddNotifications { get; set; }

    [CliOption("--log-level")]
    public string? LogLevel { get; set; }

    [CliOption("--remove-client-tokens")]
    public string? RemoveClientTokens { get; set; }

    [CliOption("--remove-notifications")]
    public string? RemoveNotifications { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sync-message-ttl")]
    public string? SyncMessageTtl { get; set; }

    [CliOption("--sync-schedule")]
    public string? SyncSchedule { get; set; }

    [CliOption("--sync-window")]
    public string? SyncWindow { get; set; }
}