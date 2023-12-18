using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

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