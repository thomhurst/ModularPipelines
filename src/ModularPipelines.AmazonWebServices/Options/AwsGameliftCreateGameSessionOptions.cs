using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "create-game-session")]
public record AwsGameliftCreateGameSessionOptions(
[property: CliOption("--maximum-player-session-count")] int MaximumPlayerSessionCount
) : AwsOptions
{
    [CliOption("--fleet-id")]
    public string? FleetId { get; set; }

    [CliOption("--alias-id")]
    public string? AliasId { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--game-properties")]
    public string[]? GameProperties { get; set; }

    [CliOption("--creator-id")]
    public string? CreatorId { get; set; }

    [CliOption("--game-session-id")]
    public string? GameSessionId { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--game-session-data")]
    public string? GameSessionData { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}