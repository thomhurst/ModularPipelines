using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signalr", "network-rule", "update")]
public record AzSignalrNetworkRuleUpdateOptions : AzOptions
{
    [BooleanCommandSwitch("--allow")]
    public bool? Allow { get; set; }

    [CommandSwitch("--connection-name")]
    public string? ConnectionName { get; set; }

    [CommandSwitch("--deny")]
    public string? Deny { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--public-network")]
    public bool? PublicNetwork { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}