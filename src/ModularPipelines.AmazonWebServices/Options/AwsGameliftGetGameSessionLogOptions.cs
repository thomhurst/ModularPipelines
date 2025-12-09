using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "get-game-session-log")]
public record AwsGameliftGetGameSessionLogOptions(
[property: CliOption("--game-session-id")] string GameSessionId,
[property: CliOption("--save-as")] string SaveAs
) : AwsOptions;