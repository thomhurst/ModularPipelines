using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datamigration", "get-sku-recommendation")]
public record AzDatamigrationGetSkuRecommendationOptions : AzOptions
{
    [CliOption("--config-file-path")]
    public string? ConfigFilePath { get; set; }

    [CliOption("--database-allow-list")]
    public string? DatabaseAllowList { get; set; }

    [CliOption("--database-deny-list")]
    public string? DatabaseDenyList { get; set; }

    [CliFlag("--display-result")]
    public bool? DisplayResult { get; set; }

    [CliFlag("--elastic-strategy")]
    public bool? ElasticStrategy { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--output-folder")]
    public string? OutputFolder { get; set; }

    [CliFlag("--overwrite")]
    public bool? Overwrite { get; set; }

    [CliOption("--scaling-factor")]
    public string? ScalingFactor { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--target-percentile")]
    public string? TargetPercentile { get; set; }

    [CliOption("--target-platform")]
    public string? TargetPlatform { get; set; }

    [CliOption("--target-sql-instance")]
    public string? TargetSqlInstance { get; set; }
}