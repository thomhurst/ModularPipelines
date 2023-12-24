using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "describe-player-sessions")]
public record AwsGameliftDescribePlayerSessionsOptions : AwsOptions
{
    [CommandSwitch("--game-session-id")]
    public string? GameSessionId { get; set; }

    [CommandSwitch("--player-id")]
    public string? PlayerId { get; set; }

    [CommandSwitch("--player-session-id")]
    public string? PlayerSessionId { get; set; }

    [CommandSwitch("--player-session-status-filter")]
    public string? PlayerSessionStatusFilter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}