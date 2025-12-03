using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "front-door", "routing-rule", "create")]
public record AzNetworkFrontDoorRoutingRuleCreateOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--frontend-endpoints")] string FrontendEndpoints,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--route-type")] string RouteType
) : AzOptions
{
    [CliOption("--accepted-protocols")]
    public string? AcceptedProtocols { get; set; }

    [CliOption("--backend-pool")]
    public string? BackendPool { get; set; }

    [CliOption("--cache-duration")]
    public string? CacheDuration { get; set; }

    [CliOption("--caching")]
    public string? Caching { get; set; }

    [CliOption("--custom-forwarding-path")]
    public string? CustomForwardingPath { get; set; }

    [CliOption("--custom-fragment")]
    public string? CustomFragment { get; set; }

    [CliOption("--custom-host")]
    public string? CustomHost { get; set; }

    [CliOption("--custom-path")]
    public string? CustomPath { get; set; }

    [CliOption("--custom-query-string")]
    public string? CustomQueryString { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--dynamic-compression")]
    public string? DynamicCompression { get; set; }

    [CliOption("--forwarding-protocol")]
    public string? ForwardingProtocol { get; set; }

    [CliOption("--patterns")]
    public string? Patterns { get; set; }

    [CliOption("--query-parameter-strip-directive")]
    public string? QueryParameterStripDirective { get; set; }

    [CliOption("--query-parameters")]
    public string? QueryParameters { get; set; }

    [CliOption("--redirect-protocol")]
    public string? RedirectProtocol { get; set; }

    [CliOption("--redirect-type")]
    public string? RedirectType { get; set; }

    [CliOption("--rules-engine")]
    public string? RulesEngine { get; set; }
}