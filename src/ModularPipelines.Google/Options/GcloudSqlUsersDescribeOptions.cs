using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "users", "describe")]
public record GcloudSqlUsersDescribeOptions(
[property: CliArgument] string Username,
[property: CliOption("--instance")] string Instance
) : GcloudOptions
{
    [CliOption("--host")]
    public string? Host { get; set; }
}