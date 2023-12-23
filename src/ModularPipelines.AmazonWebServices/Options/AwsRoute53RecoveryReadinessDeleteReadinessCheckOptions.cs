using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-readiness", "delete-readiness-check")]
public record AwsRoute53RecoveryReadinessDeleteReadinessCheckOptions(
[property: CommandSwitch("--readiness-check-name")] string ReadinessCheckName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}