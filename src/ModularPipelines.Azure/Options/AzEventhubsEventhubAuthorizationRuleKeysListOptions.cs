using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "eventhub", "authorization-rule", "keys", "list")]
public record AzEventhubsEventhubAuthorizationRuleKeysListOptions(
[property: CommandSwitch("--authorization-rule-name")] string AuthorizationRuleName,
[property: CommandSwitch("--eventhub-name")] string EventhubName,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--key-value")]
    public string? KeyValue { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

