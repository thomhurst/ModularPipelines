using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "diagnostic-settings", "subscription", "update")]
public record AzMonitorDiagnosticSettingsSubscriptionUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--event-hub-auth-rule")]
    public string? EventHubAuthRule { get; set; }

    [CliOption("--event-hub-name")]
    public string? EventHubName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--logs")]
    public string? Logs { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--service-bus-rule")]
    public string? ServiceBusRule { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}