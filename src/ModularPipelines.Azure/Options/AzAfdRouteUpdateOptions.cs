using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "route", "update")]
public record AzAfdRouteUpdateOptions : AzOptions
{
    [CommandSwitch("--content-types-to-compress")]
    public string? ContentTypesToCompress { get; set; }

    [CommandSwitch("--custom-domains")]
    public string? CustomDomains { get; set; }

    [BooleanCommandSwitch("--enable-caching")]
    public bool? EnableCaching { get; set; }

    [BooleanCommandSwitch("--enable-compression")]
    public bool? EnableCompression { get; set; }

    [BooleanCommandSwitch("--enabled-state")]
    public bool? EnabledState { get; set; }

    [CommandSwitch("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CommandSwitch("--forwarding-protocol")]
    public string? ForwardingProtocol { get; set; }

    [CommandSwitch("--https-redirect")]
    public string? HttpsRedirect { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--link-to-default-domain")]
    public string? LinkToDefaultDomain { get; set; }

    [CommandSwitch("--origin-group")]
    public string? OriginGroup { get; set; }

    [CommandSwitch("--origin-path")]
    public string? OriginPath { get; set; }

    [CommandSwitch("--patterns-to-match")]
    public string? PatternsToMatch { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--query-parameters")]
    public string? QueryParameters { get; set; }

    [CommandSwitch("--query-string-caching-behavior")]
    public string? QueryStringCachingBehavior { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--route-name")]
    public string? RouteName { get; set; }

    [CommandSwitch("--rule-sets")]
    public string? RuleSets { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--supported-protocols")]
    public string? SupportedProtocols { get; set; }
}