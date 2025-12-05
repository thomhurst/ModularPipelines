using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "deregister-game-server")]
public record AwsGameliftDeregisterGameServerOptions(
[property: CliOption("--game-server-group-name")] string GameServerGroupName,
[property: CliOption("--game-server-id")] string GameServerId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}