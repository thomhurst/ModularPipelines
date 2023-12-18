using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-policy", "create")]
public record AzNetworkApplicationGatewayWafPolicyCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--custom-rules")]
    public string? CustomRules { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-rules")]
    public string? ManagedRules { get; set; }

    [CommandSwitch("--policy-settings")]
    public string? PolicySettings { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}