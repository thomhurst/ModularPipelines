using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dax", "decrease-replication-factor")]
public record AwsDaxDecreaseReplicationFactorOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--new-replication-factor")] int NewReplicationFactor
) : AwsOptions
{
    [CommandSwitch("--availability-zones")]
    public string[]? AvailabilityZones { get; set; }

    [CommandSwitch("--node-ids-to-remove")]
    public string[]? NodeIdsToRemove { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}