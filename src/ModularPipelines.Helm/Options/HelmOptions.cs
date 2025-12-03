using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Helm.Options;

[ExcludeFromCodeCoverage]
public record HelmOptions() : CommandLineToolOptions("helm")
{
    [CliOption("--burst-limit", Format = OptionFormat.EqualsSeparated)]
    public int? BurstLimit { get; set; }

    [CliFlag("--debug")]
    public virtual bool? Debug { get; set; }

    [CliOption("--kube-apiserver", Format = OptionFormat.EqualsSeparated)]
    public string? KubeApiServer { get; set; }

    [CliOption("--kube-as-group", Format = OptionFormat.EqualsSeparated)]
    public string[]? KubeAsGroup { get; set; }

    [CliOption("--kube-as-user", Format = OptionFormat.EqualsSeparated)]
    public string? KubeAsUser { get; set; }

    [CliOption("--kube-ca-file", Format = OptionFormat.EqualsSeparated)]
    public string? KubeCaFile { get; set; }

    [CliOption("--kube-context", Format = OptionFormat.EqualsSeparated)]
    public string? KubeContext { get; set; }

    [CliOption("--kube-insecure-skip-tls-verify", Format = OptionFormat.EqualsSeparated)]
    public string? KubeInsecureSkipTlsVerify { get; set; }

    [CliOption("--kube-tls-server-name", Format = OptionFormat.EqualsSeparated)]
    public string? KubeTlsServerName { get; set; }

    [CliOption("--kube-token", Format = OptionFormat.EqualsSeparated)]
    public string? KubeToken { get; set; }

    [CliOption("--kubeconfig", Format = OptionFormat.EqualsSeparated)]
    public string? KubeConfig { get; set; }

    [CliOption("--namespace", Format = OptionFormat.EqualsSeparated)]
    public string? Namespace { get; set; }

    [CliOption("--registry-config", Format = OptionFormat.EqualsSeparated)]
    public string? RegistryConfig { get; set; }

    [CliOption("--repository-cache", Format = OptionFormat.EqualsSeparated)]
    public string? RepositoryCache { get; set; }

    [CliOption("--repository-config", Format = OptionFormat.EqualsSeparated)]
    public string? RepositoryConfig { get; set; }
}