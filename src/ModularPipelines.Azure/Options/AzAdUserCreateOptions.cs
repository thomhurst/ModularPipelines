using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "user", "create")]
public record AzAdUserCreateOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--password")] string Password,
[property: CliOption("--user-principal-name")] string UserPrincipalName
) : AzOptions
{
    [CliFlag("--force-change-password-next-sign-in")]
    public bool? ForceChangePasswordNextSignIn { get; set; }

    [CliOption("--immutable-id")]
    public string? ImmutableId { get; set; }

    [CliOption("--mail-nickname")]
    public string? MailNickname { get; set; }
}