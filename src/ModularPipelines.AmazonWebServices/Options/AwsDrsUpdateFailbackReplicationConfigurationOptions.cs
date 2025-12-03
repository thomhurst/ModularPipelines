using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("drs", "update-failback-replication-configuration")]
public record AwsDrsUpdateFailbackReplicationConfigurationOptions(
[property: CliOption("--recovery-instance-id")] string RecoveryInstanceId
) : AwsOptions
{
    [CliOption("--bandwidth-throttling")]
    public long? BandwidthThrottling { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}