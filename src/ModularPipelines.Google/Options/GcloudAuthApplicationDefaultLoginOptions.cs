using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auth", "application-default", "login")]
public record GcloudAuthApplicationDefaultLoginOptions : GcloudOptions
{
    [CliFlag("--no-browser")]
    public bool? NoBrowser { get; set; }

    [CliOption("--client-id-file")]
    public string? ClientIdFile { get; set; }

    [CliFlag("--disable-quota-project")]
    public bool? DisableQuotaProject { get; set; }

    [CliFlag("--launch-browser")]
    public bool? LaunchBrowser { get; set; }

    [CliOption("--login-config")]
    public string? LoginConfig { get; set; }

    [CliOption("--scopes")]
    public string[]? Scopes { get; set; }
}