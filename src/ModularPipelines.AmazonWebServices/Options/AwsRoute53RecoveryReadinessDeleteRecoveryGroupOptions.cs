using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-readiness", "delete-recovery-group")]
public record AwsRoute53RecoveryReadinessDeleteRecoveryGroupOptions(
[property: CommandSwitch("--recovery-group-name")] string RecoveryGroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}