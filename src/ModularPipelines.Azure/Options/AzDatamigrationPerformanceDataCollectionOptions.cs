using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datamigration", "performance-data-collection")]
public record AzDatamigrationPerformanceDataCollectionOptions : AzOptions
{
    [CommandSwitch("--config-file-path")]
    public string? ConfigFilePath { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--number-of-iteration")]
    public string? NumberOfIteration { get; set; }

    [CommandSwitch("--output-folder")]
    public string? OutputFolder { get; set; }

    [CommandSwitch("--perf-query-interval")]
    public string? PerfQueryInterval { get; set; }

    [CommandSwitch("--static-query-interval")]
    public string? StaticQueryInterval { get; set; }

    [CommandSwitch("--time")]
    public string? Time { get; set; }
}