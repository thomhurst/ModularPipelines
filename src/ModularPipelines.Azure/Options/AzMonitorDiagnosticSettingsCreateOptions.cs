using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "diagnostic-settings", "create")]
public record AzMonitorDiagnosticSettingsCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource")] string Resource
) : AzOptions
{
    [CommandSwitch("--event-hub")]
    public string? EventHub { get; set; }

    [CommandSwitch("--event-hub-rule")]
    public string? EventHubRule { get; set; }

    [BooleanCommandSwitch("--export-to-resource-specific")]
    public bool? ExportToResourceSpecific { get; set; }

    [CommandSwitch("--logs")]
    public string? Logs { get; set; }

    [CommandSwitch("--marketplace-partner-id")]
    public string? MarketplacePartnerId { get; set; }

    [CommandSwitch("--metrics")]
    public string? Metrics { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-namespace")]
    public string? ResourceNamespace { get; set; }

    [CommandSwitch("--resource-parent")]
    public string? ResourceParent { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }
}