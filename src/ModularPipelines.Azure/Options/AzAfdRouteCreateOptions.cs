using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "route", "create")]
public record AzAfdRouteCreateOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--forwarding-protocol")] string ForwardingProtocol,
[property: CliOption("--https-redirect")] string HttpsRedirect,
[property: CliOption("--origin-group")] string OriginGroup,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--route-name")] string RouteName,
[property: CliOption("--supported-protocols")] string SupportedProtocols
) : AzOptions
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

    [CliOption("--link-to-default-domain")]
    public string? LinkToDefaultDomain { get; set; }

    [CliOption("--origin-path")]
    public string? OriginPath { get; set; }

    [CliOption("--patterns-to-match")]
    public string? PatternsToMatch { get; set; }

    [CliOption("--query-parameters")]
    public string? QueryParameters { get; set; }

    [CliOption("--query-string-caching-behavior")]
    public string? QueryStringCachingBehavior { get; set; }

    [CliOption("--rule-sets")]
    public string? RuleSets { get; set; }
}