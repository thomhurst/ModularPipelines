using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "resume-game-server-group")]
public record AwsGameliftResumeGameServerGroupOptions(
[property: CliOption("--game-server-group-name")] string GameServerGroupName,
[property: CliOption("--resume-actions")] string[] ResumeActions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}