using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "connected-registry", "create")]
public record AzAcrConnectedRegistryCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--client-tokens")]
    public string? ClientTokens { get; set; }

    [CliOption("--log-level")]
    public string? LogLevel { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--notifications")]
    public string? Notifications { get; set; }

    [CliOption("--parent")]
    public string? Parent { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sync-message-ttl")]
    public string? SyncMessageTtl { get; set; }

    [CliOption("--sync-schedule")]
    public string? SyncSchedule { get; set; }

    [CliOption("--sync-token")]
    public string? SyncToken { get; set; }

    [CliOption("--sync-window")]
    public string? SyncWindow { get; set; }
}