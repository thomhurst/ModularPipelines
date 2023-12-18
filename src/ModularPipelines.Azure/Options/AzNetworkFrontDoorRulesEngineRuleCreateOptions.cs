using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "rules-engine", "rule", "create")]
public record AzNetworkFrontDoorRulesEngineRuleCreateOptions(
[property: CommandSwitch("--action-type")] string ActionType,
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--priority")] string Priority,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rules-engine-name")] string RulesEngineName
) : AzOptions
{
    [CommandSwitch("--header-action")]
    public string? HeaderAction { get; set; }

    [CommandSwitch("--header-name")]
    public string? HeaderName { get; set; }

    [CommandSwitch("--header-value")]
    public string? HeaderValue { get; set; }

    [CommandSwitch("--match-processing-behavior")]
    public string? MatchProcessingBehavior { get; set; }

    [CommandSwitch("--match-values")]
    public string? MatchValues { get; set; }

    [CommandSwitch("--match-variable")]
    public string? MatchVariable { get; set; }

    [BooleanCommandSwitch("--negate-condition")]
    public bool? NegateCondition { get; set; }

    [CommandSwitch("--operator")]
    public string? Operator { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }

    [CommandSwitch("--transforms")]
    public string? Transforms { get; set; }
}