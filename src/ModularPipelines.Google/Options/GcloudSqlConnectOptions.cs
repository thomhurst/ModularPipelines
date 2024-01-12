using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "connect")]
public record GcloudSqlConnectOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions
{
    [CommandSwitch("--database")]
    public string? Database { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }
}