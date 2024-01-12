using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "insights", "inventory-reports", "delete")]
public record GcloudStorageInsightsInventoryReportsDeleteOptions(
[property: PositionalArgument] string ReportConfig,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}