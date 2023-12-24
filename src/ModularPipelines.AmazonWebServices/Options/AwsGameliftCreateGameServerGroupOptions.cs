using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "create-game-server-group")]
public record AwsGameliftCreateGameServerGroupOptions(
[property: CommandSwitch("--game-server-group-name")] string GameServerGroupName,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--min-size")] int MinSize,
[property: CommandSwitch("--max-size")] int MaxSize,
[property: CommandSwitch("--launch-template")] string LaunchTemplate,
[property: CommandSwitch("--instance-definitions")] string[] InstanceDefinitions
) : AwsOptions
{
    [CommandSwitch("--auto-scaling-policy")]
    public string? AutoScalingPolicy { get; set; }

    [CommandSwitch("--balancing-strategy")]
    public string? BalancingStrategy { get; set; }

    [CommandSwitch("--game-server-protection-policy")]
    public string? GameServerProtectionPolicy { get; set; }

    [CommandSwitch("--vpc-subnets")]
    public string[]? VpcSubnets { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}