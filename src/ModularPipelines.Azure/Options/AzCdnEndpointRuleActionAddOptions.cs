using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cdn", "endpoint", "rule", "action", "add")]
public record AzCdnEndpointRuleActionAddOptions(
[property: CliOption("--action-name")] string ActionName,
[property: CliOption("--rule-name")] string RuleName
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

    [CliOption("--header-action")]
    public string? HeaderAction { get; set; }

    [CliOption("--header-name")]
    public string? HeaderName { get; set; }

    [CliOption("--header-value")]
    public string? HeaderValue { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--origin-group")]
    public string? OriginGroup { get; set; }

    [CliFlag("--preserve-unmatched-path")]
    public bool? PreserveUnmatchedPath { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--query-parameters")]
    public string? QueryParameters { get; set; }

    [CliOption("--query-string-behavior")]
    public string? QueryStringBehavior { get; set; }

    [CliOption("--redirect-protocol")]
    public string? RedirectProtocol { get; set; }

    [CliOption("--redirect-type")]
    public string? RedirectType { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--source-pattern")]
    public string? SourcePattern { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}