using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("drs", "terminate-recovery-instances")]
public record AwsDrsTerminateRecoveryInstancesOptions(
[property: CliOption("--recovery-instance-ids")] string[] RecoveryInstanceIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}