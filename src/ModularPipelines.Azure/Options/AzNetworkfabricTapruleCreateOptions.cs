using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "taprule", "create")]
public record AzNetworkfabricTapruleCreateOptions(
[property: CommandSwitch("--configuration-type")] string ConfigurationType,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName
) : AzOptions
{
    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [CommandSwitch("--dynamic-match-configurations")]
    public string? DynamicMatchConfigurations { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--match-configurations")]
    public string? MatchConfigurations { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--polling-interval-in-seconds")]
    public string? PollingIntervalInSeconds { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tap-rules-url")]
    public string? TapRulesUrl { get; set; }
}