using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "user", "update")]
public record AzAdUserUpdateOptions(
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