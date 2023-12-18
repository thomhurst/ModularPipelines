using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedk8s", "enable-features")]
public record AzConnectedk8sEnableFeaturesOptions(
[property: CommandSwitch("--features")] string Features
) : AzOptions
{
    [CommandSwitch("--custom-locations-oid")]
    public string? CustomLocationsOid { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kube-config")]
    public string? KubeConfig { get; set; }

    [CommandSwitch("--kube-context")]
    public string? KubeContext { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--skip-azure-rbac-list")]
    public string? SkipAzureRbacList { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}