using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "consumer-group", "create")]
public record AzIotHubConsumerGroupCreateOptions(
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--event-hub-name")]
    public string? EventHubName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}