using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "diagnostic-settings", "create")]
public record AzMonitorDiagnosticSettingsCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource")] string Resource
) : AzOptions
{
    [CliOption("--event-hub")]
    public string? EventHub { get; set; }

    [CliOption("--event-hub-rule")]
    public string? EventHubRule { get; set; }

    [CliFlag("--export-to-resource-specific")]
    public bool? ExportToResourceSpecific { get; set; }

    [CliOption("--logs")]
    public string? Logs { get; set; }

    [CliOption("--marketplace-partner-id")]
    public string? MarketplacePartnerId { get; set; }

    [CliOption("--metrics")]
    public string? Metrics { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-namespace")]
    public string? ResourceNamespace { get; set; }

    [CliOption("--resource-parent")]
    public string? ResourceParent { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}