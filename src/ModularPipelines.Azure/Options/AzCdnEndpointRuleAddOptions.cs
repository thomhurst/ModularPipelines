using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "endpoint", "rule", "add")]
public record AzCdnEndpointRuleAddOptions(
[property: CommandSwitch("--action-name")] string ActionName,
[property: CommandSwitch("--order")] string Order
) : AzOptions
{
    [CommandSwitch("--cache-behavior")]
    public string? CacheBehavior { get; set; }

    [CommandSwitch("--cache-duration")]
    public string? CacheDuration { get; set; }

    [CommandSwitch("--custom-fragment")]
    public string? CustomFragment { get; set; }

    [CommandSwitch("--custom-hostname")]
    public string? CustomHostname { get; set; }

    [CommandSwitch("--custom-path")]
    public string? CustomPath { get; set; }

    [CommandSwitch("--custom-querystring")]
    public string? CustomQuerystring { get; set; }

    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--header-action")]
    public string? HeaderAction { get; set; }

    [CommandSwitch("--header-name")]
    public string? HeaderName { get; set; }

    [CommandSwitch("--header-value")]
    public string? HeaderValue { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--match-values")]
    public string? MatchValues { get; set; }

    [CommandSwitch("--match-variable")]
    public string? MatchVariable { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--negate-condition")]
    public bool? NegateCondition { get; set; }

    [CommandSwitch("--operator")]
    public string? Operator { get; set; }

    [CommandSwitch("--origin-group")]
    public string? OriginGroup { get; set; }

    [BooleanCommandSwitch("--preserve-unmatched-path")]
    public bool? PreserveUnmatchedPath { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--query-parameters")]
    public string? QueryParameters { get; set; }

    [CommandSwitch("--query-string-behavior")]
    public string? QueryStringBehavior { get; set; }

    [CommandSwitch("--redirect-protocol")]
    public string? RedirectProtocol { get; set; }

    [CommandSwitch("--redirect-type")]
    public string? RedirectType { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-name")]
    public string? RuleName { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }

    [CommandSwitch("--source-pattern")]
    public string? SourcePattern { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--transform")]
    public string? Transform { get; set; }
}