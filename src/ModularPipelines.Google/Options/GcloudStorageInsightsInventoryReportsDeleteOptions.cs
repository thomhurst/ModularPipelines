using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "insights", "inventory-reports", "delete")]
public record GcloudStorageInsightsInventoryReportsDeleteOptions(
[property: CliArgument] string ReportConfig,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}