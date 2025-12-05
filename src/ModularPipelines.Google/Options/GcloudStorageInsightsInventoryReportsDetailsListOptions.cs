using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "insights", "inventory-reports", "details", "list")]
public record GcloudStorageInsightsInventoryReportsDetailsListOptions(
[property: CliArgument] string ReportConfig,
[property: CliArgument] string Location
) : GcloudOptions;