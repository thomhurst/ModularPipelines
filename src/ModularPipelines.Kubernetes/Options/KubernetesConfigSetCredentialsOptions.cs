using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("config", "set-credentials")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigSetCredentialsOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliOption("--auth-provider")]
    public string? AuthProvider { get; set; }

    [CliOption("--auth-provider-arg")]
    public string[]? AuthProviderArg { get; set; }

    [CliFlag("--embed-certs")]
    public virtual bool? EmbedCerts { get; set; }

    [CliOption("--exec-api-version")]
    public string? ExecApiVersion { get; set; }

    [CliOption("--exec-arg")]
    public string[]? ExecArg { get; set; }

    [CliOption("--exec-command")]
    public string? ExecCommand { get; set; }

    [CliOption("--exec-env")]
    public string[]? ExecEnv { get; set; }
}