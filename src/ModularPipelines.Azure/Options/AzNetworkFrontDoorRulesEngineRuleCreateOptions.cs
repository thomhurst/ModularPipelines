using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "front-door", "rules-engine", "rule", "create")]
public record AzNetworkFrontDoorRulesEngineRuleCreateOptions(
[property: CliOption("--action-type")] string ActionType,
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--priority")] string Priority,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rules-engine-name")] string RulesEngineName
) : AzOptions
{
    [CliOption("--header-action")]
    public string? HeaderAction { get; set; }

    [CliOption("--header-name")]
    public string? HeaderName { get; set; }

    [CliOption("--header-value")]
    public string? HeaderValue { get; set; }

    [CliOption("--match-processing-behavior")]
    public string? MatchProcessingBehavior { get; set; }

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