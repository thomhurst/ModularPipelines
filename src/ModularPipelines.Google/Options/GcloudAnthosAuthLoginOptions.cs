using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("anthos", "auth", "login")]
public record GcloudAnthosAuthLoginOptions : GcloudOptions
{
    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandSwitch("--kubeconfig")]
    public string? Kubeconfig { get; set; }

    [CommandSwitch("--login-config")]
    public string? LoginConfig { get; set; }

    [CommandSwitch("--login-config-cert")]
    public string? LoginConfigCert { get; set; }

    [CommandSwitch("--server")]
    public string? Server { get; set; }

    [BooleanCommandSwitch("--set-preferred-auth")]
    public bool? SetPreferredAuth { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }
}