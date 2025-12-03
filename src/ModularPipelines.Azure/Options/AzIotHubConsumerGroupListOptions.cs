using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "consumer-group", "list")]
public record AzIotHubConsumerGroupListOptions : AzOptions
{
    [CliOption("--event-hub-name")]
    public string? EventHubName { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}