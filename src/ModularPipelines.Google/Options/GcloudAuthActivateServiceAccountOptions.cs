using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auth", "activate-service-account")]
public record GcloudAuthActivateServiceAccountOptions : GcloudOptions
{
    public GcloudAuthActivateServiceAccountOptions(
        string account,
        string keyFile
    )
    {
        GcloudAuthActivateServiceAccountOptionsAccount = account;
        KeyFile = keyFile;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudAuthActivateServiceAccountOptionsAccount { get; set; }

    [CommandSwitch("--key-file")]
    public string KeyFile { get; set; }

    [CommandSwitch("--password-file")]
    public string? PasswordFile { get; set; }

    [BooleanCommandSwitch("--prompt-for-password")]
    public bool? PromptForPassword { get; set; }
}
