using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Helm.Options;

[ExcludeFromCodeCoverage]
public record HelmOptions() : CommandLineToolOptions("helm")
{
    [CommandEqualsSeparatorSwitch("--burst-limit")]
    public int? BurstLimit { get; set; }

    [BooleanCommandSwitch("--debug")]
    public bool? Debug { get; set; }

    [CommandEqualsSeparatorSwitch("--kube-apiserver")]
    public string? KubeApiServer { get; set; }

    [CommandEqualsSeparatorSwitch("--kube-as-group")]
    public string[]? KubeAsGroup { get; set; }

    [CommandEqualsSeparatorSwitch("--kube-as-user")]
    public string? KubeAsUser { get; set; }

    [CommandEqualsSeparatorSwitch("--kube-ca-file")]
    public string? KubeCaFile { get; set; }

    [CommandEqualsSeparatorSwitch("--kube-context")]
    public string? KubeContext { get; set; }

    [CommandEqualsSeparatorSwitch("--kube-insecure-skip-tls-verify")]
    public string? KubeInsecureSkipTlsVerify { get; set; }

    [CommandEqualsSeparatorSwitch("--kube-tls-server-name")]
    public string? KubeTlsServerName { get; set; }

    [CommandEqualsSeparatorSwitch("--kube-token")]
    public string? KubeToken { get; set; }

    [CommandEqualsSeparatorSwitch("--kubeconfig")]
    public string? KubeConfig { get; set; }

    [CommandEqualsSeparatorSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandEqualsSeparatorSwitch("--registry-config")]
    public string? RegistryConfig { get; set; }

    [CommandEqualsSeparatorSwitch("--repository-cache")]
    public string? RepositoryCache { get; set; }

    [CommandEqualsSeparatorSwitch("--repository-config")]
    public string? RepositoryConfig { get; set; }
}