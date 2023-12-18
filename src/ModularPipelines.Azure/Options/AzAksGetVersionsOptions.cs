using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "get-versions")]
public record AzAksGetVersionsOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [CommandSwitch("--base-src-url")]
    public string? BaseSrcUrl { get; set; }

    [CommandSwitch("--client-version")]
    public string? ClientVersion { get; set; }

    [CommandSwitch("--install-location")]
    public string? InstallLocation { get; set; }

    [CommandSwitch("--kubelogin-base-src-url")]
    public string? KubeloginBaseSrcUrl { get; set; }

    [CommandSwitch("--kubelogin-install-location")]
    public string? KubeloginInstallLocation { get; set; }

    [CommandSwitch("--kubelogin-version")]
    public string? KubeloginVersion { get; set; }
}