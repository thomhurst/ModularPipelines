using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dnc", "orchestrator-instance-service", "create")]
public record AzDncOrchestratorInstanceServiceCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--api-server-endpoint")]
    public string? ApiServerEndpoint { get; set; }

    [CommandSwitch("--cluster-root-ca")]
    public string? ClusterRootCa { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--orchestrator-app-id")]
    public string? OrchestratorAppId { get; set; }

    [CommandSwitch("--orchestrator-tenant-id")]
    public string? OrchestratorTenantId { get; set; }

    [CommandSwitch("--priv-link-resource-id")]
    public string? PrivLinkResourceId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}

