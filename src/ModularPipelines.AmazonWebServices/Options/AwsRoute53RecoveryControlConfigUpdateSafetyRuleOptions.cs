using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-control-config", "update-safety-rule")]
public record AwsRoute53RecoveryControlConfigUpdateSafetyRuleOptions : AwsOptions
{
    [CommandSwitch("--assertion-rule-update")]
    public string? AssertionRuleUpdate { get; set; }

    [CommandSwitch("--gating-rule-update")]
    public string? GatingRuleUpdate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}