using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auth", "activate-service-account")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudAuthActivateServiceAccountOptionsAccount { get; set; }

    [CliOption("--key-file")]
    public string KeyFile { get; set; }

    [CliOption("--password-file")]
    public string? PasswordFile { get; set; }

    [CliFlag("--prompt-for-password")]
    public bool? PromptForPassword { get; set; }
}
