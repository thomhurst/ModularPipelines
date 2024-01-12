using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "users", "describe")]
public record GcloudSqlUsersDescribeOptions(
[property: PositionalArgument] string Username,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions
{
    [CommandSwitch("--host")]
    public string? Host { get; set; }
}