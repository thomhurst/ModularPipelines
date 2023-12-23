using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("drs", "terminate-recovery-instances")]
public record AwsDrsTerminateRecoveryInstancesOptions(
[property: CommandSwitch("--recovery-instance-ids")] string[] RecoveryInstanceIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}