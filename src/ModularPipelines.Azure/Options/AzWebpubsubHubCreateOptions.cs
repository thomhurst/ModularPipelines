using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webpubsub", "hub", "create")]
public record AzWebpubsubHubCreateOptions(
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--allow-anonymous")]
    public bool? AllowAnonymous { get; set; }

    [CommandSwitch("--event-handler")]
    public string? EventHandler { get; set; }
}

