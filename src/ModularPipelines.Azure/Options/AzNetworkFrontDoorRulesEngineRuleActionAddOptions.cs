using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "rules-engine", "rule", "action", "add")]
public record AzNetworkFrontDoorRulesEngineRuleActionAddOptions(
[property: CommandSwitch("--action-type")] string ActionType,
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rules-engine-name")] string RulesEngineName
) : AzOptions
{
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

    [CommandSwitch("--forwarding-protocol")]
    public string? ForwardingProtocol { get; set; }

    [CommandSwitch("--header-action")]
    public string? HeaderAction { get; set; }

    [CommandSwitch("--header-name")]
    public string? HeaderName { get; set; }

    [CommandSwitch("--header-value")]
    public string? HeaderValue { get; set; }

    [CommandSwitch("--query-parameter-strip-directive")]
    public string? QueryParameterStripDirective { get; set; }

    [CommandSwitch("--query-parameters")]
    public string? QueryParameters { get; set; }

    [CommandSwitch("--redirect-protocol")]
    public string? RedirectProtocol { get; set; }

    [CommandSwitch("--redirect-type")]
    public string? RedirectType { get; set; }
}

