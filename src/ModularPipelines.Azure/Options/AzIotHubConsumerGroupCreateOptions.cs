using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "consumer-group", "create")]
public record AzIotHubConsumerGroupCreateOptions(
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--event-hub-name")]
    public string? EventHubName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

