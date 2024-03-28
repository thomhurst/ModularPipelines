using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigSetClusterOptions : KubernetesOptions
{
    public KubernetesConfigSetClusterOptions()
    {
        CommandParts = ["config", "set-cluster"];
    }

    [CommandSwitch("--certificate-authority")]
    public string? CertificateAuthority { get; set; }

    [BooleanCommandSwitch("--embed-certs")]
    public bool? EmbedCerts { get; set; }

    [CommandSwitch("--insecure-skip-tls-verify")]
    public string? InsecureSkipTlsVerify { get; set; }

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? NAME { get; set; }

    [CommandSwitch("--server")]
    public string? Server { get; set; }

    [CommandSwitch("--tls-server-name")]
    public string? TlsServerName { get; set; }
}
