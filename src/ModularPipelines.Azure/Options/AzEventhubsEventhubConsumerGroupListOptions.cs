using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs", "eventhub", "consumer-group", "list")]
public record AzEventhubsEventhubConsumerGroupListOptions(
[property: CliOption("--eventhub-name")] string EventhubName,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--skip")]
    public string? Skip { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}