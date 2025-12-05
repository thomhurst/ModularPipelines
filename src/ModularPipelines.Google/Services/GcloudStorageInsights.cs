using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage")]
public class GcloudStorageInsights
{
    public GcloudStorageInsights(
        GcloudStorageInsightsInventoryReports inventoryReports
    )
    {
        InventoryReports = inventoryReports;
    }

    public GcloudStorageInsightsInventoryReports InventoryReports { get; }
}