using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datamigration", "performance-data-collection")]
public record AzDatamigrationPerformanceDataCollectionOptions : AzOptions
{
    [CliOption("--config-file-path")]
    public string? ConfigFilePath { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--number-of-iteration")]
    public string? NumberOfIteration { get; set; }

    [CliOption("--output-folder")]
    public string? OutputFolder { get; set; }

    [CliOption("--perf-query-interval")]
    public string? PerfQueryInterval { get; set; }

    [CliOption("--static-query-interval")]
    public string? StaticQueryInterval { get; set; }

    [CliOption("--time")]
    public string? Time { get; set; }
}