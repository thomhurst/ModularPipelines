using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "update-game-session")]
public record AwsGameliftUpdateGameSessionOptions(
[property: CommandSwitch("--game-session-id")] string GameSessionId
) : AwsOptions
{
    [CommandSwitch("--maximum-player-session-count")]
    public int? MaximumPlayerSessionCount { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--player-session-creation-policy")]
    public string? PlayerSessionCreationPolicy { get; set; }

    [CommandSwitch("--protection-policy")]
    public string? ProtectionPolicy { get; set; }

    [CommandSwitch("--game-properties")]
    public string[]? GameProperties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}