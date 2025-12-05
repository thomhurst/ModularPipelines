using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auth", "login")]
public record GcloudAuthLoginOptions : GcloudOptions
{
    public GcloudAuthLoginOptions(
        string account
    )
    {
        GcloudAuthLoginOptionsAccount = account;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudAuthLoginOptionsAccount { get; set; }

    [CliFlag("--activate")]
    public bool? Activate { get; set; }

    [CliFlag("--brief")]
    public bool? Brief { get; set; }

    [CliFlag("--no-browser")]
    public bool? NoBrowser { get; set; }

    [CliOption("--cred-file")]
    public string? CredFile { get; set; }

    [CliFlag("--enable-gdrive-access")]
    public bool? EnableGdriveAccess { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliFlag("--launch-browser")]
    public bool? LaunchBrowser { get; set; }

    [CliOption("--login-config")]
    public string? LoginConfig { get; set; }

    [CliFlag("--update-adc")]
    public bool? UpdateAdc { get; set; }
}
