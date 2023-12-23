using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "suspend-game-server-group")]
public record AwsGameliftSuspendGameServerGroupOptions(
[property: CommandSwitch("--game-server-group-name")] string GameServerGroupName,
[property: CommandSwitch("--suspend-actions")] string[] SuspendActions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}