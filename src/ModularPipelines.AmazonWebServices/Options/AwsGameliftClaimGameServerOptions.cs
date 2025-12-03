using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "claim-game-server")]
public record AwsGameliftClaimGameServerOptions(
[property: CliOption("--game-server-group-name")] string GameServerGroupName
) : AwsOptions
{
    [CliOption("--game-server-id")]
    public string? GameServerId { get; set; }

    [CliOption("--game-server-data")]
    public string? GameServerData { get; set; }

    [CliOption("--filter-option")]
    public string? FilterOption { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}