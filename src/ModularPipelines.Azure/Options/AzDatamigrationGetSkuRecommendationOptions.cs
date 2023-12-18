using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datamigration", "get-sku-recommendation")]
public record AzDatamigrationGetSkuRecommendationOptions : AzOptions
{
    [CommandSwitch("--config-file-path")]
    public string? ConfigFilePath { get; set; }

    [CommandSwitch("--database-allow-list")]
    public string? DatabaseAllowList { get; set; }

    [CommandSwitch("--database-deny-list")]
    public string? DatabaseDenyList { get; set; }

    [BooleanCommandSwitch("--display-result")]
    public bool? DisplayResult { get; set; }

    [BooleanCommandSwitch("--elastic-strategy")]
    public bool? ElasticStrategy { get; set; }

    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--output-folder")]
    public string? OutputFolder { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public bool? Overwrite { get; set; }

    [CommandSwitch("--scaling-factor")]
    public string? ScalingFactor { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--target-percentile")]
    public string? TargetPercentile { get; set; }

    [CommandSwitch("--target-platform")]
    public string? TargetPlatform { get; set; }

    [CommandSwitch("--target-sql-instance")]
    public string? TargetSqlInstance { get; set; }
}