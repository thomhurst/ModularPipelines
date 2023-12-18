using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "user", "create")]
public record AzAdUserCreateOptions(
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--password")] string Password,
[property: CommandSwitch("--user-principal-name")] string UserPrincipalName
) : AzOptions
{
    [BooleanCommandSwitch("--force-change-password-next-sign-in")]
    public bool? ForceChangePasswordNextSignIn { get; set; }

    [CommandSwitch("--immutable-id")]
    public string? ImmutableId { get; set; }

    [CommandSwitch("--mail-nickname")]
    public string? MailNickname { get; set; }
}