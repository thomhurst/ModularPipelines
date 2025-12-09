using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-control-config", "describe-safety-rule")]
public record AwsRoute53RecoveryControlConfigDescribeSafetyRuleOptions(
[property: CliOption("--safety-rule-arn")] string SafetyRuleArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}