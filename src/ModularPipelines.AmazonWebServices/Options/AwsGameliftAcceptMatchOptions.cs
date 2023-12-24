using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "accept-match")]
public record AwsGameliftAcceptMatchOptions(
[property: CommandSwitch("--ticket-id")] string TicketId,
[property: CommandSwitch("--player-ids")] string[] PlayerIds,
[property: CommandSwitch("--acceptance-type")] string AcceptanceType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}