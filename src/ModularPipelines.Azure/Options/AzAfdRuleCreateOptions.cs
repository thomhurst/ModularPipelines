using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "rule", "create")]
public record AzAfdRuleCreateOptions(
[property: CliOption("--action-name")] string ActionName,
[property: CliOption("--order")] string Order,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-name")] string RuleName,
[property: CliOption("--rule-set-name")] string RuleSetName
) : AzOptions
{
    [CliOption("--cache-behavior")]
    public string? CacheBehavior { get; set; }

    [CliOption("--cache-duration")]
    public string? CacheDuration { get; set; }

    [CliOption("--custom-fragment")]
    public string? CustomFragment { get; set; }

    [CliOption("--custom-hostname")]
    public string? CustomHostname { get; set; }

    [CliOption("--custom-path")]
    public string? CustomPath { get; set; }

    [CliOption("--custom-querystring")]
    public string? CustomQuerystring { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliFlag("--enable-caching")]
    public bool? EnableCaching { get; set; }

    [CliFlag("--enable-compression")]
    public bool? EnableCompression { get; set; }

    [CliOption("--forwarding-protocol")]
    public string? ForwardingProtocol { get; set; }

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

    [CliOption("--origin-group")]
    public string? OriginGroup { get; set; }

    [CliFlag("--preserve-unmatched-path")]
    public bool? PreserveUnmatchedPath { get; set; }

    [CliOption("--query-parameters")]
    public string? QueryParameters { get; set; }

    [CliOption("--query-string-caching-behavior")]
    public string? QueryStringCachingBehavior { get; set; }

    [CliOption("--redirect-protocol")]
    public string? RedirectProtocol { get; set; }

    [CliOption("--redirect-type")]
    public string? RedirectType { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }

    [CliOption("--source-pattern")]
    public string? SourcePattern { get; set; }

    [CliOption("--transforms")]
    public string? Transforms { get; set; }
}