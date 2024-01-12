using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auth", "application-default", "login")]
public record GcloudAuthApplicationDefaultLoginOptions : GcloudOptions
{
    [BooleanCommandSwitch("--no-browser")]
    public bool? NoBrowser { get; set; }

    [CommandSwitch("--client-id-file")]
    public string? ClientIdFile { get; set; }

    [BooleanCommandSwitch("--disable-quota-project")]
    public bool? DisableQuotaProject { get; set; }

    [BooleanCommandSwitch("--launch-browser")]
    public bool? LaunchBrowser { get; set; }

    [CommandSwitch("--login-config")]
    public string? LoginConfig { get; set; }

    [CommandSwitch("--scopes")]
    public string[]? Scopes { get; set; }
}