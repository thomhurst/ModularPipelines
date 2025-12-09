using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "waf-policy", "managed-rules", "add")]
public record AzNetworkFrontDoorWafPolicyManagedRulesAddOptions(
[property: CliOption("--type")] string Type,
[property: CliOption("--version")] string Version
) : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--policy-name")]
    public string? PolicyName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}