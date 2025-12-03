using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "users", "create")]
public record GcloudSqlUsersCreateOptions(
[property: CliArgument] string Username,
[property: CliOption("--instance")] string Instance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--password-policy-allowed-failed-attempts")]
    public string? PasswordPolicyAllowedFailedAttempts { get; set; }

    [CliOption("--[no-]password-policy-enable-failed-attempts-check")]
    public string[]? NoPasswordPolicyEnableFailedAttemptsCheck { get; set; }

    [CliOption("--[no-]password-policy-enable-password-verification")]
    public string[]? NoPasswordPolicyEnablePasswordVerification { get; set; }

    [CliOption("--password-policy-password-expiration-duration")]
    public string? PasswordPolicyPasswordExpirationDuration { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}