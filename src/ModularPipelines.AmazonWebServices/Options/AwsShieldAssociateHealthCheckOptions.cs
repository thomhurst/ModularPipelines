using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("shield", "associate-health-check")]
public record AwsShieldAssociateHealthCheckOptions(
[property: CommandSwitch("--protection-id")] string ProtectionId,
[property: CommandSwitch("--health-check-arn")] string HealthCheckArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}