using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "describe-game-sessions")]
public record AwsGameliftDescribeGameSessionsOptions : AwsOptions
{
    [CliOption("--fleet-id")]
    public string? FleetId { get; set; }

    [CliOption("--game-session-id")]
    public string? GameSessionId { get; set; }

    [CliOption("--alias-id")]
    public string? AliasId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--status-filter")]
    public string? StatusFilter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}