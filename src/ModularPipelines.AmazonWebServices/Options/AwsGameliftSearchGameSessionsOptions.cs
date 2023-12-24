using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "search-game-sessions")]
public record AwsGameliftSearchGameSessionsOptions : AwsOptions
{
    [CommandSwitch("--fleet-id")]
    public string? FleetId { get; set; }

    [CommandSwitch("--alias-id")]
    public string? AliasId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--filter-expression")]
    public string? FilterExpression { get; set; }

    [CommandSwitch("--sort-expression")]
    public string? SortExpression { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}