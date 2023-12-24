using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "update-game-server-group")]
public record AwsGameliftUpdateGameServerGroupOptions(
[property: CommandSwitch("--game-server-group-name")] string GameServerGroupName
) : AwsOptions
{
    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--instance-definitions")]
    public string[]? InstanceDefinitions { get; set; }

    [CommandSwitch("--game-server-protection-policy")]
    public string? GameServerProtectionPolicy { get; set; }

    [CommandSwitch("--balancing-strategy")]
    public string? BalancingStrategy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}