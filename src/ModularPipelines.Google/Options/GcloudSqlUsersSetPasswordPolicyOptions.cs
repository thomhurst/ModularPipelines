using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "users", "set-password-policy")]
public record GcloudSqlUsersSetPasswordPolicyOptions(
[property: PositionalArgument] string Username,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--clear-password-policy")]
    public bool? ClearPasswordPolicy { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--password-policy-allowed-failed-attempts")]
    public string? PasswordPolicyAllowedFailedAttempts { get; set; }

    [CommandSwitch("--[no-]password-policy-enable-failed-attempts-check")]
    public string[]? NoPasswordPolicyEnableFailedAttemptsCheck { get; set; }

    [CommandSwitch("--[no-]password-policy-enable-password-verification")]
    public string[]? NoPasswordPolicyEnablePasswordVerification { get; set; }

    [CommandSwitch("--password-policy-password-expiration-duration")]
    public string? PasswordPolicyPasswordExpirationDuration { get; set; }
}