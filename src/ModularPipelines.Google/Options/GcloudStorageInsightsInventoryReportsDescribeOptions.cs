using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "insights", "inventory-reports", "describe")]
public record GcloudStorageInsightsInventoryReportsDescribeOptions(
[property: PositionalArgument] string ReportConfig,
[property: PositionalArgument] string Location
) : GcloudOptions;