using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("config", "set-credentials")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigSetCredentialsOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliOption("--auth-provider")]
    public virtual string? AuthProvider { get; set; }

    [CliOption("--auth-provider-arg")]
    public virtual string[]? AuthProviderArg { get; set; }

    [CliFlag("--embed-certs")]
    public virtual bool? EmbedCerts { get; set; }

    [CliOption("--exec-api-version")]
    public virtual string? ExecApiVersion { get; set; }

    [CliOption("--exec-arg")]
    public virtual string[]? ExecArg { get; set; }

    [CliOption("--exec-command")]
    public virtual string? ExecCommand { get; set; }

    [CliOption("--exec-env")]
    public virtual string[]? ExecEnv { get; set; }
}