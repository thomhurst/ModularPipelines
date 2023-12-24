using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("drs", "get-failback-replication-configuration")]
public record AwsDrsGetFailbackReplicationConfigurationOptions(
[property: CommandSwitch("--recovery-instance-id")] string RecoveryInstanceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}