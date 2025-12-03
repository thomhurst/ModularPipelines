using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "waf-policy", "create")]
public record AzNetworkApplicationGatewayWafPolicyCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--custom-rules")]
    public string? CustomRules { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-rules")]
    public string? ManagedRules { get; set; }

    [CliOption("--policy-settings")]
    public string? PolicySettings { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}