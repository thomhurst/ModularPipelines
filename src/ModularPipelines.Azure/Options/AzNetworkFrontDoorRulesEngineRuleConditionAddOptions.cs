using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "front-door", "rules-engine", "rule", "condition", "add")]
public record AzNetworkFrontDoorRulesEngineRuleConditionAddOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rules-engine-name")] string RulesEngineName
) : AzOptions
{
    [CliOption("--match-values")]
    public string? MatchValues { get; set; }

    [CliOption("--match-variable")]
    public string? MatchVariable { get; set; }

    [CliFlag("--negate-condition")]
    public bool? NegateCondition { get; set; }

    [CliOption("--operator")]
    public string? Operator { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }

    [CliOption("--transforms")]
    public string? Transforms { get; set; }
}