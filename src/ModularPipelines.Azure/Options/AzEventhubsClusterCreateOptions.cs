using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventhubs", "cluster", "create")]
public record AzEventhubsClusterCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--capacity")]
    public string? Capacity { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--provisioning-state")]
    public string? ProvisioningState { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliFlag("--supports-scaling")]
    public bool? SupportsScaling { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}