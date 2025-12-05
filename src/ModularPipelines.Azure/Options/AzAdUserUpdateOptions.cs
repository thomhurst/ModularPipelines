using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "user", "update")]
public record AzAdUserUpdateOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliFlag("--account-enabled")]
    public bool? AccountEnabled { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--force-change-password-next-sign-in")]
    public bool? ForceChangePasswordNextSignIn { get; set; }

    [CliOption("--mail-nickname")]
    public string? MailNickname { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }
}