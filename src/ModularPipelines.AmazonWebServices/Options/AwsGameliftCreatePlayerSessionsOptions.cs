using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "create-player-sessions")]
public record AwsGameliftCreatePlayerSessionsOptions(
[property: CliOption("--game-session-id")] string GameSessionId,
[property: CliOption("--player-ids")] string[] PlayerIds
) : AwsOptions
{
    [CliOption("--player-data-map")]
    public IEnumerable<KeyValue>? PlayerDataMap { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}