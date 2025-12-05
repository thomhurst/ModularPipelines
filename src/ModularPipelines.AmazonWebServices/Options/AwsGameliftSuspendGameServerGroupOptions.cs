using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "suspend-game-server-group")]
public record AwsGameliftSuspendGameServerGroupOptions(
[property: CliOption("--game-server-group-name")] string GameServerGroupName,
[property: CliOption("--suspend-actions")] string[] SuspendActions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}