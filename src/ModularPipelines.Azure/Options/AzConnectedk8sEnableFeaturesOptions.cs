using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectedk8s", "enable-features")]
public record AzConnectedk8sEnableFeaturesOptions(
[property: CliOption("--features")] string Features
) : AzOptions
{
    [CliOption("--custom-locations-oid")]
    public string? CustomLocationsOid { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kube-config")]
    public string? KubeConfig { get; set; }

    [CliOption("--kube-context")]
    public string? KubeContext { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--skip-azure-rbac-list")]
    public string? SkipAzureRbacList { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}