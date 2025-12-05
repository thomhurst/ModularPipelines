using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-control-config", "update-safety-rule")]
public record AwsRoute53RecoveryControlConfigUpdateSafetyRuleOptions : AwsOptions
{
    [CliOption("--assertion-rule-update")]
    public string? AssertionRuleUpdate { get; set; }

    [CliOption("--gating-rule-update")]
    public string? GatingRuleUpdate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}