using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "get-game-session-log")]
public record AwsGameliftGetGameSessionLogOptions(
[property: CommandSwitch("--game-session-id")] string GameSessionId,
[property: CommandSwitch("--save-as")] string SaveAs
) : AwsOptions;