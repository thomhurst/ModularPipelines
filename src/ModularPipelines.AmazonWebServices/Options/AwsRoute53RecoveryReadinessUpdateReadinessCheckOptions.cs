using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-readiness", "update-readiness-check")]
public record AwsRoute53RecoveryReadinessUpdateReadinessCheckOptions(
[property: CommandSwitch("--readiness-check-name")] string ReadinessCheckName,
[property: CommandSwitch("--resource-set-name")] string ResourceSetName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}