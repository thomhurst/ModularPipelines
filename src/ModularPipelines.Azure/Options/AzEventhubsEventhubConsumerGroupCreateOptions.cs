using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventhubs", "eventhub", "consumer-group", "create")]
public record AzEventhubsEventhubConsumerGroupCreateOptions(
[property: CliOption("--consumer-group-name")] string ConsumerGroupName,
[property: CliOption("--eventhub-name")] string EventhubName,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--user-metadata")]
    public string? UserMetadata { get; set; }
}