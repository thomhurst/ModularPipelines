using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Helm.Options;

[ExcludeFromCodeCoverage]
public record HelmOptions() : CommandLineToolOptions("helm")
{
    [CliOption("--burst-limit", Format = OptionFormat.EqualsSeparated)]
    public virtual int? BurstLimit { get; set; }

    [CliFlag("--debug")]
    public virtual bool? Debug { get; set; }

    [CliOption("--kube-apiserver", Format = OptionFormat.EqualsSeparated)]
    public virtual string? KubeApiServer { get; set; }

    [CliOption("--kube-as-group", Format = OptionFormat.EqualsSeparated)]
    public virtual string[]? KubeAsGroup { get; set; }

    [CliOption("--kube-as-user", Format = OptionFormat.EqualsSeparated)]
    public virtual string? KubeAsUser { get; set; }

    [CliOption("--kube-ca-file", Format = OptionFormat.EqualsSeparated)]
    public virtual string? KubeCaFile { get; set; }

    [CliOption("--kube-context", Format = OptionFormat.EqualsSeparated)]
    public virtual string? KubeContext { get; set; }

    [CliOption("--kube-insecure-skip-tls-verify", Format = OptionFormat.EqualsSeparated)]
    public virtual string? KubeInsecureSkipTlsVerify { get; set; }

    [CliOption("--kube-tls-server-name", Format = OptionFormat.EqualsSeparated)]
    public virtual string? KubeTlsServerName { get; set; }

    [CliOption("--kube-token", Format = OptionFormat.EqualsSeparated)]
    public virtual string? KubeToken { get; set; }

    [CliOption("--kubeconfig", Format = OptionFormat.EqualsSeparated)]
    public virtual string? KubeConfig { get; set; }

    [CliOption("--namespace", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Namespace { get; set; }

    [CliOption("--registry-config", Format = OptionFormat.EqualsSeparated)]
    public virtual string? RegistryConfig { get; set; }

    [CliOption("--repository-cache", Format = OptionFormat.EqualsSeparated)]
    public virtual string? RepositoryCache { get; set; }

    [CliOption("--repository-config", Format = OptionFormat.EqualsSeparated)]
    public virtual string? RepositoryConfig { get; set; }
}