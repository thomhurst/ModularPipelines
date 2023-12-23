using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("drs", "update-failback-replication-configuration")]
public record AwsDrsUpdateFailbackReplicationConfigurationOptions(
[property: CommandSwitch("--recovery-instance-id")] string RecoveryInstanceId
) : AwsOptions
{
    [CommandSwitch("--bandwidth-throttling")]
    public long? BandwidthThrottling { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}