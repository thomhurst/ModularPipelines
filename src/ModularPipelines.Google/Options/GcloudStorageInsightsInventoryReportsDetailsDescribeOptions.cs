using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "insights", "inventory-reports", "details", "describe")]
public record GcloudStorageInsightsInventoryReportsDetailsDescribeOptions(
[property: CliArgument] string ReportDetail,
[property: CliArgument] string Location,
[property: CliArgument] string ReportConfig
) : GcloudOptions;