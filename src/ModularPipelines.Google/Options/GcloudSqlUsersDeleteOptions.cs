using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "users", "delete")]
public record GcloudSqlUsersDeleteOptions(
[property: PositionalArgument] string Username,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }
}