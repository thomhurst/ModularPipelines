using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nginx", "deployment", "update")]
public record AzNginxDeploymentUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--deployment-name")]
    public string? DeploymentName { get; set; }

    [BooleanCommandSwitch("--enable-diagnostics")]
    public bool? EnableDiagnostics { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--logging")]
    public string? Logging { get; set; }

    [CommandSwitch("--managed-resource-group")]
    public string? ManagedResourceGroup { get; set; }

    [CommandSwitch("--network-profile")]
    public string? NetworkProfile { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--provisioning-state")]
    public string? ProvisioningState { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}