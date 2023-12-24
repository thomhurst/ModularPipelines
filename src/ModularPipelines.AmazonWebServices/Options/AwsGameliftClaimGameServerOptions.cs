using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "claim-game-server")]
public record AwsGameliftClaimGameServerOptions(
[property: CommandSwitch("--game-server-group-name")] string GameServerGroupName
) : AwsOptions
{
    [CommandSwitch("--game-server-id")]
    public string? GameServerId { get; set; }

    [CommandSwitch("--game-server-data")]
    public string? GameServerData { get; set; }

    [CommandSwitch("--filter-option")]
    public string? FilterOption { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}