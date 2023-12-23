using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "delete-game-server-group")]
public record AwsGameliftDeleteGameServerGroupOptions(
[property: CommandSwitch("--game-server-group-name")] string GameServerGroupName
) : AwsOptions
{
    [CommandSwitch("--delete-option")]
    public string? DeleteOption { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}