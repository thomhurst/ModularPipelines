using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "update-game-server-group")]
public record AwsGameliftUpdateGameServerGroupOptions(
[property: CliOption("--game-server-group-name")] string GameServerGroupName
) : AwsOptions
{
    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--instance-definitions")]
    public string[]? InstanceDefinitions { get; set; }

    [CliOption("--game-server-protection-policy")]
    public string? GameServerProtectionPolicy { get; set; }

    [CliOption("--balancing-strategy")]
    public string? BalancingStrategy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}