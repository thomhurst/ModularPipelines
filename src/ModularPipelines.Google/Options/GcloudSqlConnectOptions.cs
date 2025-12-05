using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "connect")]
public record GcloudSqlConnectOptions(
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }

    [CliOption("--user")]
    public string? User { get; set; }
}