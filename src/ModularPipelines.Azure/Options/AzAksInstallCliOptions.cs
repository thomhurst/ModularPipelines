using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "install-cli")]
public record AzAksInstallCliOptions : AzOptions
{
    [CliOption("--base-src-url")]
    public string? BaseSrcUrl { get; set; }

    [CliOption("--client-version")]
    public string? ClientVersion { get; set; }

    [CliOption("--install-location")]
    public string? InstallLocation { get; set; }

    [CliOption("--kubelogin-base-src-url")]
    public string? KubeloginBaseSrcUrl { get; set; }

    [CliOption("--kubelogin-install-location")]
    public string? KubeloginInstallLocation { get; set; }

    [CliOption("--kubelogin-version")]
    public string? KubeloginVersion { get; set; }
}