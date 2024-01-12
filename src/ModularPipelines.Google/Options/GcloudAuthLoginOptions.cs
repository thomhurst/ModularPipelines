using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auth", "login")]
public record GcloudAuthLoginOptions : GcloudOptions
{
    public GcloudAuthLoginOptions(
        string account
    )
    {
        GcloudAuthLoginOptionsAccount = account;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudAuthLoginOptionsAccount { get; set; }

    [BooleanCommandSwitch("--activate")]
    public bool? Activate { get; set; }

    [BooleanCommandSwitch("--brief")]
    public bool? Brief { get; set; }

    [BooleanCommandSwitch("--no-browser")]
    public bool? NoBrowser { get; set; }

    [CommandSwitch("--cred-file")]
    public string? CredFile { get; set; }

    [BooleanCommandSwitch("--enable-gdrive-access")]
    public bool? EnableGdriveAccess { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--launch-browser")]
    public bool? LaunchBrowser { get; set; }

    [CommandSwitch("--login-config")]
    public string? LoginConfig { get; set; }

    [BooleanCommandSwitch("--update-adc")]
    public bool? UpdateAdc { get; set; }
}
