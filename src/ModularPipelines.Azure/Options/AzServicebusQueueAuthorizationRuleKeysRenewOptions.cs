using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "queue", "authorization-rule", "keys", "renew")]
public record AzServicebusQueueAuthorizationRuleKeysRenewOptions(
[property: CliOption("--key")] string Key
) : AzOptions
{
    [CliOption("--authorization-rule-name")]
    public string? AuthorizationRuleName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key-value")]
    public string? KeyValue { get; set; }

    [CliOption("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CliOption("--queue-name")]
    public string? QueueName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}