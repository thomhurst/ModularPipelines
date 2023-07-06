using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Helm.Options;

public record HelmOptions() : CommandLineToolOptions("helm")
{
    [CommandLongSwitch("burst-limit")]
    public int? BurstLimit { get; set; }

    [BooleanCommandSwitch("debug")]
    public bool? Debug { get; set; }

    [CommandLongSwitch("kube-apiserver")]
    public string? KubeApiServer { get; set; }

    [CommandLongSwitch("kube-as-group")]
    public string[]? KubeAsGroup { get; set; }

    [CommandLongSwitch("kube-as-user")]
    public string? KubeAsUser { get; set; }

    [CommandLongSwitch("kube-ca-file")]
    public string? KubeCaFile { get; set; }

    [CommandLongSwitch("kube-context")]
    public string? KubeContext { get; set; }

    [CommandLongSwitch("kube-insecure-skip-tls-verify")]
    public string? KubeInsecureSkipTlsVerify { get; set; }

    [CommandLongSwitch("kube-tls-server-name")]
    public string? KubeTlsServerName { get; set; }

    [CommandLongSwitch("kube-token")]
    public string? KubeToken { get; set; }

    [CommandLongSwitch("kubeconfig")]
    public string? KubeConfig { get; set; }

    [CommandLongSwitch("namespace")]
    public string? Namespace { get; set; }

    [CommandLongSwitch("registry-config")]
    public string? RegistryConfig { get; set; }

    [CommandLongSwitch("repository-cache")]
    public string? RepositoryCache { get; set; }

    [CommandLongSwitch("repository-config")]
    public string? RepositoryConfig { get; set; }
};
