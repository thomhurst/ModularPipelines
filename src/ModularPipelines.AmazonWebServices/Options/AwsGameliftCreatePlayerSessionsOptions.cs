using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "create-player-sessions")]
public record AwsGameliftCreatePlayerSessionsOptions(
[property: CommandSwitch("--game-session-id")] string GameSessionId,
[property: CommandSwitch("--player-ids")] string[] PlayerIds
) : AwsOptions
{
    [CommandSwitch("--player-data-map")]
    public IEnumerable<KeyValue>? PlayerDataMap { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}