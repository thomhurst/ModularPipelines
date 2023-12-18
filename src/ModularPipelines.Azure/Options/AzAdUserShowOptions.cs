using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "user", "show")]
public record AzAdUserShowOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [BooleanCommandSwitch("--account-enabled")]
    public bool? AccountEnabled { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--force-change-password-next-sign-in")]
    public bool? ForceChangePasswordNextSignIn { get; set; }

    [CommandSwitch("--mail-nickname")]
    public string? MailNickname { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }
}