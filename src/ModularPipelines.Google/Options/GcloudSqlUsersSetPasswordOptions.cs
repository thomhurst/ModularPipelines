using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "users", "set-password")]
public record GcloudSqlUsersSetPasswordOptions(
[property: PositionalArgument] string Username,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [BooleanCommandSwitch("--discard-dual-password")]
    public bool? DiscardDualPassword { get; set; }

    [BooleanCommandSwitch("--retain-password")]
    public bool? RetainPassword { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [BooleanCommandSwitch("--prompt-for-password")]
    public bool? PromptForPassword { get; set; }
}