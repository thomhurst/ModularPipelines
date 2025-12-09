using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("resource", "move")]
public record AzResourceMoveOptions(
[property: CliOption("--destination-group")] string DestinationGroup,
[property: CliOption("--ids")] string Ids
) : AzOptions
{
    [CliOption("--destination-subscription-id")]
    public string? DestinationSubscriptionId { get; set; }
}