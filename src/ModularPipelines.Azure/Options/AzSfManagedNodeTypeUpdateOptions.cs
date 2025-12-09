using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "managed-node-type", "update")]
public record AzSfManagedNodeTypeUpdateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--app-end-port")]
    public string? AppEndPort { get; set; }

    [CliOption("--app-start-port")]
    public string? AppStartPort { get; set; }

    [CliOption("--capacity")]
    public string? Capacity { get; set; }

    [CliOption("--ephemeral-end-port")]
    public string? EphemeralEndPort { get; set; }

    [CliOption("--ephemeral-start-port")]
    public string? EphemeralStartPort { get; set; }

    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--placement-property")]
    public string? PlacementProperty { get; set; }
}