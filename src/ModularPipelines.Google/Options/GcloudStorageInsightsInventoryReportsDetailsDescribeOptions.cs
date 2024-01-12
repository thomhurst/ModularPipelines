using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "insights", "inventory-reports", "details", "describe")]
public record GcloudStorageInsightsInventoryReportsDetailsDescribeOptions(
[property: PositionalArgument] string ReportDetail,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string ReportConfig
) : GcloudOptions;