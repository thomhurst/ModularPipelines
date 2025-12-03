using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "update-game-session")]
public record AwsGameliftUpdateGameSessionOptions(
[property: CliOption("--game-session-id")] string GameSessionId
) : AwsOptions
{
    [CliOption("--maximum-player-session-count")]
    public int? MaximumPlayerSessionCount { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--player-session-creation-policy")]
    public string? PlayerSessionCreationPolicy { get; set; }

    [CliOption("--protection-policy")]
    public string? ProtectionPolicy { get; set; }

    [CliOption("--game-properties")]
    public string[]? GameProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}