using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "service", "create")]
public record AzSfServiceCreateOptions(
[property: CliOption("--application")] string Application,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-type")] string ServiceType,
[property: CliOption("--state")] string State
) : AzOptions
{
    [CliOption("--default-move-cost")]
    public string? DefaultMoveCost { get; set; }

    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--min-replica")]
    public string? MinReplica { get; set; }

    [CliOption("--partition-scheme")]
    public string? PartitionScheme { get; set; }

    [CliOption("--target-replica")]
    public string? TargetReplica { get; set; }
}