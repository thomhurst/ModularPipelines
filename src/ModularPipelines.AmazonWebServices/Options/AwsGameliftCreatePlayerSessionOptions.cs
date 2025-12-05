using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "create-player-session")]
public record AwsGameliftCreatePlayerSessionOptions(
[property: CliOption("--game-session-id")] string GameSessionId,
[property: CliOption("--player-id")] string PlayerId
) : AwsOptions
{
    [CliOption("--player-data")]
    public string? PlayerData { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}