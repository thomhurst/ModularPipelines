using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nginx", "deployment", "create")]
public record AzNginxDeploymentCreateOptions(
[property: CliOption("--deployment-name")] string DeploymentName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--enable-diagnostics")]
    public bool? EnableDiagnostics { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--logging")]
    public string? Logging { get; set; }

    [CliOption("--managed-resource-group")]
    public string? ManagedResourceGroup { get; set; }

    [CliOption("--network-profile")]
    public string? NetworkProfile { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--provisioning-state")]
    public string? ProvisioningState { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}