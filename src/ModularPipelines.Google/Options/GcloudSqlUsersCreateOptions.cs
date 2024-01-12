using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "users", "create")]
public record GcloudSqlUsersCreateOptions(
[property: PositionalArgument] string Username,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--password-policy-allowed-failed-attempts")]
    public string? PasswordPolicyAllowedFailedAttempts { get; set; }

    [CommandSwitch("--[no-]password-policy-enable-failed-attempts-check")]
    public string[]? NoPasswordPolicyEnableFailedAttemptsCheck { get; set; }

    [CommandSwitch("--[no-]password-policy-enable-password-verification")]
    public string[]? NoPasswordPolicyEnablePasswordVerification { get; set; }

    [CommandSwitch("--password-policy-password-expiration-duration")]
    public string? PasswordPolicyPasswordExpirationDuration { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}