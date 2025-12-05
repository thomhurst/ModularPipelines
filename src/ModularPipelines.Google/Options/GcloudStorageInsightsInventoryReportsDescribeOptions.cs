using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "insights", "inventory-reports", "describe")]
public record GcloudStorageInsightsInventoryReportsDescribeOptions(
[property: CliArgument] string ReportConfig,
[property: CliArgument] string Location
) : GcloudOptions;