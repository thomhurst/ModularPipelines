using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "import", "sql")]
public record GcloudSqlImportSqlOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Uri
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--database")]
    public string? Database { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }
}