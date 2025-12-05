using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dnc", "orchestrator-instance-service", "create")]
public record AzDncOrchestratorInstanceServiceCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--api-server-endpoint")]
    public string? ApiServerEndpoint { get; set; }

    [CliOption("--cluster-root-ca")]
    public string? ClusterRootCa { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--orchestrator-app-id")]
    public string? OrchestratorAppId { get; set; }

    [CliOption("--orchestrator-tenant-id")]
    public string? OrchestratorTenantId { get; set; }

    [CliOption("--priv-link-resource-id")]
    public string? PrivLinkResourceId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}