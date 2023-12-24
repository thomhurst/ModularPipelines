using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "create-player-session")]
public record AwsGameliftCreatePlayerSessionOptions(
[property: CommandSwitch("--game-session-id")] string GameSessionId,
[property: CommandSwitch("--player-id")] string PlayerId
) : AwsOptions
{
    [CommandSwitch("--player-data")]
    public string? PlayerData { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}