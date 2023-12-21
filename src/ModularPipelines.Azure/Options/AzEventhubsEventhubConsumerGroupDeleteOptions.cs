using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "eventhub", "consumer-group", "delete")]
public record AzEventhubsEventhubConsumerGroupDeleteOptions : AzOptions
{
    [CommandSwitch("--consumer-group-name")]
    public string? ConsumerGroupName { get; set; }

    [CommandSwitch("--eventhub-name")]
    public string? EventhubName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}