using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "describe-game-session-details")]
public record AwsGameliftDescribeGameSessionDetailsOptions : AwsOptions
{
    [CommandSwitch("--fleet-id")]
    public string? FleetId { get; set; }

    [CommandSwitch("--game-session-id")]
    public string? GameSessionId { get; set; }

    [CommandSwitch("--alias-id")]
    public string? AliasId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--status-filter")]
    public string? StatusFilter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}