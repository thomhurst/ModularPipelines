using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("drs", "get-failback-replication-configuration")]
public record AwsDrsGetFailbackReplicationConfigurationOptions(
[property: CliOption("--recovery-instance-id")] string RecoveryInstanceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}