using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "rule", "action", "add")]
public record AzAfdRuleActionAddOptions(
[property: CommandSwitch("--action-name")] string ActionName
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

    [BooleanCommandSwitch("--enable-caching")]
    public bool? EnableCaching { get; set; }

    [BooleanCommandSwitch("--enable-compression")]
    public bool? EnableCompression { get; set; }

    [CommandSwitch("--forwarding-protocol")]
    public string? ForwardingProtocol { get; set; }

    [CommandSwitch("--header-action")]
    public string? HeaderAction { get; set; }

    [CommandSwitch("--header-name")]
    public string? HeaderName { get; set; }

    [CommandSwitch("--header-value")]
    public string? HeaderValue { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--origin-group")]
    public string? OriginGroup { get; set; }

    [BooleanCommandSwitch("--preserve-unmatched-path")]
    public bool? PreserveUnmatchedPath { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--query-parameters")]
    public string? QueryParameters { get; set; }

    [CommandSwitch("--query-string-caching-behavior")]
    public string? QueryStringCachingBehavior { get; set; }

    [CommandSwitch("--redirect-protocol")]
    public string? RedirectProtocol { get; set; }

    [CommandSwitch("--redirect-type")]
    public string? RedirectType { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-name")]
    public string? RuleName { get; set; }

    [CommandSwitch("--rule-set-name")]
    public string? RuleSetName { get; set; }

    [CommandSwitch("--source-pattern")]
    public string? SourcePattern { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

