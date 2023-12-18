using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "routing-rule", "create")]
public record AzNetworkFrontDoorRoutingRuleCreateOptions(
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--frontend-endpoints")] string FrontendEndpoints,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--route-type")] string RouteType
) : AzOptions
{
    [CommandSwitch("--accepted-protocols")]
    public string? AcceptedProtocols { get; set; }

    [CommandSwitch("--backend-pool")]
    public string? BackendPool { get; set; }

    [CommandSwitch("--cache-duration")]
    public string? CacheDuration { get; set; }

    [CommandSwitch("--caching")]
    public string? Caching { get; set; }

    [CommandSwitch("--custom-forwarding-path")]
    public string? CustomForwardingPath { get; set; }

    [CommandSwitch("--custom-fragment")]
    public string? CustomFragment { get; set; }

    [CommandSwitch("--custom-host")]
    public string? CustomHost { get; set; }

    [CommandSwitch("--custom-path")]
    public string? CustomPath { get; set; }

    [CommandSwitch("--custom-query-string")]
    public string? CustomQueryString { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--dynamic-compression")]
    public string? DynamicCompression { get; set; }

    [CommandSwitch("--forwarding-protocol")]
    public string? ForwardingProtocol { get; set; }

    [CommandSwitch("--patterns")]
    public string? Patterns { get; set; }

    [CommandSwitch("--query-parameter-strip-directive")]
    public string? QueryParameterStripDirective { get; set; }

    [CommandSwitch("--query-parameters")]
    public string? QueryParameters { get; set; }

    [CommandSwitch("--redirect-protocol")]
    public string? RedirectProtocol { get; set; }

    [CommandSwitch("--redirect-type")]
    public string? RedirectType { get; set; }

    [CommandSwitch("--rules-engine")]
    public string? RulesEngine { get; set; }
}