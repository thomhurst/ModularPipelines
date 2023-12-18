using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

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