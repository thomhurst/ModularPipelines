using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "describe-player-sessions")]
public record AwsGameliftDescribePlayerSessionsOptions : AwsOptions
{
    [CliOption("--game-session-id")]
    public string? GameSessionId { get; set; }

    [CliOption("--player-id")]
    public string? PlayerId { get; set; }

    [CliOption("--player-session-id")]
    public string? PlayerSessionId { get; set; }

    [CliOption("--player-session-status-filter")]
    public string? PlayerSessionStatusFilter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}