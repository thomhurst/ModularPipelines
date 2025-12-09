using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "register-game-server")]
public record AwsGameliftRegisterGameServerOptions(
[property: CliOption("--game-server-group-name")] string GameServerGroupName,
[property: CliOption("--game-server-id")] string GameServerId,
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--connection-info")]
    public string? ConnectionInfo { get; set; }

    [CliOption("--game-server-data")]
    public string? GameServerData { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}