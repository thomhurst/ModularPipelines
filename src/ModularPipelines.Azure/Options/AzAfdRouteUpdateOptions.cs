using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "route", "update")]
public record AzAfdRouteUpdateOptions : AzOptions
{
    [CliOption("--content-types-to-compress")]
    public string? ContentTypesToCompress { get; set; }

    [CliOption("--custom-domains")]
    public string? CustomDomains { get; set; }

    [CliFlag("--enable-caching")]
    public bool? EnableCaching { get; set; }

    [CliFlag("--enable-compression")]
    public bool? EnableCompression { get; set; }

    [CliFlag("--enabled-state")]
    public bool? EnabledState { get; set; }

    [CliOption("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CliOption("--forwarding-protocol")]
    public string? ForwardingProtocol { get; set; }

    [CliOption("--https-redirect")]
    public string? HttpsRedirect { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--link-to-default-domain")]
    public string? LinkToDefaultDomain { get; set; }

    [CliOption("--origin-group")]
    public string? OriginGroup { get; set; }

    [CliOption("--origin-path")]
    public string? OriginPath { get; set; }

    [CliOption("--patterns-to-match")]
    public string? PatternsToMatch { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--query-parameters")]
    public string? QueryParameters { get; set; }

    [CliOption("--query-string-caching-behavior")]
    public string? QueryStringCachingBehavior { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--route-name")]
    public string? RouteName { get; set; }

    [CliOption("--rule-sets")]
    public string? RuleSets { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--supported-protocols")]
    public string? SupportedProtocols { get; set; }
}