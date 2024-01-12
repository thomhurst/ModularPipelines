using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "instances", "clone")]
public record GcloudSqlInstancesCloneOptions(
[property: PositionalArgument] string Source,
[property: PositionalArgument] string Destination
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--bin-log-file-name")]
    public string? BinLogFileName { get; set; }

    [CommandSwitch("--bin-log-position")]
    public string? BinLogPosition { get; set; }

    [CommandSwitch("--point-in-time")]
    public string? PointInTime { get; set; }

    [CommandSwitch("--preferred-zone")]
    public string? PreferredZone { get; set; }

    [CommandSwitch("--restore-database-name")]
    public string? RestoreDatabaseName { get; set; }
}