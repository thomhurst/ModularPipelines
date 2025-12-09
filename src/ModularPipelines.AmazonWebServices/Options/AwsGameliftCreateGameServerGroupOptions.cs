using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "create-game-server-group")]
public record AwsGameliftCreateGameServerGroupOptions(
[property: CliOption("--game-server-group-name")] string GameServerGroupName,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--min-size")] int MinSize,
[property: CliOption("--max-size")] int MaxSize,
[property: CliOption("--launch-template")] string LaunchTemplate,
[property: CliOption("--instance-definitions")] string[] InstanceDefinitions
) : AwsOptions
{
    [CliOption("--auto-scaling-policy")]
    public string? AutoScalingPolicy { get; set; }

    [CliOption("--balancing-strategy")]
    public string? BalancingStrategy { get; set; }

    [CliOption("--game-server-protection-policy")]
    public string? GameServerProtectionPolicy { get; set; }

    [CliOption("--vpc-subnets")]
    public string[]? VpcSubnets { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}