using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "export", "sql")]
public record GcloudSqlExportSqlOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Uri
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--database")]
    public string[]? Database { get; set; }

    [BooleanCommandSwitch("--offload")]
    public bool? Offload { get; set; }

    [CommandSwitch("--table")]
    public string[]? Table { get; set; }
}