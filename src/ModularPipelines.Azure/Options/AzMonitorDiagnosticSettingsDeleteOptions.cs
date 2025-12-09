using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "diagnostic-settings", "delete")]
public record AzMonitorDiagnosticSettingsDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource")] string Resource
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-namespace")]
    public string? ResourceNamespace { get; set; }

    [CliOption("--resource-parent")]
    public string? ResourceParent { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }
}