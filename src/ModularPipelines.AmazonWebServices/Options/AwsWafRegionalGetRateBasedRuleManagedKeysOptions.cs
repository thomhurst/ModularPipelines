using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf-regional", "get-rate-based-rule-managed-keys")]
public record AwsWafRegionalGetRateBasedRuleManagedKeysOptions(
[property: CliOption("--rule-id")] string RuleId
) : AwsOptions
{
    [CliOption("--next-marker")]
    public string? NextMarker { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}