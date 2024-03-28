using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigSetCredentialsOptions : KubernetesOptions
{
    public KubernetesConfigSetCredentialsOptions()
    {
        CommandParts = ["config", "set-credentials"];
    }

    [CommandSwitch("--auth-provider")]
    public string? AuthProvider { get; set; }

    [CommandSwitch("--auth-provider-arg")]
    public IEnumerable<string>? AuthProviderArg { get; set; }

    [CommandSwitch("--client-certificate")]
    public string? ClientCertificate { get; set; }

    [CommandSwitch("--client-key")]
    public string? ClientKey { get; set; }

    [BooleanCommandSwitch("--embed-certs")]
    public bool? EmbedCerts { get; set; }

    [CommandSwitch("--exec-api-version")]
    public string? ExecApiVersion { get; set; }

    [CommandSwitch("--exec-arg")]
    public IEnumerable<string>? ExecArg { get; set; }

    [CommandSwitch("--exec-command")]
    public string? ExecCommand { get; set; }

    [CommandSwitch("--exec-env")]
    public IEnumerable<string>? ExecEnv { get; set; }

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? NAME { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}
