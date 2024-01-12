using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "databases", "execute-sql")]
public record GcloudSpannerDatabasesExecuteSqlOptions(
[property: PositionalArgument] string Database,
[property: PositionalArgument] string Instance,
[property: CommandSwitch("--sql")] string Sql
) : GcloudOptions
{
    [CommandSwitch("--database-role")]
    public string? DatabaseRole { get; set; }

    [BooleanCommandSwitch("--enable-partitioned-dml")]
    public bool? EnablePartitionedDml { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--query-mode")]
    public string? QueryMode { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--read-timestamp")]
    public string? ReadTimestamp { get; set; }

    [BooleanCommandSwitch("--strong")]
    public bool? Strong { get; set; }
}