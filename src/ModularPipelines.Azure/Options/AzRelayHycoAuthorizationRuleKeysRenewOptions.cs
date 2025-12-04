using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("relay", "hyco", "authorization-rule", "keys", "renew")]
public record AzRelayHycoAuthorizationRuleKeysRenewOptions(
[property: CliOption("--key")] string Key
) : AzOptions
{
    [CliOption("--hybrid-connection-name")]
    public string? HybridConnectionName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key-value")]
    public string? KeyValue { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}