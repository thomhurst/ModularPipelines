using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "resume-game-server-group")]
public record AwsGameliftResumeGameServerGroupOptions(
[property: CommandSwitch("--game-server-group-name")] string GameServerGroupName,
[property: CommandSwitch("--resume-actions")] string[] ResumeActions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}