using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-control-config", "describe-safety-rule")]
public record AwsRoute53RecoveryControlConfigDescribeSafetyRuleOptions(
[property: CommandSwitch("--safety-rule-arn")] string SafetyRuleArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}