using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "deregister-game-server")]
public record AwsGameliftDeregisterGameServerOptions(
[property: CommandSwitch("--game-server-group-name")] string GameServerGroupName,
[property: CommandSwitch("--game-server-id")] string GameServerId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}