using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "waf-policy", "managed-rules", "override", "add")]
public record AzNetworkFrontDoorWafPolicyManagedRulesOverrideAddOptions(
[property: CliOption("--rule-group-id")] string RuleGroupId,
[property: CliOption("--rule-id")] string RuleId,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--policy-name")]
    public string? PolicyName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}