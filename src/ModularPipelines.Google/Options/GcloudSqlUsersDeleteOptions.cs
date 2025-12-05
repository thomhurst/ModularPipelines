using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "users", "delete")]
public record GcloudSqlUsersDeleteOptions(
[property: CliArgument] string Username,
[property: CliOption("--instance")] string Instance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }
}