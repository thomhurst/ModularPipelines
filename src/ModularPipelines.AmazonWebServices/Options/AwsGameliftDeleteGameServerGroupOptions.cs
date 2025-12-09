using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "delete-game-server-group")]
public record AwsGameliftDeleteGameServerGroupOptions(
[property: CliOption("--game-server-group-name")] string GameServerGroupName
) : AwsOptions
{
    [CliOption("--delete-option")]
    public string? DeleteOption { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}