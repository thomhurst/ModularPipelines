using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "eventhub", "consumer-group", "create")]
public record AzEventhubsEventhubConsumerGroupCreateOptions(
[property: CommandSwitch("--consumer-group-name")] string ConsumerGroupName,
[property: CommandSwitch("--eventhub-name")] string EventhubName,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--user-metadata")]
    public string? UserMetadata { get; set; }
}

