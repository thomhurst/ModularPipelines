using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "route", "create")]
public record AzAfdRouteCreateOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--forwarding-protocol")] string ForwardingProtocol,
[property: CommandSwitch("--https-redirect")] string HttpsRedirect,
[property: CommandSwitch("--origin-group")] string OriginGroup,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--route-name")] string RouteName,
[property: CommandSwitch("--supported-protocols")] string SupportedProtocols
) : AzOptions
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

    [CommandSwitch("--link-to-default-domain")]
    public string? LinkToDefaultDomain { get; set; }

    [CommandSwitch("--origin-path")]
    public string? OriginPath { get; set; }

    [CommandSwitch("--patterns-to-match")]
    public string? PatternsToMatch { get; set; }

    [CommandSwitch("--query-parameters")]
    public string? QueryParameters { get; set; }

    [CommandSwitch("--query-string-caching-behavior")]
    public string? QueryStringCachingBehavior { get; set; }

    [CommandSwitch("--rule-sets")]
    public string? RuleSets { get; set; }
}