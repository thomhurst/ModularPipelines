using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "insights", "inventory-reports", "details", "list")]
public record GcloudStorageInsightsInventoryReportsDetailsListOptions(
[property: PositionalArgument] string ReportConfig,
[property: PositionalArgument] string Location
) : GcloudOptions;