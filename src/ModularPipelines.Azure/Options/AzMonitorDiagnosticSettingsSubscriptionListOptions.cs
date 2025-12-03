using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "diagnostic-settings", "subscription", "list")]
public record AzMonitorDiagnosticSettingsSubscriptionListOptions : AzOptions;