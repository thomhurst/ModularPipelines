using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "eventhub", "authorization-rule", "keys", "renew")]
public record AzEventhubsEventhubAuthorizationRuleKeysRenewOptions(
[property: CommandSwitch("--key")] string Key
) : AzOptions
{
    [CommandSwitch("--authorization-rule-name")]
    public string? AuthorizationRuleName { get; set; }

    [CommandSwitch("--eventhub-name")]
    public string? EventhubName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--key-value")]
    public string? KeyValue { get; set; }

    [CommandSwitch("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}