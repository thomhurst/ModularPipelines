using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "routing-rule", "update")]
public record AzNetworkFrontDoorRoutingRuleUpdateOptions(
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--accepted-protocols")]
    public string? AcceptedProtocols { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

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

    [CommandSwitch("--dynamic-compression")]
    public string? DynamicCompression { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--forwarding-protocol")]
    public string? ForwardingProtocol { get; set; }

    [CommandSwitch("--frontend-endpoints")]
    public string? FrontendEndpoints { get; set; }

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

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--rules-engine")]
    public string? RulesEngine { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}

