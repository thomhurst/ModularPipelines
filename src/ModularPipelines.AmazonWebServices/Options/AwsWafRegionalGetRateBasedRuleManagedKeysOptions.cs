using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf-regional", "get-rate-based-rule-managed-keys")]
public record AwsWafRegionalGetRateBasedRuleManagedKeysOptions(
[property: CommandSwitch("--rule-id")] string RuleId
) : AwsOptions
{
    [CommandSwitch("--next-marker")]
    public string? NextMarker { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}