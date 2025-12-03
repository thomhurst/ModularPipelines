using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "diagnostic-settings", "update")]
public record AzMonitorDiagnosticSettingsUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource")] string Resource
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--event-hub")]
    public string? EventHub { get; set; }

    [CliOption("--event-hub-rule")]
    public string? EventHubRule { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--log-ana-dtype")]
    public string? LogAnaDtype { get; set; }

    [CliOption("--logs")]
    public string? Logs { get; set; }

    [CliOption("--marketplace-partner-id")]
    public string? MarketplacePartnerId { get; set; }

    [CliOption("--metrics")]
    public string? Metrics { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-namespace")]
    public string? ResourceNamespace { get; set; }

    [CliOption("--resource-parent")]
    public string? ResourceParent { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--service-bus-rule-id")]
    public string? ServiceBusRuleId { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--storage-account-id")]
    public int? StorageAccountId { get; set; }

    [CliOption("--workspace-id")]
    public string? WorkspaceId { get; set; }
}