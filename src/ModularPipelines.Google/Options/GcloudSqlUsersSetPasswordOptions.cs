using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "users", "set-password")]
public record GcloudSqlUsersSetPasswordOptions(
[property: CliArgument] string Username,
[property: CliOption("--instance")] string Instance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliFlag("--discard-dual-password")]
    public bool? DiscardDualPassword { get; set; }

    [CliFlag("--retain-password")]
    public bool? RetainPassword { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliFlag("--prompt-for-password")]
    public bool? PromptForPassword { get; set; }
}