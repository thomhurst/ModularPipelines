using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "create-game-session")]
public record AwsGameliftCreateGameSessionOptions(
[property: CommandSwitch("--maximum-player-session-count")] int MaximumPlayerSessionCount
) : AwsOptions
{
    [CommandSwitch("--fleet-id")]
    public string? FleetId { get; set; }

    [CommandSwitch("--alias-id")]
    public string? AliasId { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--game-properties")]
    public string[]? GameProperties { get; set; }

    [CommandSwitch("--creator-id")]
    public string? CreatorId { get; set; }

    [CommandSwitch("--game-session-id")]
    public string? GameSessionId { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--game-session-data")]
    public string? GameSessionData { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}