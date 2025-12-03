using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("anthos", "auth", "login")]
public record GcloudAnthosAuthLoginOptions : GcloudOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliOption("--kubeconfig")]
    public string? Kubeconfig { get; set; }

    [CliOption("--login-config")]
    public string? LoginConfig { get; set; }

    [CliOption("--login-config-cert")]
    public string? LoginConfigCert { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliFlag("--set-preferred-auth")]
    public bool? SetPreferredAuth { get; set; }

    [CliOption("--user")]
    public string? User { get; set; }
}