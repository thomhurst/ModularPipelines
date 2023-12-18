using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "endpoint", "rule", "action", "add")]
public record AzCdnEndpointRuleActionAddOptions(
[property: CommandSwitch("--action-name")] string ActionName,
[property: CommandSwitch("--rule-name")] string RuleName
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

    [CommandSwitch("--name")]
    public string? Name { get; set; }

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

    [CommandSwitch("--source-pattern")]
    public string? SourcePattern { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}