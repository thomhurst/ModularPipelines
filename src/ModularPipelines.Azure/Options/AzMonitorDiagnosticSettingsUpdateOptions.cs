using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "diagnostic-settings", "update")]
public record AzMonitorDiagnosticSettingsUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource")] string Resource
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--event-hub")]
    public string? EventHub { get; set; }

    [CommandSwitch("--event-hub-rule")]
    public string? EventHubRule { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--log-ana-dtype")]
    public string? LogAnaDtype { get; set; }

    [CommandSwitch("--logs")]
    public string? Logs { get; set; }

    [CommandSwitch("--marketplace-partner-id")]
    public string? MarketplacePartnerId { get; set; }

    [CommandSwitch("--metrics")]
    public string? Metrics { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-namespace")]
    public string? ResourceNamespace { get; set; }

    [CommandSwitch("--resource-parent")]
    public string? ResourceParent { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--service-bus-rule-id")]
    public string? ServiceBusRuleId { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--storage-account-id")]
    public int? StorageAccountId { get; set; }

    [CommandSwitch("--workspace-id")]
    public string? WorkspaceId { get; set; }
}