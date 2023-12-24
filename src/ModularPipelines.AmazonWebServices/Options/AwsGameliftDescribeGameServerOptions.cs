using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "describe-game-server")]
public record AwsGameliftDescribeGameServerOptions(
[property: CommandSwitch("--game-server-group-name")] string GameServerGroupName,
[property: CommandSwitch("--game-server-id")] string GameServerId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}