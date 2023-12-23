using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-readiness", "get-readiness-check")]
public record AwsRoute53RecoveryReadinessGetReadinessCheckOptions(
[property: CommandSwitch("--readiness-check-name")] string ReadinessCheckName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}