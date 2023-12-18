using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource", "move")]
public record AzResourceMoveOptions(
[property: CommandSwitch("--destination-group")] string DestinationGroup,
[property: CommandSwitch("--ids")] string Ids
) : AzOptions
{
    [CommandSwitch("--destination-subscription-id")]
    public string? DestinationSubscriptionId { get; set; }
}